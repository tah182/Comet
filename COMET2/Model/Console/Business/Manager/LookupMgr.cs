using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Console.Business.Service;
using COMET.Model.Business.Factory;
using COMET.Model.Console.Domain;
using COMET.Model.Domain;

namespace COMET.Model.Business.Manager {
    public class LookupMgr : IManager {
        private readonly IRequestSvc svc;
        private ApplicationStore<IList<SupportArea>> supportAreaList;
        private ApplicationStore<IList<ALookup>> supportUnitList;
        private ApplicationStore<IList<LookupActive>> requestTypeList;
        private ApplicationStore<IList<LookupActive>> projectTypeList;
        private ApplicationStore<IList<LookupActive>> requestCategoryList;
        private ApplicationStore<IList<ALookup>> valueDriverList;
        private ApplicationStore<IList<ALookup>> programList;

        public LookupMgr(IRequestSvc svc) {
            this.svc = svc;

            this.supportAreaList = (ApplicationStore<IList<SupportArea>>)HttpContext.Current.Application["SupportArea"];
            this.supportUnitList = (ApplicationStore<IList<ALookup>>)HttpContext.Current.Application["SupportUnit"];
            this.requestTypeList = (ApplicationStore<IList<LookupActive>>)HttpContext.Current.Application["RequestType"];
            this.projectTypeList = (ApplicationStore<IList<LookupActive>>)HttpContext.Current.Application["ProjectType"];
            this.requestCategoryList = (ApplicationStore<IList<LookupActive>>)HttpContext.Current.Application["RequestCategory"];
            this.valueDriverList = (ApplicationStore<IList<ALookup>>)HttpContext.Current.Application["ValueDriver"];
            this.programList = (ApplicationStore<IList<ALookup>>)HttpContext.Current.Application["Program"];

            if (this.supportAreaList == null || this.supportUnitList == null || this.requestTypeList == null || this.requestCategoryList == null || this.valueDriverList == null || this.programList == null ||
                !this.supportUnitList.isValid() || !this.supportUnitList.isValid() || !this.requestTypeList.isValid() || !this.requestCategoryList.isValid() || !this.valueDriverList.isValid() || !this.programList.isValid())
                refresh();
        }
        
        public IList<SupportArea> getSupportAreas() {
            return this.supportAreaList.Data;
        }

        public bool isValidSupportArea(int value) {
            return getSupportAreas().Any(x => x.ID == value);
        }

        public IList<ALookup> getSupportUnits() {
            return this.supportUnitList.Data;
        }

        public IList<ALookup> getPrograms() {
            return this.programList.Data;
        }

        public bool isValidSupportUnit(int value) {
            return getSupportUnits().Any(x => x.ID == value);
        }
        
        public IList<LookupActive> getRequestTypes(EOpenType type, bool activeOnly) {
            IList<LookupActive> requestType;
            if (type == EOpenType.Request)
                requestType = this.requestTypeList.Data;
            else
                requestType = this.projectTypeList.Data;


            if (activeOnly)
                return requestType.Where(x => x.Active == activeOnly).OrderBy(y => y.Text).ToList();
            else
                return requestType.OrderBy(x => x.Text).OrderByDescending(y => y.Active).ToList();
        }

        public IList<LookupActive> getRequestCategories(bool activeOnly) {
            if (activeOnly)
                return this.requestCategoryList.Data.Where(x => x.Active == activeOnly).OrderBy(y => y.Text).ToList();
            else
                return this.requestCategoryList.Data.OrderBy(x => x.Text).OrderByDescending(y => y.Active).ToList();
        }

        public IList<ALookup> getValueDrivers() {
            return this.valueDriverList.Data;
        }

        public IList<IUser> getEmployees(string match, bool activeOnly) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            EmployeeMgr employeeMgr = new EmployeeMgr(ConsoleFactory.getEmployeeSvc());
            IList<IEmployee> list = employeeMgr.getUserList(activeOnly).Cast<IEmployee>().ToList();
            IList<IUser> userList = userMgr.getAllUsers().Where(x => x.Name.ToLower().Contains(match.ToLower())).ToList();

            IList<IUser> users = (from a in list
                                  join e in userList
                                  on a.ID equals e.EmployeeID
                                  select e).ToList();
            return users;
        }

        public void refresh() {
            this.supportAreaList = new ApplicationStore<IList<SupportArea>>(MainFactory.getCacheExpiration(), this.svc.getSupportAreas().OrderBy(x => x.Text).ToList());
            this.supportUnitList = new ApplicationStore<IList<ALookup>>(MainFactory.getCacheExpiration(), this.svc.getSupportUnits().OrderBy(x => x.Text).ToList());
            this.requestTypeList = new ApplicationStore<IList<LookupActive>>(MainFactory.getCacheExpiration(), this.svc.getRequestTypes().OrderBy(x => x.Text).ToList());
            this.projectTypeList = new ApplicationStore<IList<LookupActive>>(MainFactory.getCacheExpiration(), this.svc.getProjectTypes().OrderBy(x => x.Text).ToList());
            this.requestCategoryList = new ApplicationStore<IList<LookupActive>>(MainFactory.getCacheExpiration(), this.svc.getRequestCategories().OrderBy(x => x.Text).ToList());
            this.valueDriverList = new ApplicationStore<IList<ALookup>>(MainFactory.getCacheExpiration(), this.svc.getValueDrivers().OrderBy(x => x.Text).ToList());
            this.programList = new ApplicationStore<IList<ALookup>>(MainFactory.getCacheExpiration(), this.svc.getPrograms().OrderBy(x => x.Text).ToList());

            HttpContext.Current.Application["SupportArea"] = supportAreaList;
            HttpContext.Current.Application["SupportUnit"] = supportUnitList;
            HttpContext.Current.Application["RequestType"] = requestTypeList;
            HttpContext.Current.Application["ProjectType"] = projectTypeList;
            HttpContext.Current.Application["RequestCategory"] = requestCategoryList;
            HttpContext.Current.Application["ValueDriver"] = valueDriverList;
            HttpContext.Current.Application["Program"] = programList;
        }
    }
}