using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Business.Manager;
using COMET.Model.Business.Factory;
using COMET.Model.Console.Domain;
using COMET.Model.Console.Domain.View;

namespace COMET.Controllers.Console {
    [RouteArea("Console")]
    public class ManagerController : Controller {
        // GET: Manager
        public ActionResult Index(string type = "", int? id = null) {
            return Dashboard(type, id);
        }

        public ActionResult Dashboard(string type = "", int? id = null) {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            IEnumerable<AProjectView> requestList = requestMgr.getRequests(EOpenType.Request, null).Where(x => x.Status.Text.Equals("Moved to Project") && ((RequestView)x).Parent == null).Cast<AProjectView>().OrderBy(x => x.RequestedDueDate);
            ViewData["pendingRequests"] = requestList.Cast<ARequestView>().Count();

            ConsoleController consoleController = new ConsoleController();

            ViewData["isAdmin"] = true;
            ViewData["type"] = null;
            ViewData["type"] = type;
            switch (type.ToLower()) {
                case "pendingpromotes" :
                    ViewData["partialData"] = requestList.Cast<RequestView>().ToList();
                    break;
                case "request" :
                    ViewData["partialData"] = requestMgr.getRequest((int)id);
                    break;
                case "element" :
                    ViewData["partialData"] = requestMgr.getElement((int)id);
                    break;
                case "project":
                    ViewData["partialData"] = requestMgr.getProject((int)id);
                    break;
                default :
                    ViewData["type"] = null;
                    ViewData["partialData"] = consoleController.GridHelper(null, null, null, null, null, null, null, null, null, null, null);
                    break;
            }
            ViewBag.Message = "Manager - Dashboard";
            return View("../Console/ManagerDashboard");
        }

        [HttpPost]
        public ActionResult CreateProject(RequestView requestView, DateTime startDate) {
            ProjectView project = null;
            try {
                RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
                project = requestMgr.saveProject(requestView, startDate);
                TempData["valid"] = true;
                TempData["error"] = "New project created.";
                return RedirectToAction("Project", "Console", new { id = project.ID });
            } catch (Exception e) {
                TempData["valid"] = false;
                TempData["error"] = "Unable to create Project: " + e.Message;
                return RedirectToAction("Request", "Console", new { id = requestView.ID });
            }
        }

        [HttpPost]
        public ActionResult PromoteToProject(int requestID, DateTime startDate)  {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            return CreateProject(requestMgr.getRequest(requestID), startDate);
        }

        [HttpPost]
        public ActionResult PendingRequest() {
            RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());
            IEnumerable<AProjectView> requestList = requestMgr.getRequests(EOpenType.Request, null).Where(x => x.Status.Text.Equals("Moved to Project") && ((RequestView)x).Parent == null).Cast<AProjectView>().OrderBy(x => x.RequestedDueDate);
            List<RequestView> a = requestList.Cast<RequestView>().ToList();
            return PartialView("~/Views/Console/Partial/_PendingPromotes.ascx", a);
        }
    }
}