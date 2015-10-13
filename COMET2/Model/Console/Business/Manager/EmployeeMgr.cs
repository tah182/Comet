using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Model.Console.Domain;
using COMET.Model.Business.Factory;
using COMET.Model.Console.Business.Service;

namespace COMET.Model.Business.Manager {
    public class EmployeeMgr : IManager {
        IEmployeeSvc repository;
        ApplicationStore<IList<Employee>> employeeList;
        ApplicationStore<IList<GroupManager>> groupManagerList;
        ApplicationStore<IList<Director>> directorList;

        public EmployeeMgr(IEmployeeSvc repository) {
            this.repository = repository;

            this.employeeList = (ApplicationStore<IList<Employee>>)HttpContext.Current.Application["Employee"];
            this.groupManagerList = (ApplicationStore<IList<GroupManager>>)HttpContext.Current.Application["GroupManager"];
            this.directorList = (ApplicationStore<IList<Director>>)HttpContext.Current.Application["Director"];

            if (employeeList == null || groupManagerList == null || directorList == null || 
                !employeeList.isValid() || !groupManagerList.isValid() || !directorList.isValid())
                refresh();
        }

        public IEmployee createEmployee(IUser user) {
            return createEmployee(user.EmployeeID);
        }

        public IEmployee createEmployee(int employeeID) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            IUser user;
            if (!userMgr.getAllUsers().Any(x => x.EmployeeID == employeeID))
                throw new ArgumentException(employeeID + " is not a valid employeeID.");

            user = userMgr.getUser(employeeID);
            int? groupManagerID = groupManagerList.Data
                                    .Where(x => x.ID == user.ManagerID).FirstOrDefault().ID;

            if (groupManagerID == null) {              // Create Group in repository 
                GroupManager groupManager = createGroupManager((int)user.ManagerID);
                this.groupManagerList.Data.Add(groupManager);
                HttpContext.Current.Application["GroupManager"] = this.groupManagerList;
                groupManagerID = groupManager.ID;
            }

            Employee employee = new Employee(user, DateTime.Today, null, (int)groupManagerID);
            this.employeeList.Data.Add(this.repository.saveEmployee(employee));
            HttpContext.Current.Application["Employee"] = this.employeeList;
            return employee;
            
        }

        public GroupManager createGroupManager(int employeeID) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            IUser user = userMgr.getUser(employeeID);
            
            int? groupID = getGroupList(true)
                                .Where(x => x.ID == employeeID)
                                .Select(y => y.GroupID)
                                .FirstOrDefault();
            int? directorID = getDirectorList(true)
                                .Where(x => x.ID == user.ManagerID)
                                .Select(y => y.ID)
                                .FirstOrDefault();

            if (groupID == null) 
                groupID = createGroup(user.DeptName).ID;

            if (directorID == null) {
                int? temp = userMgr.getAllUsers().Where(x => x.EmployeeID == groupID).Select(y => y.ManagerID).FirstOrDefault();
                directorID = createDirector((int)temp).ID;
            }

            GroupManager groupManager = this.repository.saveGroupManager((int)groupID, employeeID, (int)directorID);
            this.groupManagerList.Data.Add(groupManager);
            HttpContext.Current.Application.Add("GroupManager", groupManagerList);

            return groupManager;
        }

        public IEmployee createDirector(int employeeID) {
            Director director = this.repository.saveDirector(employeeID);
            this.directorList.Data.Add(director);
            HttpContext.Current.Application.Add("Director", directorList);

            return director;
        }

        public ILookup createGroup(string text) {
            Group group = new Group(text);
            return this.repository.saveGroup(group);
        }

        public IEmployee getUser(int employeeID) {
            if (this.employeeList == null || !this.employeeList.isValid())
                refresh();

            if (!employeeList.Data.Any(x => x.ID == employeeID))
                createEmployee(employeeID);

            return this.employeeList.Data.Where(x => x.ID == employeeID).OrderBy(y => y.EndDate == null ? DateTime.MinValue : y.EndDate).FirstOrDefault();
        }

        public IEmployee getUser(string ntLogin) {
            if (this.employeeList == null || !this.employeeList.isValid())
                refresh();
            return this.employeeList.Data.Where(x => x.User.NTLogin.Equals(ntLogin)).OrderBy(y => y.EndDate == null ? DateTime.MinValue : y.EndDate).FirstOrDefault();
        }

        public IEmployee getUser(IUser user) {
            return getUser(user.EmployeeID);
        }

        public IEmployee getUser(IEmployee employee) {
            if (this.employeeList == null || !this.employeeList.isValid())
                refresh();

            return this.employeeList.Data.Where(x => x.User.Equals(employee.User)).OrderBy(y => y.EndDate == null ? DateTime.MinValue : y.EndDate).FirstOrDefault();
        }

        public List<IEmployee> getAdminList() {
            if (this.employeeList == null || this.groupManagerList == null ||
                !this.employeeList.isValid() || !this.groupManagerList.isValid())
                refresh();

            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            // return the groupID with the name amo data team
            // return all employees part of that group.
            int groupManagerID = groupManagerList.Data.Where(x => x.Text.ToLower().Equals("amo data team")).OrderBy(y => y.EndDate ?? DateTime.MinValue).FirstOrDefault().GroupManagerID;
            IEnumerable<IEmployee> adminList = employeeList.Data.AsParallel().Where(y => y.GroupManagerID == groupManagerID);

            return adminList.OrderBy(x => x.User.EnglishName).OrderByDescending(y => y.Active).ToList();

            //return userMgr
            //    .getAllUsers()
            //    .Where(x => adminList.Any(y => y.ID == x.EmployeeID))
            //    .OrderBy(z => z.Name)
            //    //.Select(s => new {
            //    //    EnglishName = s.EnglishName,
            //    //    EmployeeID = s.EmployeeID,
            //    //    Enabled = adminList.Where(a => a.ID == s.EmployeeID).FirstOrDefault().EndDate == null
            //    //})
            //    .ToList();
        }

        public IList<Employee> getUserList(bool activeOnly) {
            if (this.employeeList == null || !this.employeeList.isValid()) 
                refresh();

            if (activeOnly)
                return this.employeeList.Data.Where(x => x.EndDate == null).ToList();
            else
                return this.employeeList.Data;
        }

        public IList<GroupManager> getGroupList(bool activeOnly) {
            if (this.groupManagerList == null || !this.employeeList.isValid())
                refresh();

            if (activeOnly)
                return this.groupManagerList.Data.Where(x => x.EndDate == null).ToList();
            else
                return this.groupManagerList.Data;
        }

        public IList<Director> getDirectorList(bool activeOnly) {
            if (this.directorList == null || !this.directorList.isValid())
                refresh();

            if (activeOnly)
                return this.directorList.Data.Where(x => x.EndDate == null).ToList();
            else
                return this.directorList.Data;
        }

        public int getUserGroupID(Employee employee) {
            if (this.groupManagerList == null || !this.groupManagerList.isValid())
                refresh();

            return this.groupManagerList.Data.Where(x => x.GroupManagerID == employee.GroupManagerID).FirstOrDefault().ID;
        }

        public string getUserGroupName(Employee employee) {
            if (this.groupManagerList == null || !this.groupManagerList.isValid())
                refresh();

            return this.groupManagerList.Data.Where(x => x.GroupManagerID == employee.GroupManagerID).FirstOrDefault().Text;
        }

        public bool isAdmin(Employee employee) {
            return employee.User.UserType == UserType.ADMIN || employee.User.UserType == UserType.ADMIN_MANAGER;
        }

        public void refresh() {
            this.employeeList = new ApplicationStore<IList<Employee>>(MainFactory.getCacheExpiration(), this.repository.getAllEmployees().Cast<Employee>().ToList());
            HttpContext.Current.Application["Employee"] = employeeList;
            this.groupManagerList = new ApplicationStore<IList<GroupManager>>(MainFactory.getCacheExpiration(), this.repository.getAllGroupManagers().Cast<GroupManager>().ToList());
            HttpContext.Current.Application["GroupManager"] = groupManagerList;
            this.directorList = new ApplicationStore<IList<Director>>(MainFactory.getCacheExpiration(), this.repository.getAllDirectors().Cast<Director>().ToList());
            HttpContext.Current.Application["Director"] = directorList;
        }
    }
}