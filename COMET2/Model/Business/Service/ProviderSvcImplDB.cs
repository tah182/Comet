using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using COMET.Server.Domain;
using COMET.Model.Domain;
using COMET.Model.Business.Factory;

namespace COMET.Model.Business.Service {
    public class ProviderSvcImplDB : IRegion {
        private IList<KeyValue<string, string>> providers;
        DateTime lastUpdatedDB, lastUpdatedApp;

        public ProviderSvcImplDB() {
            lastUpdatedDB = ReferenceFactory.lastUpdated("SDP_UPDATED");
            lastUpdatedApp = HttpContext.Current.Application["ProviderUpdated"] == null
                                    ? ReferenceFactory.getDefaultDate()
                                    : (DateTime)HttpContext.Current.Application["ProviderUpdated"];
        }

        public void ThreadRun() {
            providers = (IList<KeyValue<string, string>>)HttpRuntime.Cache["Providers"];
            if (providers == null || lastUpdatedDB > lastUpdatedApp) {
                try {
                    using (ReferenceDataContext dc = (ReferenceDataContext)MainFactory.getDb("Reference", true)) {
                        providers = (from cableOperators in dc.GetTable<CABLE_OPERATOR_AREAS_FCC_EXCEL>()
                                     join vendor in dc.GetTable<LKP_VENDOR_TYP_XREF_EXCEL>()
                                     on cableOperators.Legal_Name equals vendor.UPPER_VENDOR_NAME
                                     where cableOperators.CUID_Status.ToLower().Equals("active")
                                         && vendor.XREF_PARENT_VENDOR != null
                                         && vendor.VENDOR_TYPE.ToUpper().Equals("MSO")
                                     select new KeyValue<string, string> {
                                         Key = vendor.XREF_PARENT_VENDOR,
                                         Value = vendor.XREF_PARENT_VENDOR })
                                     .Distinct()
                                     .OrderBy(x => x.Key)
                                     .ToList();

                        foreach (var keyvalue in providers) 
                            keyvalue.Value = keyvalue.Key = MainFactory.formatProvider(keyvalue.Key);

                        HttpRuntime.Cache.Insert("Providers", providers,
                            null, MainFactory.getCacheExpiration(), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                } catch (SqlException se) {
                    ILogSvc logger = MainFactory.getLogSvc();
                    logger.logError("Index", "ProviderSvc", "p-tr-01", se.Message + "<br>" + se.StackTrace);
                }
            }
        }

        public IList<KeyValue<string, string>> getData() {
            return this.providers;
        }
    }
}