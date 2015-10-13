using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain.Console;
using COMET.Model.Business.Factory;
using COMET.Model.Business.Service.Console;

namespace COMET.Model.Business.Manager.Console {
    public class EmployeeMgr {
        IEmployeeSvc repository;

        public EmployeeMgr(IEmployeeSvc repository) {
            this.repository = repository;
        }

        public IEmployee getUser(int employeeID) {
            return this.repository.getEmployee(employeeID);
        }

        public IEmployee getUser(string ntLogin) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            int employeeID = userMgr.getUser(ntLogin).EmployeeID;

            return this.repository.getEmployee(employeeID);
        }
    }
}