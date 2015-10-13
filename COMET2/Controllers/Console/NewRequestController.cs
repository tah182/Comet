using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Business.Factory;
using COMET.Model.Business.Manager;
using COMET.Model.Console.Business.Service;
using COMET.Model.Business.Service;
using COMET.Model.Console.Domain;
using COMET.Model.Domain;

namespace COMET.Controllers.Console {
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class NewRequestController : Controller {

        class EmpGroupMan {
            public EmpGroupMan(string manager, string groupName) {
                this.Manager = manager;
                this.GroupName = groupName;
            }

            public string Manager { get; private set; }
            public string GroupName { get; private set; }
        }

        public ActionResult Employees() {
            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            return Jsonify<IList<Employee>>.Serialize(employeeMgr.getUserList(true));
        }

        [HttpGet]
        public ContentResult RequestAreas() {
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            return Jsonify<IList<ILookup>>.Serialize(lookupMgr.getSupportAreas().Cast<ILookup>().ToList());
        }
        
        [HttpGet]
        public ContentResult RequestTypes(bool? activeOnly) {
            bool active = activeOnly == null ? false : (bool)activeOnly;
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            return Jsonify<IList<LookupActive>>.Serialize(lookupMgr.getRequestTypes(EOpenType.Request, active));
        }

        [HttpGet]
        public ContentResult RequestCategories(bool? activeOnly) {
            bool active = activeOnly == null ? false : (bool)activeOnly;
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            return Jsonify<IList<LookupActive>>.Serialize(lookupMgr.getRequestCategories(active));
        }

        [HttpGet]
        public ContentResult ValueDrivers() {
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            return Jsonify<IList<ALookup>>.Serialize(lookupMgr.getValueDrivers());
        }

        [HttpGet]
        public ContentResult Employees(string input, bool? activeOnly) {
            bool active = activeOnly == null ? false : (bool)activeOnly;
            LookupMgr lookupMgr = new LookupMgr(ConsoleFactory.getRequestSvc());
            return Jsonify<IList<IUser>>.Serialize(lookupMgr.getEmployees(input, active));
        }

        [HttpGet]
        public ContentResult EmployeeGroupManager(int employeeID) {
            EmpGroupMan empGroupMan = new EmpGroupMan(employeeManager(employeeID), employeeGroupName(employeeID));
            return Jsonify<EmpGroupMan>.Serialize(empGroupMan);
        }

        public string employeeGroupName(int employeeID) {
            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            Employee thisUser = (Employee)employeeMgr.getUser(employeeID);
            IList<GroupManager> groupList = employeeMgr.getGroupList(true);

            return employeeMgr.getUserGroupName(thisUser);
        }

        public string employeeManager(int employeeID) {
            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            UserMgr usrMgr = new UserMgr(MainFactory.getUserSvc());

            Employee thisUser = (Employee)employeeMgr.getUser(employeeID);
            IList<GroupManager> groupList = employeeMgr.getGroupList(true);



            return usrMgr.getUser(groupList.Where(x => x.GroupManagerID == thisUser.GroupManagerID).FirstOrDefault().ID).EnglishName;
        }

        //[HttpPost]
        //public ActionResult CreateNewRequest(int requestedBy, 
        //                                     int submittedBy, 
        //                                     int requestArea, 
        //                                     int requestType, 
        //                                     int requestCategory,
        //                                     string requestSummary,
        //                                     string requestDescription,
        //                                     int valueDriver,
        //                                     int value,
        //                                     string valueDescription,
        //                                     DateTime desiredDueDate) {
        //    RequestMgr requestMgr = new RequestMgr(ConsoleFactory.getRequestSvc());

        //    int defaultID = Int32.Parse((string)MainFactory.getConfiguration().get("BIManagerID"));
        //    int supportID = requestMgr.getSupportAreas().Where(x => x.ID == requestArea).FirstOrDefault().SupportID ?? defaultID;

        //    try {
        //        Request request = new Request(requestedBy, 
        //                                      submittedBy, 
        //                                      requestArea, 
        //                                      requestType, 
        //                                      requestCategory, 
        //                                      requestSummary, 
        //                                      requestDescription,
        //                                      supportID,
        //                                      valueDriver, 
        //                                      value, 
        //                                      valueDescription, 
        //                                      desiredDueDate);
                
        //        request = (Request)requestMgr.createRequest((IRequest)request);
        //    } catch (Exception e) {
        //        throw e;
        //    }
        //    UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
        //    IUser user = userMgr.getUser(supportID);

        //    string from = (string)MainFactory.getConfiguration().get(CONFIG_EMAIL);
        //    EmailSvc.Email(from, user.EmailAddress, "takashi.tatsumoto@level3.com", "New Request", "<html><font style='font-size:200px'>HUGE NEW REQUEST WAS MADE</font></html>");
        //}

        //
    }
}
