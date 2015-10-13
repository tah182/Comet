using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Server.Domain;
using COMET.Model.Business.Manager;
using COMET.Model.Console.Domain.View;
using COMET.Model.Business.Factory;
using COMET.Model.Console.Domain;
using COMET.Model.Domain;

namespace COMET.Model.Console.Business.Service {
    public class RequestSvcImplDB : IRequestSvc {
        //private ConsoleDataContext db;

        //public RequestSvcImplDB() {
        //    db = (ConsoleDataContext)MainFactory.getDb("Console", true);
        //}

        public IList<ALookup> getValueDrivers() {
            List<ValueDriver> valueDrivers = new List<ValueDriver>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    valueDrivers = (from sa in db.GetTable<VALUE_DRIVER>()
                                    select new ValueDriver(sa.VALUE_DRIVER_ID,
                                                           sa.VALUE_DRIVER_TEXT,
                                                           sa.COMMENT)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-gvd-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            return valueDrivers.Cast<ALookup>().ToList();
        }

        public IList<ALookup> getSupportUnits() {
            List<SupportUnit> supportUnits = new List<SupportUnit>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    supportUnits = (from su in db.GetTable<SUPPORT_UNIT>()
                                    select new SupportUnit(su.SUPPORT_UNIT_ID, su.SUPPORT_UNIT_TEXT)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                 MainFactory.getCurrentMethod(),
                                 "rs-gsu-01",
                                 se.Message + "\n" + se.StackTrace);
            }
            return supportUnits.Cast<ALookup>().ToList();
        }
        
        public IList<ALookup> getPrograms() {
            List<Program> programList = new List<Program>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    programList = (from p in db.GetTable<PROGRAM>()
                                    select new Program(p)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                 MainFactory.getCurrentMethod(),
                                 "rs-gsu-01",
                                 se.Message + "\n" + se.StackTrace);
            }
            return programList.Cast<ALookup>().ToList();
        }

        public IList<SupportArea> getSupportAreas() {
            List<SupportArea> supportAreas = new List<SupportArea>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    supportAreas = (from sa in db.GetTable<SUPPORT_AREA>()
                                    select new SupportArea(sa.SUPPORT_AREA_ID,
                                                           sa.SUPPORT_AREA_TEXT,
                                                           sa.SUPPORT_UNIT_ID,
                                                           sa.BUSINESS_TYPE_ID,
                                                           sa.SUPPORT_ID,
                                                           sa.DEVELOPER_ID)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-gsa-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            return supportAreas.ToList();
        }
        
        public IList<LookupSorted> getElementStatus() {
            List<LookupSorted> elementStatusList = new List<LookupSorted>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    elementStatusList = (from es in db.GetTable<ELEMENT_STATUS>()
                                         select new LookupSorted(es)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                 MainFactory.getCurrentMethod(),
                                 "rs-ges-01",
                                 se.Message + "\n" + se.StackTrace);
            }
            return elementStatusList;
        }

        public IList<LookupSorted> getRequestStatus() {
            List<LookupSorted> requestStatusList = new List<LookupSorted>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    requestStatusList = (from es in db.GetTable<REQUEST_STATUS>()
                                         select new LookupSorted(es)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                 MainFactory.getCurrentMethod(),
                                 "rs-ges-01",
                                 se.Message + "\n" + se.StackTrace);
            }
            return requestStatusList;
        }

        public IList<LookupSorted> getProjectStatus() {
            List<LookupSorted> projectStatusList = new List<LookupSorted>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    projectStatusList = (from es in db.GetTable<PROJECT_STATUS>()
                                         select new LookupSorted(es)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                 MainFactory.getCurrentMethod(),
                                 "rs-ges-01",
                                 se.Message + "\n" + se.StackTrace);
            }
            return projectStatusList;
        }

        public IList<LookupActive> getRequestTypes() {
            List<LookupActive> requestTypes = new List<LookupActive>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    requestTypes = (from rt in db.GetTable<REQUEST_TYPE>()
                                    select new LookupActive(rt.REQUEST_TYPE_ID,
                                                            rt.REQUEST_TYPE_TEXT,
                                                            rt.ACTIVE,
                                                            rt.COMMENT)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-grt-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            return requestTypes.Cast<LookupActive>().ToList();
        }

        public IList<LookupActive> getProjectTypes() {
            List<LookupActive> requestTypes = new List<LookupActive>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    requestTypes = (from rt in db.GetTable<PROJECT_TYPE>()
                                    select new LookupActive(rt.PROJECT_TYPE_ID,
                                                            rt.PROJECT_TYPE_TEXT,
                                                            rt.ACTIVE,
                                                            rt.COMMENT)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-grt-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            return requestTypes.Cast<LookupActive>().ToList();
        }

        public IList<LookupActive> getRequestCategories() {
            List<LookupActive> requestCategories = new List<LookupActive>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    requestCategories = (from rc in db.GetTable<REQUEST_CATEGORY>()
                                         select new LookupActive(rc.REQUEST_CATEGORY_ID,
                                                                 rc.REQUEST_CATEGORY_TEXT,
                                                                 rc.ACTIVE,
                                                                 rc.COMMENT)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-grc-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            return requestCategories;
        }
        
        public NOTE saveNote(Note note) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    NOTE newNote = new NOTE {
                        ELEMENT_ID = note.Parent.ID,
                        NOTE_DATE = note.Date,
                        NOTE_TEXT = note.Text,
                        UPDATED_BY_ID = note.UpdatedBy.EmployeeID
                    };
                    db.NOTEs.InsertOnSubmit(newNote);
                    db.SubmitChanges();
                    return newNote;
                }
            } catch (Exception se) {
                throw new Exception("Unable to add a new note due to " + se.Message);
            }
        }
        
        public ELEMENT saveElement(ElementView element) {
            if (!element.isValid())
                throw new Exception("There is an issue with your element.");
            ELEMENT details = convertElement(element);

            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    db.ELEMENTs.InsertOnSubmit(details);
                    db.SubmitChanges();

                    return details;
                }
            } catch (Exception se) {
                throw new Exception("Unable to save new element due to " + se.Message);
            }
        }

        public ELEMENT updateElement(ElementView element) {
            if (!element.isValid())
                throw new Exception("Element is not valid. Please send error.");

            ELEMENT updateElement = convertElement(element);
            updateElement.ELEMENT_ID = element.ID;

            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    var details = db.ELEMENTs.Where(x => x.ELEMENT_ID == element.ID).FirstOrDefault();
                   
                    details.ASSIGNED_TO_ID = element.AssignedTo.EmployeeID;
                    details.ELEMENT_STATUS_ID = (byte)element.Status.ID;
                    details.PERCENT_COMPLETE = (int)element.PercentComplete;
                    details.HOURS = element.Hours;
                    details.RESOLUTION = element.Resolution;
                    details.CLOSED_DATE = element.ClosedDate;

                    db.SubmitChanges();

                    return details;
                }
            } catch (Exception se) {
                throw new Exception(se.Message);
            }
        }

        public REQUEST saveRequest(RequestView request) {
            try {
                REQUEST submittal = convertRequest(request);

                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    db.REQUESTs.InsertOnSubmit(submittal);
                    
                    db.SubmitChanges();
                    return submittal;
                }
            } catch (Exception e) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-sr-01",
                                                 e.Message + "\n" + e.StackTrace);
                throw new Exception("Something terrible went wrong with your request.");
            }
            throw new Exception("Something terrible went wrong with your request. Please notify us if you continue to see these errors.");
        }

        public REQUEST updateRequest(RequestView request) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    REQUEST details = db.REQUESTs.Where(x => x.REQUEST_ID == request.ID).FirstOrDefault();

                    details.ASSIGNED_TO_ID = request.AssignedTo.EmployeeID;
                    details.REQUEST_STATUS_ID = (byte)request.Status.ID;
                    details.PROGRAM_ID = request.Program == null ? null : (request.Program.ID > 0 ? (short)request.Program.ID : details.PROGRAM_ID);
                    details.REQUEST_TYPE_ID = (short)request.CType.ID;
                    details.SUPPORT_AREA_ID = (short?)request.SupportArea.ID;
                    details.VALUE_DRIVER_ID = (short)request.ValueDriver.ID;
                    details.ESTIMATED_DUE_DATE = request.EstimatedDueDate;
                    details.ESTIMATED_COST = request.EstimatedCost;
                    details.ESTIMATED_HOURS = request.EstimatedHours;
                    details.ACTUAL_COST = request.ActualCost;
                    details.REQUEST_CATEGORY_ID = (short)request.RequestCategory.ID;
                    details.PARENT_PROJECT_ID = (request.Parent == null || request.Parent.ID == 0) ? null : (int?)request.Parent.ID;
                    details.RESOLUTION = request.Resolution;
                    details.IT_FEATURE = request.ITFeatures;
                    details.TOP_OFFNET_ATTRIBUTE_NUMBER = request.TopOffnetAttributeNumber;
                    details.CLOSED_DATE = request.ClosedDate;

                    db.SubmitChanges();

                    return details;
                }
            } catch (Exception se) {
                throw new Exception("Unable to update request #" + request.ID + " Due to " + se.Message);
            }
        }

        public PROJECT saveProject(ProjectView project) {
            try {
                PROJECT submittal = convertProject(project);

                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    db.PROJECTs.InsertOnSubmit(submittal);

                    db.SubmitChanges();
                    return submittal;
                }
            } catch (Exception e) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-sr-01",
                                                 e.Message + "\n" + e.StackTrace);
                throw new Exception("Something terrible has happened.");
            }
            throw new Exception("Something terrible has happened. Please notify us if you continue to see these errors.");
        }

        public PROJECT updateProject(ProjectView project) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    PROJECT details = db.PROJECTs.Where(x => x.PROJECT_ID == project.ID).FirstOrDefault();

                    details.PROJECT_STATUS_ID = (byte)project.Status.ID;
                    details.PROGRAM_ID = project.Program == null ? null : (project.Program.ID > 0 ? (short)project.Program.ID : details.PROGRAM_ID);
                    details.PROJECT_TYPE_ID = (short)project.CType.ID;
                    details.SUPPORT_AREA_ID = (short?)project.SupportArea.ID;
                    details.VALUE_DRIVER_ID = (short)project.ValueDriver.ID;
                    details.ESTIMATED_DUE_DATE = project.EstimatedDueDate;
                    details.ESTIMATED_COST = project.EstimatedCost;
                    details.ESTIMATED_HOURS = project.EstimatedHours;
                    details.ACTUAL_COST = project.ActualCost;
                    details.CLOSED_DATE = project.ClosedDate;

                    db.SubmitChanges();

                    return details;
                }
            } catch (Exception se) {
                throw new Exception("Unable to update project #" + project.ID + " Due to " + se.Message);
            }
        }
       
        public IList<PROJECT> getProjects() {
            IList<PROJECT> projectList = new List<PROJECT>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    projectList = db.PROJECTs.ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                    MainFactory.getCurrentMethod(),
                                                    "rs-gp-02",
                                                    se.Message + "\n" + se.StackTrace);
            }
            return projectList;
        }
        
        public IList<REQUEST> getRequests() {
            IList<REQUEST> requestList = new List<REQUEST>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    requestList = db.REQUESTs.ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                    MainFactory.getCurrentMethod(),
                                                    "rs-gr-02",
                                                    se.Message + "\n" + se.StackTrace);
            }
            return requestList;
        }
        
        public IList<ELEMENT> getElements() {
            List<ELEMENT> elements = new List<ELEMENT>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    elements = db.ELEMENTs.ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                    MainFactory.getCurrentMethod(),
                                                    "rs-ge-01",
                                                    se.Message + "\n" + se.StackTrace);
            } 
            return elements;
        }

        public IList<NOTE> getNotes() {
            List<NOTE> notes = new List<NOTE>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    notes = db.NOTEs.ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                    MainFactory.getCurrentMethod(),
                                                    "rs-gn-01",
                                                    se.Message + "\n" + se.StackTrace);
            }
            return notes;
        }

        public int getProgramFromSupportArea(int supportAreaID) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    return (from p in db.SUPPORT_AREA_TO_PROGRAMs
                            where p.SUPPORT_AREA_ID == (short)supportAreaID
                            select p.PROGRAM_ID).FirstOrDefault();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                    MainFactory.getCurrentMethod(),
                                                    "rs-gpsa-01",
                                                    se.Message + "\n" + se.StackTrace);
            }
            return -1;
        }

        private NOTE convertNote(Note note) {
            return new NOTE {
                ELEMENT_ID = note.Parent.ID,
                NOTE_DATE = note.Date,
                NOTE_TEXT = note.Text,
                UPDATED_BY_ID = note.UpdatedBy.EmployeeID
            };
        }

        private ELEMENT convertElement(ElementView element) {
            return new ELEMENT {
                ASSIGNED_TO_ID = element.AssignedTo.EmployeeID,
                PARENT_REQUEST_ID = element.Parent.ID,
                ELEMENT_STATUS_ID = (byte)element.Status.ID,
                ASSIGNED_DATE = element.OpenDate,
                CLOSED_DATE = element.ClosedDate,
                LAST_UPDATED_DATE = element.LastUpdated,
                PERCENT_COMPLETE = (int)element.PercentComplete,
                HOURS = element.Hours,
                ELEMENT_SUMMARY = element.Summary,
                RESOLUTION = element.Resolution                
            };
        }

        private REQUEST convertRequest(RequestView request) {
            return new REQUEST {
                ASSIGNED_TO_ID = request.AssignedTo.EmployeeID,
                SUBMITTED_BY_ID = request.SubmittedBy.EmployeeID,
                REQUESTED_BY_ID = request.RequestedBy.EmployeeID,
                REQUEST_TYPE_ID = (short)request.CType.ID,
                REQUEST_CATEGORY_ID = (short)request.RequestCategory.ID,
                SUPPORT_AREA_ID = (short?)request.SupportArea.ID,
                PARENT_PROJECT_ID = request.Parent == null ? null : (int?)request.Parent.ID,
                PROGRAM_ID = request.Program == null ? null : (short?)request.Program.ID,
                VALUE_DRIVER_ID = (short)request.ValueDriver.ID,
                VALUE_REASON = request.ValueReason,
                REQUEST_STATUS_ID = (byte)request.Status.ID,
                SUBMITTED_DATE = request.OpenDate,
                REQUESTED_DUE_DATE = (DateTime)request.RequestedDueDate,
                ESTIMATED_DUE_DATE = request.EstimatedDueDate,
                MANAGER_QUEUE_DATE = request.ManagerQueueDate,
                MANAGER_APPROVED_DATE = request.ManagerApprovedDate,
                HOLD_DATE = request.HoldDate,
                RESUME_DATE = request.ResumeDate,
                CLOSED_DATE = request.ClosedDate,
                LAST_UPDATED_DATE = request.LastUpdated,
                ESTIMATED_HOURS = request.EstimatedHours,
                VALUE = request.Value,
                ESTIMATED_COST = request.EstimatedCost,
                ACTUAL_COST = request.ActualCost,
                REQUEST_SUMMARY = request.Summary,
                REQUEST_DESCRIPTION = request.Description
            };
        }

        private PROJECT convertProject(ProjectView project) {
            return new PROJECT {
                SUBMITTED_BY_ID = project.SubmittedBy.EmployeeID,
                REQUESTED_BY_ID = project.RequestedBy.EmployeeID,
                PROJECT_TYPE_ID = (short)project.CType.ID,
                SUPPORT_AREA_ID = (short)project.SupportArea.ID,
                PROGRAM_ID = project.Program == null ? null : (short?)project.Program.ID,
                VALUE_DRIVER_ID = (short)project.ValueDriver.ID,
                PROJECT_STATUS_ID = (byte)project.Status.ID,
                SUBMITTED_DATE = project.OpenDate,
                REQUESTED_DUE_DATE = (DateTime)project.RequestedDueDate,
                ESTIMATED_DUE_DATE = project.EstimatedDueDate,
                MANAGER_QUEUE_DATE = project.ManagerQueueDate,
                MANAGER_APPROVED_DATE = project.ManagerApprovedDate,
                HOLD_DATE = project.HoldDate,
                RESUME_DATE = project.ResumeDate,
                CLOSED_DATE = project.ClosedDate,
                LAST_UPDATED_DATE = project.LastUpdated,
                ESTIMATED_HOURS = project.EstimatedHours,
                VALUE = project.Value,
                ESTIMATED_COST = project.EstimatedCost,
                ACTUAL_COST = project.ActualCost,
                PROJECT_SUMMARY = project.Summary,
                PROJECT_DESCRIPTION = project.Description,
                VALUE_REASON = project.ValueReason,
                MANAGER_NOTE = project.ManagerNote,
                PROJECT_LEAD_ID = project.AssignedTo.EmployeeID,
                START_DATE = project.StartDate
            };
        }
    }
}