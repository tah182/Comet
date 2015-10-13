using COMET.Model.Business.Factory;
using COMET.Model.Domain;
using COMET.Server.Domain;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Service to log activities and errors in to the database
    /// </summary>
    public class LogSvcImplDB : ILogSvc {

        /// <summary>
        /// Logs the action taken by the user by each controller
        /// </summary>
        /// <param name="action">The action to save in the table</param>
        public void logAction(string action) {
            IUser user = (IUser)HttpContext.Current.Session["User"];

            if (user == null)
                user = (IUser)COMET.Controllers.ContextController.getContext().Session["User"];

            ApplicationLog al = new ApplicationLog {
                Application_Name = (string)MainFactory.getConfiguration().get("application"),
                EmployeeID = user == null ? 0 : user.EmployeeID,
                LogTime = DateTime.Now,
                ActionTaken = action
            };

            try {
                using (UserDataContext dc = (UserDataContext) MainFactory.getDb("User", false)) {
                    dc.ApplicationLogs.InsertOnSubmit(al);
                    dc.SubmitChanges();
                }
            } catch (SqlException sqle) {
                logError("Logger", MethodBase.GetCurrentMethod().ToString(), sqle.ErrorCode.ToString(), "Unable to log Action: " + sqle.Message);
            }
        }

        /// <summary>
        /// Logs the error into the database
        /// </summary>
        /// <param name="pageName">The name of the page that the error occurred on</param>
        /// <param name="stepName">The step or method that the error occurred within</param>
        /// <param name="errorCode">The unique error code for the error for debugging</param>
        /// <param name="details">The Exception with stacktrace if available or other details to include</param>
        public ApplicationErrors2 logError(string pageName, string stepName, string errorCode, string details) {
            IUser user = (IUser)HttpContext.Current.Session["User"];

            if (user == null)
                user = (IUser)COMET.Controllers.ContextController.getContext().Session["User"];
            if (user == null) {
                string userNT = Environment.UserName.ToLower();
                if (userNT.Length > 0 && !userNT.ToLower().Equals("iusr")) {
                    COMET.Model.Business.Manager.UserMgr userMgr = new COMET.Model.Business.Manager.UserMgr(MainFactory.getUserSvc());
                    user = userMgr.getUser(userNT);
                }
            }

            ApplicationErrors2 ae = new ApplicationErrors2 {
                ApplicationName = (string)MainFactory.getConfiguration().get("application"),
                PageName = pageName,
                StepName = stepName,
                EmployeeID = user.EmployeeID,
                ErrorTime = DateTime.Now,
                ErrorCode = errorCode,
                ErrorDetails = details
            };

            try {
                using (UserDataContext dc = (UserDataContext)MainFactory.getDb("User", false)) {
                    dc.ApplicationErrors2s.InsertOnSubmit(ae);
                    dc.SubmitChanges();
                }
            } catch (SqlException se) {
                EmailSvc.EmailError(ae, se.Message + "<br>" + se.StackTrace, ae.ErrorDetails);
            }
            return ae;
        }
    }
}