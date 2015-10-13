using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Domain;
using COMET.Model.Console.Domain.View;
using COMET.Model.Console.Domain;
using COMET.Model.Business.Factory;
using COMET.Model.Business.Service;
using COMET.Model.Business.Manager;
using System.Net;

namespace COMET.Controllers.Console {
    /// <summary>
    /// Provides services for all console activities
    /// </summary>
    public class ConsoleController : Controller {
        private readonly string CONFIG_EMAIL = "ConsoleFrom";

        /// <summary>
        /// Access to the Index page for console
        /// </summary>
        /// <returns>A Redirect to the Dashboard method</returns>
        public ActionResult Index() {
            MainFactory.getLogSvc().logAction("Console - Dashboard");
            return RedirectToAction("Dashboard", "Console", new { type="" });
            //return Dashboard("", null);
        }

        /// <summary>
        /// Submit a new Request
        /// </summary>
        /// <param name="request">The new request to be submitted</param>
        /// <returns>Redirects to HttpGet page</returns>
        [HttpPost]
        public ActionResult SubmitRequest(NewRequestModel request) {
            if (request == null)
                return NewRequest();

            MainFactory.getLogSvc().logAction("Console - Submit Request");

            int enteredRequest = 0;
            try {
                RequestView newRequest = submitRequest(request);
                enteredRequest = newRequest.ID;

                IUser assignedTo = newRequest.AssignedTo;
                RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());

                string from = (string)MainFactory.getConfiguration().get(CONFIG_EMAIL);

                EmailSvc.Email(from,
                                assignedTo.EmailAddress,
                                "amo_Datateam@level3.com",
                                "New Request #" + newRequest.ID + " - " + newRequest.Summary,
                                ConsoleFactory.requestEmailSupportBody(newRequest));
                if (!assignedTo.Equals(newRequest.RequestedBy))
                    EmailSvc.Email(from,
                                newRequest.RequestedBy.EmailAddress,
                                "",
                                "New Request #" + newRequest.ID + " - " + newRequest.Summary,
                                ConsoleFactory.requestEmailSubmitterBody(newRequest));
            } catch (Exception e) {
                return RedirectToAction("NewWithResponse", new { success = false, message = e.Message });
            }
            string response = "Request has been entered. Reference number is " + enteredRequest + ".";
            return RedirectToAction("NewWithResponse", new { success = true, message = response });
        }

        /// <summary>
        /// Get page response after submitting a new request
        /// </summary>
        /// <param name="request">Copy of the request that was submitted</param>
        /// <param name="success">True if submit was successful</param>
        /// <param name="message">Response from system of the submission</param>
        /// <returns>The new request view with a response message</returns>
        [ActionName("NewWithResponse")]
        public ActionResult NewRequest(NewRequestModel request, bool? success, string message) {
            if (request == null)
                return RedirectToAction("NewRequest");
            ViewBag.Message = "New Request";
            
            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            Employee thisUser = (Employee)employeeMgr.getUser((IUser)Session["User"]);

            ViewData["user"] = Session["User"];
            ViewData["isAdmin"] = employeeMgr.isAdmin(thisUser);
            ViewData["groupName"] = new NewRequestController().employeeGroupName(thisUser.ID);
            ViewData["groupManager"] = new NewRequestController().employeeManager(thisUser.ID);
            ViewData["displayMessage"] = message;
            ViewData["success"] = success;

            return View("NewRequest", request);
        }

        /// <summary>
        /// View for a new request
        /// </summary>
        /// <returns>The View for a new request</returns>
        [OutputCache(Duration = 120)]
        public ActionResult NewRequest() {
            MainFactory.getLogSvc().logAction("Console - New Request");
            ViewBag.Message = "New Request";

            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            Employee thisUser = (Employee)employeeMgr.getUser((IUser)Session["User"]);
           
            ViewData["user"] = Session["User"];
            ViewData["isAdmin"] = employeeMgr.isAdmin(thisUser);
            ViewData["groupManager"] = new NewRequestController().employeeManager(thisUser.ID);
            ViewData["groupName"] = new NewRequestController().employeeGroupName(thisUser.ID);
            
            return View("NewRequest");
        }

        [OutputCache(Duration = 15)]
        public ActionResult Dashboard(string type="", int? id=null) {
            if (type.Length > 0)
                ViewBag.Message = type.Left(1).ToUpper() + type.Substring(1).ToLower() + " #" + id;
            else
                ViewBag.Message = "Dashboard";

            IUser employee = (IUser)Session["User"];
            if (employee.canImpersonate()) {
                UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
                return AdminDashboard(type, id, employee);
            } else
                return UserDashboard(type, id, ((IUser)Session["User"]));
        }

        public ActionResult Project(int? type = null) {
            int? id = type;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            ProjectView project = requestMgr.getProject((int)id);

            ViewData["statusList"] = requestMgr.getStatusList(EOpenType.Project);
            ViewData["programList"] = lookupMgr.getPrograms();
            ViewData["typeList"] = lookupMgr.getRequestTypes(EOpenType.Project, false);
            ViewData["supportArea"] = lookupMgr.getSupportAreas();
            ViewData["valueDriver"] = lookupMgr.getValueDrivers();

            ViewData["type"] = EOpenType.Project;
            ViewData["user"] = (IUser)Session["User"];
            ViewData["isValidated"] = TempData["valid"] ?? true;
            ViewData["error"] = TempData["error"] ?? "";
            ViewBag.Message = "Project #" + project.ID;
            if (project == null)
                return Dashboard(null, null);
            return View("FullView", project);
        }

        [HttpPost]
        public ActionResult updateProject(ProjectView projectView) {
            ProjectView project = null;
            if (projectView != null) {
                try {
                    RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
                    project = requestMgr.updateProject(projectView);
                    TempData["error"] = "Update Successful";
                    TempData["valid"] = true;
                    return RedirectToAction("Project", "Console", new { id = project.ID });
                } catch (Exception e) {
                    project = projectView;
                    project.isNew = false;
                    TempData["error"] = e.Message;
                    TempData["valid"] = false;
                    return RedirectToAction("Project", "Console", new { id = projectView.ID });
                }
            }
            return Dashboard(null, null);
        }
        
        public new ActionResult Request(int? type = null) {
            int? id = type;
            if (id == null)
                throw new HttpException(404, "Not Found");

            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            RequestView request = requestMgr.getRequest((int)id);
            if (request == null)
                throw new HttpException(404, "Not Found");

            ViewData["statusList"] = requestMgr.getStatusList(EOpenType.Request);
            ViewData["programList"] = lookupMgr.getPrograms();
            ViewData["typeList"] = lookupMgr.getRequestTypes(EOpenType.Request, false);
            ViewData["supportArea"] = lookupMgr.getSupportAreas();
            ViewData["valueDriver"] = lookupMgr.getValueDrivers();
            IList<IEmployee> admin = employeeMgr.getAdminList();
            ViewData["assignedTo"] = employeeMgr.getAdminList();
            ViewData["requestCategory"] = lookupMgr.getRequestCategories(false);
            ViewData["project"] = requestMgr.getProjects();

            ViewBag.Message = "Request #" + request.ID;
            ViewData["type"] = EOpenType.Request;
            ViewData["error"] = TempData["error"] ?? "";
            ViewData["isValidated"] = TempData["valid"] ?? true;
            ViewData["user"] = (IUser)Session["User"];
            return View("FullView", request);
        }

        [HttpPost]
        public ActionResult updateRequest(RequestView requestView) {
            RequestView request = null;
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            if (requestView != null) {
                try {
                    RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
                    requestView.RequestedBy = (User)userMgr.getUser(requestView.RequestedBy.EmployeeID);
                    requestView.AssignedTo = (User)userMgr.getUser(requestView.AssignedTo.EmployeeID);
                    request = requestMgr.updateRequest(requestView);
                    TempData["error"] = "Update Successful";
                    TempData["valid"] = true;

                    string from = (string)MainFactory.getConfiguration().get(CONFIG_EMAIL);
                    
                    return RedirectToAction("Request", "Console", new { id = request.ID });
                } catch (Exception e) {
                    request = requestView;
                    request.isNew = false;
                    TempData["error"] = e.Message;
                    TempData["valid"] = false;
                    return RedirectToAction("Request", "Console", new { id = request.ID });
                }
            }
            return Dashboard(null, null);
        }

        public ActionResult Element(int? type = null) {
            int? id = type;
            if (id == null)
                throw new HttpException(404, "Not Found");
            
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            ElementView element = requestMgr.getElement((int)id);
            if (element == null)
                throw new HttpException(404, "Not Found");
            
            ViewData["statusList"] = requestMgr.getStatusList(EOpenType.Element);
            ViewData["assignedTo"] = employeeMgr.getAdminList();
            
            ViewData["error"] = TempData["error"] ?? "";
            ViewBag.Message = "Element #" + element.ID; 
            ViewData["type"] = EOpenType.Element;
            ViewData["isValidated"] = TempData["valid"] ?? true;
            ViewData["user"] = (IUser)Session["User"];
            return View("FullView", TempData["model"] == null ? element : (ElementView)TempData["model"]);
        }

        public ActionResult createElement(int type) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());

            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            ViewData["statusList"] = requestMgr.getStatusList(EOpenType.Element);
            ViewData["assignedTo"] = employeeMgr.getAdminList();

            ViewData["error"] = TempData["error"] ?? "";
            ViewBag.Message = "New Element";
            ViewData["type"] = EOpenType.Element;
            ViewData["isValidated"] = TempData["valid"] ?? true;
            ViewData["user"] = (IUser)Session["User"];

            ElementView element = requestMgr.createElement(type);
            element.AssignedTo = (User)ViewData["user"];
            return View("FullView", element);
        }

        [HttpPost]
        public ActionResult createElement(ElementView elementView) {
            ElementView element = null;
            if (elementView != null) {
                try {
                    RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
                    element = requestMgr.saveElement(elementView);
                    TempData["error"] = "New Element created";
                    TempData["valid"] = true;
                    if (element.AssignedTo != (IUser)Session["User"])
                        EmailSvc.Email((string)MainFactory.getConfiguration().get(CONFIG_EMAIL),
                                    element.AssignedTo.EmailAddress,
                                    "",
                                    "New Element #" + element.ID + " - " + element.Summary,
                                    ConsoleFactory.elementEmailSupportBody(element, (IUser)Session["User"]));
                    return RedirectToAction("Element", "Console", new { id = element.ID });
                } catch (Exception e) {
                    TempData["model"] = elementView;
                    TempData["error"] = e.Message;
                    TempData["valid"] = false;
                    return RedirectToAction("createElement", "Console", new { Id = elementView.Parent.ID });
                }
            }
            return Dashboard(null, null);
        }

        [HttpPost]
        public ActionResult updateElement(ElementView elementView) {
            ElementView element = null;
            if (elementView != null) {
                try {
                    RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
                    element = requestMgr.updateElement(elementView, false);
                    TempData["error"] = "Update Successful";
                    TempData["valid"] = true;
                    return RedirectToAction("Element", "Console", new { id = element.ID });
                } catch (Exception e) {
                    elementView.isNew = false;
                    TempData["error"] = e.Message;
                    TempData["valid"] = false;
                    TempData["model"] = elementView;
                    return RedirectToAction("Element", "Console", new { id = elementView.ID });
                }
            }
            return Dashboard(null, null);
        }

        [HttpPost]
        public ActionResult addNote(string text, int elementId) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());

            Note note = new Note();
            note.UpdatedBy = (IUser)Session["User"];
            note.Text = text;
            requestMgr.saveNote(note, elementId);
            IList<Note> notes = requestMgr.getElement(elementId).Note;

            return PartialView("~/Views/Console/Partial/_NoteList.ascx", notes);
        }
        
        [HttpPost]
        [OutputCache(Duration = 15)]
        public ActionResult PartialElement(int elementID) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            ViewData["isAdmin"] = ((IUser)Session["User"]).canImpersonate();
            return PartialView("~/Views/Console/Partial/_ElementView.ascx", requestMgr.getElement(elementID));
        }

        [HttpPost]
        [OutputCache(Duration = 15)]
        public ActionResult PartialRequest(int requestID) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            ViewData["isAdmin"] = ((IUser)Session["User"]).canImpersonate();
            RequestView v = requestMgr.getRequest(requestID);
            return PartialView("~/Views/Console/Partial/_RequestView.ascx", requestMgr.getRequest(requestID));
        }

        [HttpPost]
        [OutputCache(Duration = 15)]
        public ActionResult PartialProject(int projectID) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            ViewData["isAdmin"] = ((IUser)Session["User"]).canImpersonate();
            return PartialView("~/Views/Console/Partial/_ProjectView.ascx", requestMgr.getProject(projectID));
        }

        [HttpGet]
        [OutputCache(Duration = 120)]
        public ContentResult SearchConsole(string searchContext) {
            if (searchContext.Length > 0) {
                RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
                IList<Link> linkList = requestMgr.getItemsLike(searchContext.ToLower());
                return Jsonify<IList<Link>>.Serialize(linkList);
            }
            return null;
        }

        [HttpPost]
        public JsonResult GridAsJson(int? ID,
                                 int[] requestorIdlist,
                                 int[] assignedIdList,
                                 int[] category,
                                 int[] type,
                                 int[] area,
                                 string summary,
                                 string submittedRange,
                                 string dueDateRange,
                                 int[] status,
                                 string closedRange) {
            List<ReturnGrid> returnGrid = new List<ReturnGrid>();

            foreach (RequestView requestView in GridHelper(ID,
                        requestorIdlist,
                        assignedIdList,
                        category,
                        type,
                        area,
                        summary,
                        submittedRange,
                        dueDateRange,
                        status,
                        closedRange).Data)
                returnGrid.Add(new ReturnGrid(requestView.ID,
                    requestView.RequestedBy.EnglishName,
                    requestView.AssignedTo.EnglishName,
                    requestView.RequestCategory.Text,
                    requestView.CType.Text,
                    requestView.SupportArea.Text,
                    requestView.Summary,
                    requestView.OpenDate,
                    requestView.RequestedDueDate,
                    requestView.Status.Text,
                    requestView.ClosedDate,
                    requestView.Hours,
                    requestView.Parent));

            return Json(returnGrid);
        }

        [HttpPost]
        public ActionResult Grid(int? ID, 
                                 int[] requestorIdlist, 
                                 int[] assignedIdList, 
                                 int[] category,
                                 int[] requestType, 
                                 int[] area, 
                                 string summary, 
                                 string submittedRange,
                                 string dueDateRange,
                                 int[] status,
                                 string closedRange) {
            return PartialView("~/Views/Console/Partial/_DashboardGrid.ascx", 
                GridHelper(ID, 
                           requestorIdlist, 
                           assignedIdList, 
                           category,
                           requestType,
                           area,
                           summary,
                           submittedRange,
                           dueDateRange,
                           status,
                           closedRange));
        }

        internal DashboardGrid GridHelper(int? ID, 
                                         int[] requestorIdList, 
                                         int[] assignedIdList, 
                                         int[] category,
                                         int[] requestType, 
                                         int[] area,
                                         string summary,
                                         string submittedRange,
                                         string dueDateRange,
                                         int[] status,
                                         string closedRange) {

            if (submittedRange != null && submittedRange.Length != 21) 
                submittedRange = null;
            if (dueDateRange != null && dueDateRange.Length != 21) 
                dueDateRange = null;
            if (closedRange != null && closedRange.Length != 21)
                closedRange = null;

            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            IList<RequestView> requestList = 
                requestMgr
                    .getRequests(EOpenType.Request, null)
                    .Cast<RequestView>()
                    .Where(x => (ID == null || ID == x.ID))
                    .Where(x => (requestorIdList == null || (requestorIdList.Count() == 1 && requestorIdList[0] == 0) || requestorIdList.Contains(x.RequestedBy.EmployeeID)))
                    .Where(x => (assignedIdList == null || (assignedIdList.Count() == 1 && assignedIdList[0] == 0) || assignedIdList.Contains(x.AssignedTo.EmployeeID)))
                    .Where(x => (category == null || (category.Count() == 1 && category[0] == 0) || category.Contains(x.RequestCategory.ID)))
                    .Where(x => (requestType == null || (requestType.Count() == 1 && requestType[0] == 0) || requestType.Contains(x.CType.ID)))
                    .Where(x => (area == null || (area.Count() == 1 && area[0] == 0) || area.Contains(x.SupportArea.ID)))
                    .Where(x => (submittedRange == null || (x.OpenDate >= DateTime.Parse(submittedRange.Split('-')[0]) && x.OpenDate <= DateTime.Parse(submittedRange.Split('-')[1]))))
                    .Where(x => (dueDateRange == null || (x.RequestedDueDate >= DateTime.Parse(dueDateRange.Split('-')[0]) && x.RequestedDueDate <= DateTime.Parse(dueDateRange.Split('-')[1]))))
                    .Where(x => (status == null || (status.Count() == 1 && status[0] == 0) || status.Contains(x.Status.ID)))
                    .Where(x => (closedRange == null || (x.ClosedDate >= DateTime.Parse(closedRange.Split('-')[0]) && x.ClosedDate <= DateTime.Parse(closedRange.Split('-')[1]))))
                    .ToList();

           // filter by summary using wildcard
            if (summary != null && summary.Length > 0)
                if (summary.Left(1).Equals("%") && summary.Right(1).Equals("%"))
                    requestList = requestList.Where(x => x.Summary.ToLower().Contains(summary.Substring(1, summary.Length - 2).ToLower())).ToList();
                else if (summary.Left(1).Equals("%"))
                    requestList = requestList.Where(x => x.Summary.Right(summary.Length - 1).ToLower().Equals(summary.Right(summary.Length - 1).ToLower())).ToList();
                else if (summary.Right(1).Equals("%"))
                    requestList = requestList.Where(x => x.Summary.Left(summary.Length - 1).ToLower().Equals(summary.Left(summary.Length - 1).ToLower())).ToList();
                else
                    requestList = requestList.Where(x => x.Summary.ToLower().Equals(summary.ToLower())).ToList();
            
            DashboardGrid gridView = 
                new DashboardGrid(requestList.Select(x => x.RequestedBy).OrderBy(y => y.EnglishName).Distinct().ToList(),
                                  requestList.Select(x => x.AssignedTo).OrderBy(y => y.EnglishName).Distinct().ToList(),
                                  requestList.Select(x => x.RequestCategory).OrderBy(y => y.Text).Distinct().ToList(),
                                  requestList.Select(x => x.CType).OrderBy(y => y.Text).Distinct().ToList(),
                                  requestList.Select(x => x.SupportArea).OrderBy(y => y.Text).Distinct().ToList(),
                                  summary,
                                  submittedRange,
                                  dueDateRange,
                                  requestList.Select(x => x.Status).OrderBy(y => y.Text).Distinct().ToList(),
                                  closedRange,
                                  requestList);
            return gridView;
        }

        private ActionResult UserDashboard(string type, int? id, IUser user) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            IEnumerable<AProjectView> requestList = requestMgr.getRequests(EOpenType.Request, ((IUser)Session["User"])).Cast<AProjectView>().OrderBy(x => x.RequestedDueDate);

            ViewData["openRequests"] = ConsoleFactory.createLink(EOpenType.Request, requestList.Cast<ARequestView>().ToList(), true);
            ViewData["type"] = type == null || type.Length < 1 ? null : type;
            if (type != null && type.Length > 0) {
                if (type.ToString().ToLower().Equals("request")) {
                    ViewData["partialData"] = requestMgr.getRequest((int)id);
                    if (ViewData["partialData"] == null)
                        throw new HttpException(404, "Not Found");
                } else if (type.ToString().ToLower().Equals("element")) {
                    ViewData["partialData"] = requestMgr.getElement((int)id);
                    if (ViewData["partialData"] == null)
                        throw new HttpException(404, "Not Found");
                } else if (type.ToString().ToLower().Equals("project")) {
                    ViewData["partialData"] = requestMgr.getProject((int)id);
                    if (ViewData["partialData"] == null)
                        throw new HttpException(404, "Not Found");
                } else
                    throw new HttpException(404, "Not Found");
            } 

            ViewData["id"] = id;
            ViewData["isAdmin"] = false;
            ViewData["isAdminManager"] = false;

            return View("Dashboard");
        }

        private ActionResult AdminDashboard(string type, int? id, IUser user) {
            List<RequestView> requests = (List<RequestView>)HttpContext.ApplicationInstance.Application["newRequests"];
            if (requests != null) {
                requests = requests.Where(x => !x.AssignedTo.Equals(user)).ToList();
                HttpContext.ApplicationInstance.Application["newRequests"] = requests;
            }
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            IEnumerable<AProjectView> requestList = requestMgr.getRequests(EOpenType.Request, user).Where(x => !x.Status.Text.Equals("Moved to Project")).Cast<AProjectView>().OrderBy(x => x.RequestedDueDate);
            
            ViewData["openRequests"] = ConsoleFactory.createLink(EOpenType.Request, requestList.Cast<ARequestView>().ToList(), true);
            ViewData["openElements"] = ConsoleFactory.createLink(EOpenType.Element, requestMgr.getRequests(EOpenType.Element, ((IUser)Session["User"])), true);
            ViewData["openProjects"] = ConsoleFactory.createLink(EOpenType.Project, requestMgr.getRequests(EOpenType.Project, user).ToList(), true);
            ViewData["type"] = type == null || type.Length < 1 ? null : type;
            if (type != null && type.Length > 0) {
                if (type.ToString().ToLower().Equals("request")) {
                    ViewData["partialData"] = requestMgr.getRequest((int)id);
                    if (ViewData["partialData"] == null)
                        throw new HttpException(404, "Not Found");
                } else if (type.ToString().ToLower().Equals("element")) {
                    ViewData["partialData"] = requestMgr.getElement((int)id);
                    if (ViewData["partialData"] == null)
                        throw new HttpException(404, "Not Found");
                } else if (type.ToString().ToLower().Equals("project")) {
                    ViewData["partialData"] = requestMgr.getProject((int)id);
                    if (ViewData["partialData"] == null)
                        throw new HttpException(404, "Not Found");
                } else
                    throw new HttpException(404, "Not Found");
            } else
                ViewData["partialData"] = GridHelper(null, null, null, null, null, null, null, null, null, null, null);

            ViewData["id"] = id;
            ViewData["isAdmin"] = true;
            ViewData["isAdminManager"] = ((IUser)Session["User"]).isBIManager();

            return View("Dashboard");
        }

        private RequestView submitRequest(NewRequestModel request) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            RequestView submittal = new RequestView();
            submittal.RequestedBy = (User)userMgr.getUser(request.RequestBy);
            submittal.SubmittedBy = (User)userMgr.getUser(request.SubmittedBy);
            submittal.SupportArea = lookupMgr.getSupportAreas().Where(x => x.ID == request.SupportAreaID).FirstOrDefault();
            submittal.CType = lookupMgr.getRequestTypes(EOpenType.Request, true).Where(x => x.ID == request.TypeID).FirstOrDefault();
            submittal.RequestCategory = lookupMgr.getRequestCategories(true).Where(x => x.ID == request.RequestCategory).FirstOrDefault();
            submittal.RequestedDueDate = request.RequestedDueDate;
            submittal.Summary = request.RequestSummary;
            submittal.Description = request.RequestDescription;
            submittal.ValueDriver = lookupMgr.getValueDrivers().Where(x => x.ID == request.ValueDriverID).Cast<ValueDriver>().FirstOrDefault();
            submittal.Value = request.Value;
            submittal.ValueReason = request.ValueReason;

            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView r = (RequestView)requestMgr.createRequest(submittal);
            addToNewRequests(r);
            return r;
        }

        private void addToNewRequests(RequestView request) {
            List<RequestView> requests = (List<RequestView>)HttpContext.ApplicationInstance.Application["newRequests"];
            if (requests == null)
                requests = new List<RequestView>();

            if (!requests.Any(x => x.ID == request.ID))
                requests.Add(request);

            HttpContext.ApplicationInstance.Application["newRequests"] = requests;
        }

        private class ReturnGrid {
            public ReturnGrid(int id,
                            string requestor,
                            string assigned,
                            string category,
                            string type,
                            string area,
                            string summary,
                            DateTime? submitted,
                            DateTime? dueDate,
                            string status,
                            DateTime? closed,
                            decimal hours,
                            ProjectView parent) {
                this.ID = id;
                this.Requestor = requestor;
                this.Assigned = assigned;
                this.Category = category;
                this.Type = type;
                this.Area = area;
                this.Summary = summary;
                this.Submitted = submitted;
                this.DueDate = DueDate;
                this.Status = status;
                this.Closed = closed;
                this.Hours = hours;
                this.Parent = parent == null ? null : (int?)parent.ID;
            }
            public int ID { get; private set; }
            public string Requestor { get; private set; }
            public string Assigned { get; private set; }
            public string Category { get; private set; }
            public string Type { get; private set; }
            public string Area { get; private set; }
            public string Summary { get; private set; }
            public DateTime? Submitted { get; private set; }
            public DateTime? DueDate { get; private set; }
            public string Status { get; private set; }
            public DateTime? Closed { get; private set; }
            public decimal Hours { get; private set; }
            public int? Parent { get; private set; }
        }
    }
}
