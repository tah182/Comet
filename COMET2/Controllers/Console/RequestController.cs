using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using COMET.Model.Business.Manager;
using COMET.Model.Business.Factory;
using COMET.Model.Console.Domain;
using COMET.Model.Domain;
using COMET.Model.Console.Domain.View;

namespace COMET.Controllers {
    public class RequestController : ApiController { 
        // GET: Request
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET: Request/5
        public string Get(int id) {
            return "value";
        }

        // POST: Request/SetAssignedTo
        public void setAssignedTo(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);
            IUser assignedTo = getUser(value);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }
            // throw exception if bad employeeID
            if (assignedTo == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Employee with ID = {0}", id)),
                    ReasonPhrase = "Employee ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.AssignedTo = (User)assignedTo;
            requestMgr.updateRequest(request);
        }

        public void setSubmittedBy(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);
            IUser submittedBy = getUser(value);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }
            // throw exception if bad employeeID
            if (submittedBy == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Employee with ID = {0}", id)),
                    ReasonPhrase = "Employee ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.SubmittedBy = (User)submittedBy;
            requestMgr.updateRequest(request);
        }

        public void setRequestedBy(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);
            IUser requestedBy = getUser(value);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }
            // throw exception if bad employeeID
            if (requestedBy == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Employee with ID = {0}", id)),
                    ReasonPhrase = "Employee ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.RequestedBy = (User)requestedBy;
            requestMgr.updateRequest(request);
        }

        public void setRequestType(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);
            
            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            LookupActive requestType = lookupMgr.getRequestTypes(EOpenType.Request, true).Where(x => x.ID == value).FirstOrDefault();
            // throw exception if bad requestType ID
            if (requestType == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request Type with ID = {0}", id)),
                    ReasonPhrase = "Request Type Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.CType = requestType;
            requestMgr.updateRequest(request);
        }

        public void setRequestCategory(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            LookupActive requestCategory = lookupMgr.getRequestCategories(true).Where(x => x.ID == value).FirstOrDefault();
            // throw exception if bad requestType ID
            if (requestCategory == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request Category with ID = {0}", id)),
                    ReasonPhrase = "Request Type Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.RequestCategory = requestCategory;
            requestMgr.updateRequest(request);
        }

        public void setSupportArea(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            SupportArea supportArea = lookupMgr.getSupportAreas().Where(x => x.ID == value).FirstOrDefault();
            // throw exception if bad requestType ID
            if (supportArea == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Support Area with ID = {0}", id)),
                    ReasonPhrase = "Request Type Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.SupportArea = supportArea;
            requestMgr.updateRequest(request);
        }

        public void setParentProject(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            ProjectView projectView = requestMgr.getProject(value);
            // throw exception if bad requestType ID
            if (projectView == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Project with ID = {0}", id)),
                    ReasonPhrase = "Request Type Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.Parent = projectView;
            requestMgr.updateRequest(request);
        }

        public void setProgram(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            Program program = (Program) lookupMgr.getPrograms().Where(x => x.ID == value);
            // throw exception if bad requestType ID
            if (program == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Program with ID = {0}", id)),
                    ReasonPhrase = "Request Type Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.Program = program;
            requestMgr.updateRequest(request);
        }

        public void setValueDriver(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            ValueDriver valueDriver = (ValueDriver)lookupMgr.getValueDrivers().Where(x => x.ID == value);
            // throw exception if bad requestType ID
            if (valueDriver == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Value Driver with ID = {0}", id)),
                    ReasonPhrase = "Request Type Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.ValueDriver = valueDriver;
            requestMgr.updateRequest(request);
        }

        public void setStatus(int id, int value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            LookupSorted status = (LookupSorted)requestMgr.getStatusList(EOpenType.Request).Where(x => x.ID == value);
            // throw exception if bad requestType ID
            if (status == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request Status with ID = {0}", id)),
                    ReasonPhrase = "Request Type Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.Status = status;
            requestMgr.updateRequest(request);
        }

        public void setSubmitDate(int id, DateTime value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.OpenDate = value;
            requestMgr.updateRequest(request);
        }

        public void setDuedate(int id, DateTime value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.RequestedDueDate = value;
            requestMgr.updateRequest(request);
        }

        public void setEstimated(int id, DateTime value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.EstimatedDueDate = value;
            requestMgr.updateRequest(request);
        }

        public void setManagerQueue(int id, DateTime value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.ManagerQueueDate = value;
            requestMgr.updateRequest(request);
        }

        public void setManagerApproved(int id, DateTime value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.ManagerApprovedDate = value;
            requestMgr.updateRequest(request);
        }

        public void setHold(int id, DateTime value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.HoldDate = value;
            requestMgr.updateRequest(request);
        }

        public void setResume(int id, DateTime value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.ResumeDate = value;
            requestMgr.updateRequest(request);
        }

        public void setClose(int id, DateTime value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.isNew = true;
            request.ClosedDate = value;
            request.isNew = false;
            requestMgr.updateRequest(request);
        }

        public void setLastUpdated(int id, DateTime value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.LastUpdated = value;
            requestMgr.updateRequest(request);
        }

        public void setEstimatedHours(int id, decimal value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            if (value < 0) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Conflict) {
                    Content = new StringContent(string.Format("Estimated hours requested to be set to {0}", value)),
                    ReasonPhrase = "Cannot set to number less than 0"
                };
                throw new HttpResponseException(response);
            }

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.EstimatedHours = value;
            requestMgr.updateRequest(request);
        }

        public void setValue(int id, decimal value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            if (value < 0) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Conflict) {
                    Content = new StringContent(string.Format("Value requested to be set to {0}", value)),
                    ReasonPhrase = "Cannot set to number less than 0"
                };
                throw new HttpResponseException(response);
            }

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.Value = value;
            requestMgr.updateRequest(request);
        }

        public void setSummary(int id, string value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            if (value == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Conflict) {
                    Content = new StringContent(string.Format("Summary value is being set to null")),
                    ReasonPhrase = "Summary cannot be null"
                };
                throw new HttpResponseException(response);
            }

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.Summary = value;
            requestMgr.updateRequest(request);
        }

        public void setDescription(int id, string value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            if (value == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Conflict) {
                    Content = new StringContent(string.Format("Description value is being set to null")),
                    ReasonPhrase = "Description cannot be null"
                };
                throw new HttpResponseException(response);
            }

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.Description = value;
            requestMgr.updateRequest(request);
        }

        public void setResolution(int id, string value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            if (value == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Conflict) {
                    Content = new StringContent(string.Format("Resolution value is being set to null")),
                    ReasonPhrase = "Resolution cannot be null"
                };
                throw new HttpResponseException(response);
            }

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.Resolution = value;
            requestMgr.updateRequest(request);
        }

        public void setValueReason(int id, string value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            if (value == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Conflict) {
                    Content = new StringContent(string.Format("Value Reason is being set to null")),
                    ReasonPhrase = "Value Reason cannot be null"
                };
                throw new HttpResponseException(response);
            }

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.ValueReason = value;
            requestMgr.updateRequest(request);
        }

        public void setITFeature(int id, string value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);
            
            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.ITFeatures = value;
            requestMgr.updateRequest(request);
        }

        public void setTOA(int id, string value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.TopOffnetAttributeNumber = value;
            requestMgr.updateRequest(request);
        }

        public void setManagerNote(int id, string value) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            RequestView request = requestMgr.getRequest(id);

            // throw exception if bad request id
            if (request == null) {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    Content = new StringContent(string.Format(" No Request with ID = {0}", id)),
                    ReasonPhrase = "Request ID Not Found"
                };
                throw new HttpResponseException(response);
            }

            request.ManagerNote = value;
            requestMgr.updateRequest(request);
        }

        private IUser getUser(int id) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            IUser user = userMgr.getUser(id);

            return user;
        }
    }
}
