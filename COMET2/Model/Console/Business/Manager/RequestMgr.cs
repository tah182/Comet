using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading.Tasks;

using COMET.Model.Business.Factory;
using COMET.Model.Console.Business.Service;
using COMET.Model.Console.Domain;
using COMET.Model.Console.Domain.View;
using COMET.Model.Domain;
using COMET.Server.Domain;

using COMET.Model.Business.Service;

namespace COMET.Model.Business.Manager {
    public class RequestMgr : IManager {
        private ApplicationStore<IList<RequestView>> requestList;
        private ApplicationStore<IList<LookupSorted>> elementStatusList;
        private ApplicationStore<IList<LookupSorted>> requestStatusList;
        private ApplicationStore<IList<LookupSorted>> projectStatusList;

        private readonly IRequestSvc svc;
        private readonly string CONFIG_EMAIL = "ConsoleFrom";

        public RequestMgr(IRequestSvc svc) {
            this.svc = svc;
            this.requestList = (ApplicationStore<IList<RequestView>>)HttpContext.Current.Application["Request"];
            this.elementStatusList = (ApplicationStore<IList<LookupSorted>>)HttpContext.Current.Application["ElementStatus"];
            this.requestStatusList = (ApplicationStore<IList<LookupSorted>>)HttpContext.Current.Application["RequestStatus"];
            this.projectStatusList = (ApplicationStore<IList<LookupSorted>>)HttpContext.Current.Application["ProjectStatus"];

            if (this.requestList == null || this.elementStatusList == null || this.requestStatusList == null || this.projectStatusList == null || 
                !this.requestList.isValid() || !this.elementStatusList.isValid() || !this.requestStatusList.isValid() || !this.projectStatusList.isValid())
                refresh();
        }

        public IList<LookupSorted> getStatusList(EOpenType type) {
            switch (type) {
                case EOpenType.Request:
                    return this.requestStatusList.Data.OrderBy(x => x.SortOrder).ToList();
                case EOpenType.Element:
                    return this.elementStatusList.Data.OrderBy(x => x.SortOrder).ToList();
                default:
                    return this.projectStatusList.Data.OrderBy(x => x.SortOrder).ToList();
            }
        }

        public ARequestView createRequest(ARequestView request) {
            if (request.GetType() == typeof(RequestView)) {
                LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
                UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
                int defaultID = Int32.Parse((string)MainFactory.getConfiguration().get("BIManagerID"));
                int supportID = lookupMgr.getSupportAreas().Where(x => x.ID == ((RequestView)request).SupportArea.ID).FirstOrDefault().DeveloperID ?? defaultID;
                if (((RequestView)request).CType.ID == 3 || ((RequestView)request).CType.ID == 7)
                    supportID = lookupMgr.getSupportAreas().Where(x => x.ID == ((RequestView)request).SupportArea.ID).FirstOrDefault().SupportID ?? defaultID;

                // auto select program based off support area.
                ((RequestView)request).Program = lookupMgr.getPrograms().Where(x => x.ID == this.svc.getProgramFromSupportArea(((RequestView)request).SupportArea.ID)).Cast<Program>().FirstOrDefault();

                ((RequestView)request).AssignedTo = (User)userMgr.getUser(supportID);
                request.setLastUpdated();
                RequestView requestView = (RequestView)request;
                if (((RequestView)request).AssignedTo.EmployeeID != defaultID)
                    requestView = (RequestView)this.setStatusID((AProjectView)request, 2);
                else
                    requestView = (RequestView)this.setStatusID((AProjectView)request, 1);

                Configuration config = MainFactory.getConfiguration();
                if (((RequestView)request).ValueDriver.ID != 1)
                    ((RequestView)request).Value = ((RequestView)request).Value * Decimal.Parse((string)config.get("HoursCostMultiplierExternal"));

                requestView = convertRequest(this.svc.saveRequest(requestView));

                this.requestList.Data.Add(requestView);
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application["Request"] = this.requestList;
                HttpContext.Current.Application.UnLock();
                return requestView;
            }
            throw new NotSupportedException("Cannot createa a request that is not a project or a request type at this time.");
        }
        
        public IList<ARequestView> getRequests(EOpenType type, IUser user) {
            List<ARequestView> items = new List<ARequestView>();
            switch (type) {
                case EOpenType.Request :
                    if (user != null)
                        return this.requestList
                            .Data
                            .Where(x => ((x.AssignedTo.EmployeeID == user.EmployeeID || x.RequestedBy.EmployeeID == user.EmployeeID) && x.ClosedDate == null) || (user == null))
                            .Cast<ARequestView>()
                            .ToList();
                    else
                        return this.requestList.Data.Cast<ARequestView>().ToList();
                case EOpenType.Project :
                    foreach (RequestView r in this.requestList.Data)
                        if (r.AssignedTo.Equals(user) && new[] { "complete", "cancelled", "rejected", "out of scope" }.All(x => !x.Equals(r.Parent == null ? "complete" : r.Parent.Status.Text.ToLower())))
                            if (!items.Any(x => x.ID == r.Parent.ID))
                                items.Add(r.Parent);
                    return items.OrderBy(x => x.Summary).ToList();
                case EOpenType.Element :
                    foreach (RequestView r in this.requestList.Data)
                        items.AddRange(r.ElementList.Where(x => x.AssignedTo.EmployeeID == user.EmployeeID && x.ClosedDate == null));
                    return items;
                default:
                    return items;
            }
        }

        public Note saveNote(Note note, int parentID) {
            note.Parent = getElement(parentID);
            Note n = convertNote(this.svc.saveNote(note));

            Parallel.ForEach(this.requestList.Data, rv => {
                Parallel.ForEach(rv.ElementList, ev => {
                    if (ev.ID == parentID)
                        ev.addNote(n);
                });
            });

            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["Request"] = this.requestList;
            HttpContext.Current.Application.UnLock();
            return n;
        }

        /// <summary>
        /// Saves a new element into the requestList Tree. \n
        /// Point to parent Request
        /// Add element to parent Request ElementList
        /// </summary>
        /// <param name="element">The new element to add</param>
        /// <returns>The updated element with the pointers updated</returns>
        public ElementView saveElement(ElementView element) {
            if (element.Summary == null || element.Summary.Length < 1)
                throw new ArgumentException("Summary is Required.");

            element.Status = this.elementStatusList.Data.Where(x => x.ID == element.Status.ID).FirstOrDefault();
            
            element.Status = this.elementStatusList.Data.Where(x => x.ID == element.Status.ID).FirstOrDefault();
            string status = element.Status.Text.ToLower();
            element.isNew = false;
            if (status.Equals("complete") || status.Equals("rejected")) {
                if (element.Resolution == null || element.Resolution.Length < 1)
                    throw new ArgumentException("You are closing this element. Please provide a resolution.");
                element.setClosed();
            }
            element.setLastUpdated();

            ElementView e = convertElement(this.svc.saveElement(element));
            RequestView r = getRequest(element.Parent.ID);
            e.Parent = r;
            r.ElementList.Add(e);

            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["Request"] = this.requestList;
            HttpContext.Current.Application.UnLock();
            return e;
        }
        
        /// <summary>
        /// Updates a element's property by creating a new element, 
        /// updating the parent and child pointers
        /// repoints the parent's child pointer to this
        /// repoints the children's parent pointer to this.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public ElementView updateElement(ElementView element, bool autoclose) {
            ElementView oldElement = this.requestList.Data.Where(x => x.ID == element.Parent.ID).FirstOrDefault().ElementList.Where(y => y.ID == element.ID).FirstOrDefault();

            if (!autoclose && oldElement.Status.ID == element.Status.ID && this.elementStatusList.Data.Where(x => x.ID == element.Status.ID).FirstOrDefault().Text.Equals("Complete"))
                throw new InvalidOperationException("The status is closed. You cannot update an element that is closed.");

            element.isNew = false;
            element.Status = this.elementStatusList.Data.Where(x => x.ID == element.Status.ID).FirstOrDefault();
            string status = element.Status.Text.ToLower();
            if (status.Equals("complete") || status.Equals("rejected")) {
                if (element.Resolution == null || element.Resolution.Length < 1)
                    throw new ArgumentException("You are closing this element. Please provide a resolution.");
                element.setClosed();
            }

            element.setLastUpdated();
            

            ElementView e = convertElement(this.svc.updateElement(element));
            
            e.Parent = oldElement.Parent;
            Parallel.ForEach(oldElement.Note, note => {
                note.Parent = e;
            });
            e.Note = oldElement.Note;

            // change the pointer in the requestList to point to this object
            foreach (RequestView rv in this.requestList.Data.ToList())
                foreach (ElementView ev in rv.ElementList.ToList())
                    if (ev.ID == element.ID) {
                        rv.ElementList.Remove(ev);
                        rv.ElementList.Add(e);
                        continue;
                    }


            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["Request"] = this.requestList;
            HttpContext.Current.Application.UnLock();
            return e;
        }

        public RequestView updateRequest(RequestView request) {
            LookupMgr lookupMgr = new LookupMgr(this.svc);
            RequestView oldRequest = getRequest(request.ID);

            string from = (string)MainFactory.getConfiguration().get(CONFIG_EMAIL);

            if (request.EstimatedDueDate == null)
                throw new ArgumentException("Estimated Due Date cannot be empty.");

            string status = this.requestStatusList.Data.Where(x => x.ID == request.Status.ID).FirstOrDefault().Text.ToLower();
            request.Status.Text = status;
            // update elements
            if (status.Equals("complete") && request.Status.ID != oldRequest.Status.ID) {
                if (request.Resolution == null || request.Resolution.Length < 1)
                    throw new ArgumentException("You are closing this request. Please provide a resolution.");
                foreach (ElementView element in oldRequest.ElementList.ToList()) {
                    if (element.ClosedDate == null) {
                        if (!element.AssignedTo.Equals(request.AssignedTo))
                            throw new InvalidOperationException("There are elements that are not assigned to you. You cannot close this request until these elements are closed manually.");

                        element.PercentComplete = 100;
                        element.Resolution = "AutoStamp: " + request.Resolution;
                        element.Status = this.elementStatusList.Data.Where(x => x.Text.Equals("Complete")).FirstOrDefault();
                        updateElement(element, true);
                    }
                }
                request.setClosed();
                EmailSvc.Email(from,
                                request.RequestedBy.EmailAddress,
                                "",
                                "Request " + request.ID + " - " + request.Summary + " has been completed",
                                ConsoleFactory.requestEmailSubmitterStatusChange(request));
            } else if ((status.Equals("rejected") || status.Equals("cancelled")) && request.Status.ID != oldRequest.Status.ID) {
                if (request.Resolution == null || request.Resolution.Length < 1)
                    throw new ArgumentException("You are closing this request. Please provide a resolution.");
                request.setClosed();
                EmailSvc.Email(from,
                                request.RequestedBy.EmailAddress,
                                "",
                                "Request " + request.ID + " - " + request.Summary + " has been " + request.Resolution + ".",
                                ConsoleFactory.requestEmailSubmitterStatusChange(request));
            } else if (status.Equals("on hold") && request.Status.ID != oldRequest.Status.ID)
                request.setHoldDate();
            else if (oldRequest.Status.Text.ToLower().Equals("on hold") && request.Status.ID != oldRequest.Status.ID)
                request.setResumeDate();
            else if (status.Equals("moved to project") && request.Status.ID != oldRequest.Status.ID) {
                UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());

                if (request.EstimatedHours == null || request.EstimatedHours <= 0)
                    throw new ArgumentException("Request must have estimated hours before being requested for promotion.");

                EmailSvc.Email(from,
                                userMgr.getUser(Int32.Parse((string)MainFactory.getConfiguration().get("BIManagerID"))).EmailAddress,
                                "",
                                "Request " + request.ID + " - " + request.Summary + " has been updated",
                                ConsoleFactory.requestEmailPromoteBody(request));
                request.ManagerQueueDate = DateTime.Today;
            }

            // hours are different. update cost
            if (request.EstimatedHours != oldRequest.EstimatedHours)
                request.EstimatedCost = request.EstimatedHours * request.InternalHoursMultiplier;

            request.ActualCost = oldRequest.ElementList.Sum(x => x.Hours) * request.InternalHoursMultiplier;
            request.isNew = false;
            request.setLastUpdated();

            RequestView r = convertRequest(this.svc.updateRequest(request));
            
            //change oldRequest's child Element's parent pointer to this
            foreach (ElementView e in oldRequest.ElementList)
                e.Parent = r;

            r.ElementList = oldRequest.ElementList;
            
            // change Parent's pointer to this
            if (r.Parent != null) {
                r.Parent.RequestList.Remove(r);
                r.Parent.RequestList.Add(r);
            }

            // if a u parent was added in this update.
            if (r.Parent == null && request.Parent != null && request.Parent.ID != 0) {
                foreach (RequestView rv in this.requestList.Data)
                    if (rv.Parent != null && rv.Parent.ID == request.Parent.ID) {
                        r.Parent = rv.Parent;
                        r.Parent.RequestList.Add(r);
                        break;
                    }
            }

            // change the pointer in the requestList to point to this object
            foreach (RequestView rv in this.requestList.Data.ToList())
                if (rv.ID == request.ID)
                    this.requestList.Data.Remove(rv);
            this.requestList.Data.Add(r);

            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["Request"] = this.requestList;
            HttpContext.Current.Application.UnLock();
            return r;
        }

        public ProjectView saveProject(RequestView request, DateTime startDate) {
            if (request.Parent != null && request.Parent.ID != 0)
                throw new ArgumentException("Request is already part of a project. It cannot be promoted to project.");
            LookupMgr lookupMgr = new LookupMgr(this.svc);
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            request.SubmittedBy = (User)userMgr.getUser(request.SubmittedBy.EmployeeID);
            request.RequestedBy = (User)userMgr.getUser(request.RequestedBy.EmployeeID);
            request.AssignedTo = (User)userMgr.getUser(request.AssignedTo.EmployeeID);
            request.Program = lookupMgr.getPrograms().Where(x => x.ID == request.Program.ID).Cast<Program>().FirstOrDefault();
            request.RequestCategory = lookupMgr.getRequestCategories(true).Where(x => x.ID == request.RequestCategory.ID).FirstOrDefault();
            request.Status = this.requestStatusList.Data.Where(x => x.Text.ToLower().Equals("moved to project")).FirstOrDefault();
            request.CType = lookupMgr.getRequestTypes(EOpenType.Request, true).Where(x => x.ID == request.CType.ID).FirstOrDefault();
            request.SupportArea = lookupMgr.getSupportAreas().Where(x => x.ID == request.SupportArea.ID).FirstOrDefault();
            request.ValueDriver = lookupMgr.getValueDrivers().Where(x => x.ID == request.ValueDriver.ID).Cast<ValueDriver>().FirstOrDefault();
                                    
            ProjectView project = new ProjectView(request);
            project.Status = this.projectStatusList.Data.Where(x => x.Text.Equals("Pending")).FirstOrDefault();
            project.StartDate = startDate;
            project.ManagerApprovedDate = DateTime.Today;
            project = convertProject(this.svc.saveProject(project));
            request.Parent = project;
            request.isNew = false;
            request.setLastUpdated();
            
            project.RequestList.Add(getRequest(request.ID));

            updateRequest(request);

            // update pointer and status
            foreach (RequestView rv in this.requestList.Data.ToList()) {
                if (rv.ID == request.ID) {
                    request.ElementList = rv.ElementList;
                    this.requestList.Data.Remove(rv);
                }
            }
            this.requestList.Data.Add(request);

            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["Request"] = this.requestList;
            HttpContext.Current.Application.UnLock();

            return project;
        }

        public ProjectView updateProject(ProjectView project) {
            LookupMgr lookupMgr = new LookupMgr(this.svc);
            ProjectView oldProject = getProject(project.ID);

            string status = this.projectStatusList.Data.Where(x => x.ID == project.Status.ID).FirstOrDefault().Text.ToLower();
            // check if everything is closed
            if (status.Equals("complete") && project.Status.ID != oldProject.Status.ID) {
                foreach (RequestView request in oldProject.RequestList) {
                    if (request.ClosedDate == null)
                        throw new InvalidOperationException("There are still requests open under this project. Please close these manually for accuracy.");
                }
                project.setClosed();
            } else if ((status.Equals("rejected") || status.Equals("cancelled")) && project.Status.ID != oldProject.Status.ID)
                project.setClosed();
            else if (status.Equals("on hold") && project.Status.ID != oldProject.Status.ID)
                project.setHoldDate();
            else if (oldProject.Status.Text.ToLower().Equals("on hold") && project.Status.ID != oldProject.Status.ID)
                project.setResumeDate();
            // hours are different. update cost
            if (project.EstimatedHours != project.EstimatedHours)
                project.EstimatedCost = project.EstimatedHours * project.InternalHoursMultiplier;

            project.ActualCost = oldProject.RequestList.Sum(x => x.Hours) * project.InternalHoursMultiplier;
            project.isNew = false;
            project.setLastUpdated();

            ProjectView p = convertProject(this.svc.updateProject(project));

            //change project's child request's parent pointer to this
            foreach (RequestView r in oldProject.RequestList)
                r.Parent = p;

            p.RequestList = oldProject.RequestList;

            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["Request"] = this.requestList;
            HttpContext.Current.Application.UnLock();

            return p;
        }

        public ElementView createElement(int parentID) {
            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            
            ElementView element = new ElementView(getRequest(parentID));

            return element;
        }

        public ElementView getElement(int id) {
            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            ElementView element = null;
            foreach (RequestView r in this.requestList.Data)
                foreach (ElementView e in r.ElementList)
                    if (e.ID == id)
                        element = e;

            return element;
        }

        public RequestView getRequest(int id) {
            RequestView request = this.requestList.Data.Where(x => x.ID == id).FirstOrDefault();
            return request;
        }

        public ProjectView getProject(int id) {
            ProjectView project = null;
            Parallel.ForEach(this.requestList.Data, request => {
                if (request.Parent != null && request.Parent.ID == id)
                    project = request.Parent;
            });

            return project;
        }

        public IList<ProjectView> getProjects() {
            IList<ProjectView> project = new List<ProjectView>();
            foreach (RequestView r in this.requestList.Data)
                if (r.Parent != null && !project.Any(x => x.ID == r.Parent.ID)) {
                    r.Parent.Summary = r.Parent.Summary.Left(50);
                    project.Add(r.Parent);
                }

            return project.OrderBy(y => y.Summary).OrderByDescending(x => x.ClosedDate == null).ToList();
        }

        public IList<Link> getItemsLike(string text) {
            List<Link> items = new List<Link>();
            string linkPre = "Partial";
            //string linkPre = "";

            // Reqeusts
            foreach (RequestView request in this.requestList.Data.OrderByDescending(x => x.LastUpdated)) {
                ProjectView project = request.Parent;
                string preText = "<b>Project: </b>";
                int id;
                string link = "" ;
                if (project != null) {
                    id = project.ID;
                    link = linkPre + "Project/" + id;
                    if (project.ID.ToString().Equals(text))
                        items.Add(new Link(preText + "ID - <mark>" + id + "</mark>", "Project#" + project.ID, link));
                    if (project.Summary.ToLower().Contains(text))
                        items.Add(new Link(preText + "Summary - " + getNearSearchHtml(project.Summary, text), "Project#" + project.ID, link));
                    if (project.Description.ToLower().Contains(text))
                        items.Add(new Link(preText + "Description - " + getNearSearchHtml(project.Description, text), "Project#" + project.ID, link));
                    if (project.ValueReason.ToLower().Contains(text))
                        items.Add(new Link(preText + "Value Reason - " + getNearSearchHtml(project.ValueReason, text), "Project#" + project.ID, link));
                }

                id = request.ID;
                preText = "<b>Request: </b>";
                link = linkPre + "Request/" + id;
                if (request.ID.ToString().Equals(text))
                    items.Add(new Link(preText + "ID - <mark>" + id + "</mark>", "Request#" + request.ID, link));
                if (request.Summary.ToLower().Contains(text))
                    items.Add(new Link(preText + "Summary - " + getNearSearchHtml(request.Summary, text), "Request#" + request.ID, link));
                if (request.Description.ToLower().Contains(text))
                    items.Add(new Link(preText + "Description - " + getNearSearchHtml(request.Description, text), "Request#" + request.ID, link));
                if (request.ValueReason.ToLower().Contains(text))
                    items.Add(new Link(preText + "Value reason - " + getNearSearchHtml(request.ValueReason, text), "Request#" + request.ID, link));

                foreach (ElementView element in request.ElementList) {
                    id = element.ID;
                    preText = "<b>Element: </b>";
                    link = linkPre + "Element/" + id;
                    if (element.ID.ToString().Equals(text))
                        items.Add(new Link(preText + "ID - <mark>" + id + "</mark>", "Element#" + element.ID, link));
                    if (element.Summary.ToLower().Contains(text))
                        items.Add(new Link(preText + "Summary - " + getNearSearchHtml(element.Summary, text), "Element#" + element.ID, link));
                    if (element.Resolution != null && element.Resolution.ToLower().Contains(text))
                        items.Add(new Link(preText + "Resolution - " + getNearSearchHtml(element.Resolution, text), "Element#" + element.ID, link));
                    foreach (Note note in element.Note) 
                        if (note.Text.ToLower().Contains(text))
                            items.Add(new Link(preText + "Note - " + getNearSearchHtml(note.Text, text), "Element#" + element.ID, link));
                }
            }
            items = items.GroupBy(x => new { x.RelativeUrl, x.ToolTip, x.Text }).Select(y => y.First()).ToList();
            return items.OrderBy(y => y.Text).OrderByDescending(x => x.Text.Left(1)).ToList();
        }

        public void refresh() {
            this.elementStatusList = new ApplicationStore<IList<LookupSorted>>(MainFactory.getCacheExpiration(), this.svc.getElementStatus());
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["ElementStatus"] = this.elementStatusList;

            this.requestStatusList = new ApplicationStore<IList<LookupSorted>>(MainFactory.getCacheExpiration(), this.svc.getRequestStatus());
            HttpContext.Current.Application["RequestStatus"] = this.requestStatusList;

            this.projectStatusList = new ApplicationStore<IList<LookupSorted>>(MainFactory.getCacheExpiration(), this.svc.getProjectStatus());
            HttpContext.Current.Application["ProjectStatus"] = this.projectStatusList;
            
            List<RequestView> requests = new List<RequestView>();

            this.requestList = new ApplicationStore<IList<RequestView>>(MainFactory.getCacheExpiration(), new List<RequestView>());
            
            IList<ELEMENT> elements = this.svc.getElements();
            IList<NOTE> notes = this.svc.getNotes();
            IList<ProjectView> projects = new List<ProjectView>();
            foreach (PROJECT project in this.svc.getProjects())
                projects.Add(convertProject(project));
            foreach (REQUEST r in this.svc.getRequests()) {
                RequestView rv = convertRequest(r);
                this.requestList.Data.Add(rv);

                // pointer to parent project
                rv.Parent = r.PARENT_PROJECT_ID == null ? null : projects.Where(x => x.ID == r.PARENT_PROJECT_ID).FirstOrDefault();
                if (rv.Parent != null) 
                    rv.Parent.RequestList.Add(rv);
                
                // pointer to child elements
                foreach (ELEMENT e in elements.Where(x => x.PARENT_REQUEST_ID == rv.ID)) {
                    ElementView ev = convertElement(e);
                    ev.Parent = rv;
                    rv.ElementList.Add(ev);

                    // pointer to notes
                    foreach (NOTE n in notes.Where(x => x.ELEMENT_ID == ev.ID)) {
                        Note nv = convertNote(n);
                        nv.Parent = ev;
                        ev.addNote(nv);
                    }
                }
            }
            HttpContext.Current.Application["Request"] = this.requestList;
            HttpContext.Current.Application.UnLock();
        }

        public string requestEmailUpdateRequesterBody(RequestView request) {
            string url = MainFactory.getInstance() + "/Console/Dashboard?type=Request&id=" + request.ID;

            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            string email = "<html>";
            email += "An item that you requested has been updated.<br />";
            email += "To see the updated request, please check the status by going to the following web site:";
            email += "<br /><a href='" + url + "'>" + url + "</a>";
            email += "<br><br>Or view all your pending details in the <a href='http://" + MainFactory.getInstance() + "/Console" + "'>Dashboard</a>";
            email += "</html>";

            return email;
        }

        private string getNearSearchHtml(string text, string search) {
            int charLimit = 20;
            string returnText = text;
            text = text.ToLower();
            search = search.ToLower();

            int start, end;
            start = text.IndexOf(search);
            end = start + search.Length;

            bool leftOk, rightOk;
            leftOk = rightOk = false;
            while (!(leftOk && rightOk)) {
                if (!leftOk) {
                    if (charLimit - (end - start) > 0 && start > 0)
                        start--;
                    else
                        leftOk = true;
                }
                if (!rightOk) {
                    if (charLimit - (end - start) > 0 && end < text.Length - 1)
                        end++;
                    else
                        rightOk = true;
                }
            }

            text = returnText = (start > 0 ? "..." : "") + returnText.Substring(start, end - start) + (end <= text.Length - 1 ? "..." : "");
            text = text.ToLower();
            start = text.IndexOf(search);
            end = start + search.Length;
            returnText = returnText.Replace(returnText.Substring(start, end - start), "<mark>" + returnText.Substring(start, end - start) + "</mark>");
            return returnText;
        }

        private Note convertNote(NOTE note) {
            // find parent element
            ElementView parentElement = null;
            foreach (RequestView request in this.requestList.Data) {
                foreach (ElementView element in request.ElementList)
                    if (element.ID == note.ELEMENT_ID)
                        parentElement = element;
            }

            if (parentElement == null)
                throw new NullReferenceException("Unable to find the parent element of the note: " + note.NOTE_TEXT);

            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            return new Note(note, userMgr.getUser(note.UPDATED_BY_ID), parentElement);
        }

        private ElementView convertElement(ELEMENT element) {
            // find parent Request
            RequestView parentRequest = getRequest(element.PARENT_REQUEST_ID);

            if (parentRequest == null)
                throw new NullReferenceException("Unable to find the parent request of the element: " + element.ELEMENT_SUMMARY);

            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            return new ElementView(element,
                                    null,
                                    userMgr.getUser(element.ASSIGNED_TO_ID),
                                    this.elementStatusList.Data.Where(x => x.ID == element.ELEMENT_STATUS_ID).FirstOrDefault(),
                                    parentRequest);
        }

        private RequestView convertRequest(REQUEST request) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            LookupMgr lookupMgr = new LookupMgr(this.svc);
            Configuration config = MainFactory.getConfiguration();
            RequestView r = new RequestView(request, 
                                    (User)userMgr.getUser(request.SUBMITTED_BY_ID),
                                    (User)userMgr.getUser(request.REQUESTED_BY_ID),
                                    (User)userMgr.getUser((int)request.ASSIGNED_TO_ID), 
                                    null,
                                    lookupMgr.getRequestCategories(false).Where(x => x.ID == request.REQUEST_CATEGORY_ID).FirstOrDefault(),
                                    lookupMgr.getSupportAreas().Where(x => x.ID == request.SUPPORT_AREA_ID).FirstOrDefault(),
                                    null,
                                    lookupMgr.getPrograms().Where(x => x.ID == request.PROGRAM_ID).Cast<Program>().FirstOrDefault(),
                                    lookupMgr.getValueDrivers().Where(x => x.ID == request.VALUE_DRIVER_ID).Cast<ValueDriver>().FirstOrDefault(),
                                    this.requestStatusList.Data.Where(x => x.ID == request.REQUEST_STATUS_ID).FirstOrDefault(),
                                    lookupMgr.getRequestTypes(EOpenType.Request, false).Where(x => x.ID == request.REQUEST_TYPE_ID).FirstOrDefault(),
                                    Decimal.Parse((string)config.get("HoursCostMultiplierInternal")),
                                    Decimal.Parse((string)config.get("HoursCostMultiplierExternal")));

            return r;
        }

        private ProjectView convertProject(PROJECT project) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            LookupMgr lookupMgr = new LookupMgr(this.svc);
            Configuration config = MainFactory.getConfiguration();
            return new ProjectView(project,
                        (User)userMgr.getUser(project.SUBMITTED_BY_ID),
                        (User)userMgr.getUser(project.REQUESTED_BY_ID),
                        null,
                        lookupMgr.getSupportAreas().Where(x => x.ID == project.SUPPORT_AREA_ID).FirstOrDefault(),
                        lookupMgr.getPrograms().Where(x => x.ID == project.PROGRAM_ID).Cast<Program>().FirstOrDefault(),
                        lookupMgr.getValueDrivers().Where(x => x.ID == project.VALUE_DRIVER_ID).Cast<ValueDriver>().FirstOrDefault(),
                        this.projectStatusList.Data.Where(x => x.ID == project.PROJECT_STATUS_ID).FirstOrDefault(),
                        lookupMgr.getRequestTypes(EOpenType.Project, false).Where(x => x.ID == project.PROJECT_TYPE_ID).FirstOrDefault(),
                        Decimal.Parse((string)config.get("HoursCostMultiplierInternal")),
                        Decimal.Parse((string)config.get("HoursCostMultiplierExternal")),
                        (User)userMgr.getUser(project.PROJECT_LEAD_ID ?? 0));
        }

        private AProjectView setStatusID(AProjectView item, int statusID) {
            switch (statusID) {
                case 6: //2 = "On Hold"
                    item.setHoldDate();
                    break;
                case 7: //3 = "Complete"
                    item.setClosed();
                    break;
                case 8: //4 = "Rejected"
                    item.setClosed();
                    break;
                case 9: //5 = "Cancelled"
                    item.setClosed();
                    break;
            }
            if (item.GetType() == typeof(RequestView))
                item.Status = this.requestStatusList.Data.Where(x => x.ID == statusID).FirstOrDefault();
            else
                item.Status = this.projectStatusList.Data.Where(x => x.ID == statusID).FirstOrDefault();
            return item;
        }
    }
}