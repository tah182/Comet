using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Model.Business.Service;

namespace COMET.Model.Business.Manager {
    public class BulkMgr : IManager {
        IBulkSvc svc;

        public BulkMgr(IBulkSvc svc) {
            this.svc = svc;
        }

        public IList<ClliSearchVendor> getBulkByCLLI(string clliString, float distance, int topCount) {
            return this.svc.getBulkByCLLI(clliString, distance, topCount);
        }

        public void refresh() { }
    }
}