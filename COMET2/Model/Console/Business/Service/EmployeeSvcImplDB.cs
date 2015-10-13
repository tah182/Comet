using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

using COMET.Model.Domain;
using COMET.Model.Business.Manager;
using COMET.Model.Console.Domain;
using COMET.Model.Business.Factory;
using COMET.Server.Domain;

namespace COMET.Model.Console.Business.Service {
    public class EmployeeSvcImplDB : IEmployeeSvc {
        
        public IList<Employee> getAllEmployees() {
            IList<Employee> employeeList = new List<Employee>();
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    var query  = from e in db.GetTable<EMPLOYEE>()
                                select new {
                                    id = e.EMPLOYEE_ID,
                                    startDate = e.START_DATE,
                                    endDate = e.END_DATE,
                                    groupManagerStart = e.GROUP_MANAGER_START
                                };
                    foreach(var e in query) {
                        employeeList.Add(new Employee(userMgr.getUser(e.id),
                                                    e.startDate,
                                                    e.endDate,
                                                    e.groupManagerStart));
                    }
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name, 
                                                 MainFactory.getCurrentMethod(), 
                                                 "esid-gae-01", 
                                                 se.Message + "\n" + se.StackTrace);
            }

            return employeeList;
        }

        public IList<GroupManager> getAllGroupManagers() {
            IList<GroupManager> groupManagerList = new List<GroupManager>();
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    var query = from e in db.GetTable<GROUP_MANAGER>()
                                join g in db.GetTable<GROUP_lkp>()
                                on e.GROUP_ID equals g.GROUP_ID
                                 select new {
                                     name = g.GROUP_TEXT,
                                     managerID = e.MANAGER_ID,
                                     directorID = e.DIRECTOR_ID,
                                     startDate = e.START_DATE,
                                     endDate = e.END_DATE,
                                     groupID = e.GROUP_ID,
                                     groupManagerStart = e.GROUP_MANAGER_START
                                 };

                    foreach (var e in query) {
                        groupManagerList.Add(new GroupManager(userMgr.getUser(e.managerID),
                                                        e.directorID,
                                                        e.startDate,
                                                        e.endDate,
                                                        e.groupID,
                                                        e.groupManagerStart, 
                                                        e.name));
                    }
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-gag-01",
                                                 se.Message + "\n" + se.StackTrace);
            }

            return groupManagerList;
        }
        
        public IList<Director> getAllDirectors() {
            IList<Director> directorList = new List<Director>();
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    var query = from d in db.GetTable<DIRECTOR>()
                                select new {
                                    id = d.DIRECTOR_ID,
                                    startDate = d.START_DATE,
                                    endDate = d.END_DATE
                                };
                    foreach (var e in query) {
                        directorList.Add(new Director(userMgr.getUser(e.id),
                                                     e.startDate,
                                                     e.endDate));
                    }
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-gad-01",
                                                 se.Message + "\n" + se.StackTrace);
            }

            return directorList;
        }

        public Employee saveEmployee(Employee employee) {
            MainFactory.getLogSvc().logAction("Save new Employee - " + employee.ID + " into console.");
            EMPLOYEE e = new EMPLOYEE {
                EMPLOYEE_ID = employee.ID,
                START_DATE = employee.StartDate,
                GROUP_MANAGER_START = employee.GroupManagerID
            };

            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Consxole", false)) {
                    db.EMPLOYEEs.InsertOnSubmit(e);
                    db.SubmitChanges();
                    return employee;
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-se-01",
                                                 se.Message + "\n" + se.StackTrace);
                throw new Exception("Unable to create Employee. Please see error log for further details.");
            }
        }

        public bool updateEmployee(IEmployee employee) {
            if (employee.GetType() == typeof(Director)) {
                if (!isValidDirectorId(employee.ID))
                    throw new ArgumentException("Director does not exist. Did you mean to add a new Director instead?");
            } 

            throw new System.NotImplementedException();
        }

        public ILookup saveGroup(Group group) {
            MainFactory.getLogSvc().logAction("Insert new group - " + group.Text + "into console.");
            GROUP_lkp lkp = new GROUP_lkp {
                GROUP_TEXT = group.Text
            };
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    db.GROUP_lkps.InsertOnSubmit(lkp);
                    db.SubmitChanges();
                    group = new Group(lkp.GROUP_ID, lkp.GROUP_TEXT);
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-sg-01",
                                                 se.Message + "\n" + se.StackTrace);
                throw new Exception("Unable to create Group . Please see error log for further details.");
            }
            return (ILookup)group;
        }

        public GroupManager saveGroupManager(int groupID, int managerID, int directorID) {
            MainFactory.getLogSvc().logAction("Save new GroupManager - " + managerID + " into console.");
            GROUP_MANAGER gm = new GROUP_MANAGER {
                GROUP_ID = (short)groupID,
                MANAGER_ID = managerID,
                DIRECTOR_ID = directorID,
                START_DATE = DateTime.Today
            };
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            GroupManager groupManager;

            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    db.GROUP_MANAGERs.InsertOnSubmit(gm);
                    db.SubmitChanges();
                    groupManager = new GroupManager(userMgr.getUser(managerID),
                                                    gm.DIRECTOR_ID,
                                                    gm.START_DATE,
                                                    gm.END_DATE,
                                                    gm.GROUP_ID,
                                                    gm.GROUP_MANAGER_START,
                                                    getGroupName(gm.GROUP_ID));
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                MainFactory.getCurrentMethod(),
                                                "esid-sgm-01",
                                                se.Message + "\n" + se.StackTrace);
                throw new Exception("Unable to create Group Manager. Please see error log for further details.");
            }
            return groupManager;
        }

        public Director saveDirector(int directorID) {
            MainFactory.getLogSvc().logAction("Save new Director - " + directorID + " into console.");
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            Director director = new Director(userMgr.getUser(directorID));
            DIRECTOR d = new DIRECTOR {
                DIRECTOR_ID = director.ID,
                START_DATE = director.StartDate
            };

            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    db.DIRECTORs.InsertOnSubmit(d);
                    db.SubmitChanges();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                MainFactory.getCurrentMethod(),
                                                "esid-sgm-01",
                                                se.Message + "\n" + se.StackTrace);
                throw new Exception("Unable to create Director. Please see error log for further details.");
            }
            return director;
        }

        public string getGroupName(int groupID) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    return db.GROUP_lkps.Where(x => x.GROUP_ID == groupID).FirstOrDefault().GROUP_TEXT;
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "rs-ggn-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            return null;
        }
        
        public bool isValidGroupManagerId(int groupManagerID) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    return (db.GROUP_MANAGERs.Any(x => x.GROUP_MANAGER_START == groupManagerID));
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name, 
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-ivgm-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            return false;
        }

        public bool isValidDirectorId(int directorID) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    return (db.DIRECTORs.Any(x => x.DIRECTOR_ID == directorID));
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-ivd-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            return false;
        }
       
        
    }
}