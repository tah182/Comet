using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Business.Factory;
using COMET.Model.Business.Service;

namespace COMET.Controllers {
    public class SSRSReportsController : Controller {
        // GET: SSRSReports
        public ActionResult Index(int type) {
            ILogSvc logger = MainFactory.getLogSvc();
            logger.logAction("Access SSRS Report " + type);
            ViewBag.Message = "AMO DataTeam Reports";

            return Redirect("ReportViewer.aspx?ReportType=" + type);
        }
    }
}