using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Factory;
using COMET.Model.Business.Service;
using COMET.Model.Domain;

namespace COMET.Model.Business.Manager {
    public class UserMgr : IManager {
        IUserSvc svc;
        ApplicationStore<IList<IUser>> userList;

        public UserMgr(IUserSvc svc) {
            this.svc = svc;
            userList = (ApplicationStore<IList<IUser>>)HttpContext.Current.Application["Users"];
            if (this.userList == null || !this.userList.isValid())
                refresh();
        }

        public IList<IUser> getAllUsers() {
            if (this.userList == null || !this.userList.isValid())
                refresh();
            
            return this.userList.Data;
        }

        public IUser getUser(int employeeID) {
            if (this.userList == null || !this.userList.isValid())
                refresh();

            return this.userList.Data.AsParallel().Where(x => x.EmployeeID == employeeID).FirstOrDefault();
        }

        public IUser getUser(string ntLogin) {
            if (this.userList == null || !this.userList.isValid())
                refresh();

            return this.userList.Data.AsParallel().Where(x => x.NTLogin.Equals(ntLogin)).FirstOrDefault();
        }

        public IUser addUser(IUser user) {
            this.svc.addUser(user);
            
            if (this.userList == null || !this.userList.isValid()) 
                refresh();
            else
                this.userList.Data.Add(user);

            return this.userList.Data.AsParallel().Where(x => x.Equals(user)).FirstOrDefault();
        }

        public bool updateUserAcknowledged(IUser user) {
            bool updated = this.svc.setUserAcknowledge(user);
            if (this.userList == null || !this.userList.isValid())
                refresh();

            this.userList.Data.AsParallel().Where(x => x.EmployeeID.Equals(user)).FirstOrDefault().setAcknowledged();
            return updated;
        }

        public IList<IUser> retreiveHeirarchy(int id) {
            IUser user = getUser(id);
            return this.svc.retreiveHeirarchy(user);
        }

        public IList<Activity> usagebyWeek() {
            return this.svc.usagebyWeek();
        }

        public IList<string> usersInGroup(string userGroup) {
            return this.svc.usersInGroup(userGroup);
        }

        public void refresh() {
            this.userList = new ApplicationStore<IList<IUser>>(MainFactory.getCacheExpiration(), this.svc.getAllUsers());
            var temp = userList.Data.Where(x => x.AcknowledgedUpdateNotes).ToList();
            HttpContext.Current.Application["Users"] = userList;
        }
    }
}