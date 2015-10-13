using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Business.Service;
using COMET.Model.Business.Factory;

namespace COMET.Controllers {
    public class ErrorController : Controller {
        // GET: Error
        public ActionResult Index() {
            Exception exc = Server.GetLastError();

            COMET.Server.Domain.ApplicationErrors2 ae = MainFactory.getLogSvc().logError("Application", "Global", exc.Message, exc.StackTrace);
            Session["ApplicationError"] = exc.Message + "<br>" + exc.StackTrace;
            EmailSvc.EmailError(ae, exc.Message, exc.StackTrace);
            Server.ClearError();
            return View("Error");
        }

        public ActionResult Error404() {
            return View("Error404");
        }
    }
}