using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Business.Service;
using COMET.Model.Business.Factory;

namespace COMET.Controllers {
    public class NNI_CalcController : Controller {
        // GET: NNI_Calc
        public ActionResult Index() {
            ILogSvc logger = MainFactory.getLogSvc();
            ViewBag.Message = "NNI Entrance Calculator";
            logger.logAction(ViewBag.Message);

            return View("../Main/NNI_Calc");
        }
    }
}