using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Domain;
using COMET.Model.Business.Factory;
using COMET.Model.Business.Manager;
using COMET.Model.Business.Service;

namespace COMET.Controllers {
    public class ActivityController : Controller {
        ILogSvc logger = MainFactory.getLogSvc();

        public ActionResult Index() {

            return RedirectToAction("ActivityByGroup");
        }

        public ActionResult ActivityByGroup() {
            ViewBag.Message = "Activity By Group";
            logger.logAction(ViewBag.Message);
                        
            return View("../Main/Activity");
        }

        public ActionResult ActivityByUser() {
            ViewBag.Message = "Activity By User";
            logger.logAction(ViewBag.Message);

            throw new NotImplementedException();
        }

        public ActionResult Trends() {
            ViewBag.Message = "Activity Trends";
            logger.logAction(ViewBag.Message);

            throw new NotImplementedException();
        }

        public ActionResult Futures() {
            ViewBag.Message = "Activity Futures";
            logger.logAction(ViewBag.Message);

            throw new NotImplementedException();
        }

        [HttpGet]
        [OutputCache(Duration = 600)]
        public JsonResult GetUsageByWeeks() {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            IList<Activity> activityByWeekList = userMgr.usagebyWeek();
            return Json(activityByWeekList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OutputCache(Duration = 600)]
        public JsonResult GetUsersInGroup(string userGroup) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            IList<string> userList = userMgr.usersInGroup(userGroup);
            return Json(userList);
        }
    }
}