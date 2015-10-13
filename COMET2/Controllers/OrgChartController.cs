using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Domain;
using COMET.Model.Business.Factory;
using COMET.Model.Business.Service;
using COMET.Model.Business.Manager;

namespace COMET.Controllers {
    public class OrgChartController : Controller {

        [OutputCache(Duration = 300)]
        public ActionResult Index(int? id) {
            ILogSvc logger = MainFactory.getLogSvc();
            logger.logAction("Organization Chart");
            
            ViewBag.Message = "Employee Search";
            if (id != null)
                return Employee((int)id);

            ViewData["person"] = Session["user"];
            return View("../Main/OrgChart");
        }

        public ActionResult Employee(int id) {
            ViewBag.Message = "Employee Search";
            ViewData["person"] = getUser((int)id);
            if (ViewData["person"] == null)
                throw new HttpException(404, "Not Found");
            return View("../Main/OrgChart");
        }

        [HttpPost]
        public ActionResult GetUser(int id) {
            IUser user = getUser(id);
            return PartialView("~/Views/Partial/_EmployeeDetails.ascx", user);
        }

        [HttpGet]
        public JsonResult PeopleList(string input) {
            List<string[]> searchReturn = new List<string[]>();
            if (input != null) {
                UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());

                // get users that match and are active employees within the last 60 days
                List<IUser> userList = userMgr.getAllUsers().Where(x => x.TermDate == null || x.TermDate >= DateTime.Today.AddDays(-60)).ToList();
                foreach (IUser user in userList.Where(x => x.Name.ToLower().Contains(input.ToLower())).OrderBy(y => y.Name)) {
                    string[] returnstuff = { user.EmployeeID.ToString(), getNearSearchHtml(user.Name, input) };
                    searchReturn.Add(returnstuff);
                }
            }

            return Json(searchReturn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Hierarchy(int id) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            return Json(userMgr.retreiveHeirarchy(id).OrderBy(x => x.Name));
        }

        private IUser getUser(int id) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());

            return userMgr.getUser(id);
        }
        
        private string getNearSearchHtml(string text, string search) {
            string returnText = text;
            text = text.ToLower();
            search = search.ToLower();

            int start = text.IndexOf(search);
            int end = start + search.Length;

            text = returnText.Substring(start, end - start);
            returnText = returnText.Replace(text, "<mark>" + text + "</mark>");
            return returnText;
        }
    }
}