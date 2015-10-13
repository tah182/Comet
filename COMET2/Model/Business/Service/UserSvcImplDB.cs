using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

using COMET.Model.Domain;
using COMET.Server.Domain;
using COMET.Model.Business.Factory;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Database Implementation of the User Retrieval
    /// </summary>
    public class UserSvcImplDB : IUserSvc {

        public IList<IUser> getAllUsers() {
            List<IUser> users = new List<IUser>();

            try {
                int biManager = Int32.Parse((string)MainFactory.getConfiguration().get("BIManagerID"));
                using (UserDataContext dc = (UserDataContext)MainFactory.getDb("User", true)) {
                     users = (from xxhr in dc.GetTable<XXHR>()
                          join xxhr1 in dc.GetTable<XXHR>()
                          on xxhr.MANAGER equals xxhr1.NAME into xr
                          from x in xr.DefaultIfEmpty()
                          join auth in dc.GetTable<Application_Role>()
                          on xxhr.EMPLOYEE_ID equals auth.EmployeeID into xa
                          from newx in xa.DefaultIfEmpty()
                          select new User (xxhr.EMPLOYEE_ID,
                                              xxhr.NAME,
                                              xxhr.DISPLAY_NAME,
                                              xxhr.SYS_USER,
                                              xxhr.EMAIL_ADDRESS,
                                              xxhr.JOB_NAME,
                                              x.EMPLOYEE_ID,
                                              xxhr.DEPARTMENT,
                                              xxhr.TELEPHONE,
                                              xxhr.MOBILE,
                                              (DateTime)xxhr.HIRE_DATE,
                                              xxhr.TERM_DATE,
                                              new OfficeBuilding(xxhr.OFFICE_NAME,
                                                                  xxhr.STREET_ADDRESS,
                                                                  xxhr.CITY,
                                                                  xxhr.POSTALCODE,
                                                                  xxhr.STATE_CD),
                                              newx.AuthLevel == null ? 3 : newx.AuthLevel,
                                              newx.AckVersion == null ? false : newx.AckVersion, 
                                              biManager == xxhr.EMPLOYEE_ID)).Cast<IUser>().ToList();
                }
            } catch (SqlException se) {
                ILogSvc logger = MainFactory.getLogSvc();
                logger.logError("UserSvcImplDB", MethodBase.GetCurrentMethod().ToString(), se.ErrorCode.ToString(), "Unable to query database to get user information. <br>" + se.StackTrace + "\n" + se.Message);
                throw;
            }

            return users;
        }

        public bool setUserAcknowledge(IUser user) {
            try {
                using (UserDataContext dc = (UserDataContext)MainFactory.getDb("User", false)) {
                    Application_Role ar = dc.Application_Roles.Where(x => x.EmployeeID == user.EmployeeID).FirstOrDefault();
                    ar.AckVersion = true;
                    dc.SubmitChanges();
                    return true;
                }
            } catch (SqlException se) {
                ILogSvc logger = MainFactory.getLogSvc();
                logger.logError("UserSvcImplDB", MethodBase.GetCurrentMethod().ToString(), se.ErrorCode.ToString(), "Unable to update " + user.EmployeeID + " acknowledged message. <br>" + se.StackTrace + "\n" + se.Message);
            }
            return false;
        }

        public IList<IUser> retreiveHeirarchy(IUser user) {
            IList<IUser> result = new List<IUser>();
            try {
                using (UserDataContext dc = (UserDataContext)MainFactory.getDb("User", true)) {
                    result = (from users in dc.XXHR_HEIRARCHY(user.NTLogin)
                            where users.TERM_DATE == null ||
                            users.TERM_DATE >= DateTime.Today.AddDays(-60)
                            select (IUser)new User (users.EMPLOYEE_ID,
                                              users.NAME,
                                              users.NAME,
                                              users.SYS_USER,
                                              users.EMAIL_ADDRESS,
                                              users.JOB_NAME,
                                              users.MANAGER1_ID,
                                              users.DEPARTMENT,
                                              users.TELEPHONE,
                                              users.MOBILE,
                                              (DateTime)users.HIRE_DATE,
                                              users.TERM_DATE,
                                              new OfficeBuilding(users.OFFICE_NAME,
                                                                  users.STREET_ADDRESS,
                                                                  users.CITY,
                                                                  users.POSTALCODE,
                                                                  users.STATE_CD),
                                              3,
                                              false,
                                              false))
                            .ToList();
                }
            } catch (SqlException se) {
                ILogSvc logger = MainFactory.getLogSvc();
                logger.logError("UserSvcImplDB", MethodBase.GetCurrentMethod().ToString(), se.ErrorCode.ToString(), "XXHR_HEIRARCHY Lookup Failure. <br>" + se.StackTrace + "\n" + se.Message);
            }
            return result;
        }

        public IUser addUser(IUser user) {
            throw new System.NotImplementedException();
        }

        public IList<Activity> usagebyWeek() {
            IList<Activity> activityByWeekList = new List<Activity>();
            try {
                using(UserDataContext dc = (UserDataContext)MainFactory.getDb("User", true)) {
                    activityByWeekList = (from activity in dc.GetTable<APP_USAGE_W_GROUP>()
                                          select activity)
                                         .ToList()
                                         .Select(x => 
                                             new Activity(
                                              ((DateTime)x.WeekStart).ToString("MM/dd"),
                                              x.GROUPING,
                                              (int)x.Logs
                                              ))
                                         .ToList();
                }
            } catch (SqlException se) {
                ILogSvc logger = MainFactory.getLogSvc();
                logger.logError("UserSvcImplDB", MethodBase.GetCurrentMethod().ToString(), se.ErrorCode.ToString(), "APP_USAGE_W_GROUP Lookup Error. <br>" + se.StackTrace + "\n" + se.Message);
            }
            return activityByWeekList;
        }

        public IList<string> usersInGroup(string userGroup) {
            IList<string> usersInGroupList = new List<string>();
            try {
                using (UserDataContext dc = (UserDataContext)MainFactory.getDb("User", true)) {
                    usersInGroupList = (from activity in dc.GetTable<APP_LOG_W_GROUP>()
                                      join user in dc.GetTable<XXHR>()
                                      on activity.EmployeeID equals user.EMPLOYEE_ID
                                      where activity.GROUPING.Equals(userGroup)
                                      select user.DISPLAY_NAME)
                                      .Distinct()
                                      .ToList();
                }
            } catch (SqlException se) {
                ILogSvc logger = MainFactory.getLogSvc();
                logger.logError("UserSvcImplDB", MethodBase.GetCurrentMethod().ToString(), se.ErrorCode.ToString(), "APP_LOG_W_GROUP Lookup Error. <br>" + se.StackTrace + "\n" + se.Message);
            }
            return usersInGroupList;
        }
    }
}