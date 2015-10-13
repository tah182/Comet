using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

using COMET.Model.Domain.Console;
using COMET.Model.Business.Factory;
using COMET.Server.Domain;

namespace COMET.Model.Business.Service.Console {
    public class EmployeeSvcImplDB : IEmployeeSvc {
        public IList<IEmployee> getAllEmployees() {
            IList<IEmployee> employeeList = new List<IEmployee>();
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    var query = (from e in db.GetTable<EMPLOYEE>()
                                 select new Employee(e.EMPLOYEE_ID,
                                                     e.START_DATE,
                                                     e.END_DATE,
                                                     e.GROUP_MANAGER_START));
                    foreach (Employee e in query)
                        employeeList.Add(e);
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name, 
                                                 MainFactory.getCurrentMethod(), 
                                                 "esid-gae-01", 
                                                 se.Message + "\n" + se.StackTrace);
            }

            return employeeList;
        }

        public IEmployee getEmployee(int employeeID) {
            Employee employee = new Employee(employeeID);
            return getUser(employee);
        }

        /// <summary>
        /// Checks the type of employee to get information on and queries from specific table.
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public IEmployee getEmployee(IEmployee employee) {
            if (employee.GetType() == typeof(Director))
                return getDirector(employee);
            if (employee.GetType() == typeof(GroupManager))
                return getManager(employee);

            return getUser(employee);
        }

        public IEmployee saveEmployee(IEmployee employee) {
            if (!employee.isValid())
                throw new ArgumentException("Please fill out all necessary information.");

            if (employee.GetType() == typeof(Director)) {
                Director d = (Director)employee;
                DIRECTOR director = new DIRECTOR {
                    DIRECTOR_ID = d.ID,
                    START_DATE = d.StartDate,
                    END_DATE = null
                };
                return saveDirector(director);
            }
            if (employee.GetType() == typeof(GroupManager)) {
                GroupManager m = (GroupManager)employee;
                GROUP_MANAGER manager = new GROUP_MANAGER {
                    MANAGER_ID = m.ID,
                    GROUP_ID = (Int16)m.GroupID,
                    DIRECTOR_ID = m.DirectorID,
                    START_DATE = m.StartDate,
                    END_DATE = null
                };
                return saveManager(manager);
            } else {
                Employee e = (Employee)employee;
                EMPLOYEE emp = new EMPLOYEE {
                    EMPLOYEE_ID = e.ID,
                    GROUP_MANAGER_START = e.GroupManagerID,
                    START_DATE = e.StartDate,
                    END_DATE = null
                };
                return saveUser(emp);
            }
        }

        public bool updateEmployee(IEmployee employee) {
            if (employee.GetType() == typeof(Director)) {
                if (!isValidDirectorId(employee.ID))
                    throw new ArgumentException("Director does not exist. Did you mean to add a new Director instead?");
            } 

            throw new System.NotImplementedException();
        }

        public bool isValidGroupId(int groupID) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    return (db.GROUP_lkps.Any(x => x.GROUP_ID == groupID));
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("EmployeeSvcImplDB", 
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-ivg-01", 
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
                MainFactory.getLogSvc().logError("EmployeeSvcImplDB",
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-ivd-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            return false;
        }

        /// <summary>
        /// Retrieves a user level employee from the Employee table
        /// </summary>
        /// <param name="employee">The minimal details to retrieve the employee from the database</param>
        /// <returns>A full user level with detail.</returns>
        private IEmployee getUser(IEmployee employee) {
            Employee returnEmp = null;
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    returnEmp = (from e in db.GetTable<EMPLOYEE>()
                                 where e.EMPLOYEE_ID == employee.ID
                                 select new Employee(e.EMPLOYEE_ID, 
                                                     e.START_DATE, 
                                                     e.END_DATE, 
                                                     e.GROUP_MANAGER_START))
                                .FirstOrDefault();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-ge-01",
                                                 se.Message + "\n" + se.StackTrace);
            }

            return returnEmp;
        }

        private IEmployee getManager(IEmployee employee) {
            GroupManager manager = null;
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    manager = (from m in db.GetTable<GROUP_MANAGER>()
                               where m.DIRECTOR_ID == employee.ID
                               select new GroupManager(m.MANAGER_ID,
                                                       m.DIRECTOR_ID,
                                                       m.START_DATE,
                                                       m.END_DATE,
                                                       m.GROUP_MANAGER_START))
                                .FirstOrDefault();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-gm-01",
                                                 se.Message + "\n" + se.StackTrace);
            }

            return manager;
        }

        private IEmployee getDirector(IEmployee employee) {
            Director director = null;
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", true)) {
                    director = (from d in db.GetTable<DIRECTOR>()
                                where d.DIRECTOR_ID == employee.ID
                                select new Director(d.DIRECTOR_ID,
                                                    d.START_DATE,
                                                    d.END_DATE)).FirstOrDefault();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-gd-01",
                                                 se.Message + "\n" + se.StackTrace);
            }

            return director;
        }

        private IEmployee saveUser(EMPLOYEE employee) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    db.EMPLOYEEs.InsertOnSubmit(employee);
                    db.SubmitChanges();

                    return new Employee(employee.EMPLOYEE_ID,
                                        employee.START_DATE,
                                        employee.END_DATE,
                                        employee.GROUP_MANAGER_START);
                }
            } catch (SqlException se) {
                if (se.Number == 2627)  // if exception is due to primary key violation.
                    throw new ArgumentException("The employee already has a new record for today. Please update that record.");

                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-su-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            throw new ArgumentException("Unable to insert employee. Please validate information and try again.");
        }

        private IEmployee saveManager(GROUP_MANAGER manager) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    db.GROUP_MANAGERs.InsertOnSubmit(manager);
                    db.SubmitChanges();

                    return new GroupManager(manager.MANAGER_ID, 
                                            manager.DIRECTOR_ID,
                                            manager.START_DATE,
                                            manager.END_DATE,
                                            manager.GROUP_ID);
                }
            } catch (SqlException se) {
                if (se.Number == 2627)  // if exception is due to primary key violation.
                    throw new ArgumentException("The manager already has a new record for today. Please update that record.");

                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-sm-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            throw new ArgumentException("Unable to insert manager. Please validate information and try again.");
        }

        private IEmployee saveDirector(DIRECTOR director) {
            try {
                using (ConsoleDataContext db = (ConsoleDataContext)MainFactory.getDb("Console", false)) {
                    db.DIRECTORs.InsertOnSubmit(director);
                    db.SubmitChanges();

                    return new Director(director.DIRECTOR_ID,
                                        director.START_DATE,
                                        director.END_DATE);
                }
            } catch (SqlException se) {
                if (se.Number == 2627)  // if exception is due to primary key violation.
                    throw new ArgumentException("The director already has a new record for today. Please update that record.");

                MainFactory.getLogSvc().logError(this.GetType().Name,
                                                 MainFactory.getCurrentMethod(),
                                                 "esid-sd-01",
                                                 se.Message + "\n" + se.StackTrace);
            }
            throw new ArgumentException("Unable to insert director. Please validate information and try again.");
        }
    }
}