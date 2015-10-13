using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;

using COMET.Model.Business.Factory;
using COMET.Model.Domain;
using COMET.Server.Domain;

namespace COMET.Model.Business.Service {
    public class StatesSvcImplDB : IRegion {
        private IList<KeyValue<string, string>> states;

        public void ThreadRun() {
            states = (IList<KeyValue<string, string>>)HttpRuntime.Cache["States"];
            if (states == null) {
                try {
                    using (ReferenceDataContext dc = (ReferenceDataContext) MainFactory.getDb("Reference", true)) {
                        states = (from state in dc.GetTable<LKP_POSTAL_CD_STATE>()
                                  join abbr in dc.GetTable<STATES_ABBREVIATION>()
                                  on state.STATE_CD equals abbr.abbreviation
                                  where !abbr.name.StartsWith("Dist")
                                  select new KeyValue<string, string> { 
                                      Key = "(" + state.STATE_CD + ") " + abbr.name, 
                                      Value = state.STATE_CD })
                                  .Distinct()
                                  .OrderBy(x => x.Key)
                                  .ToList();
                                                
                        HttpRuntime.Cache.Insert("States", states,
                            null, MainFactory.getCacheExpiration(), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                } catch (SqlException se) {
                    ILogSvc logger = MainFactory.getLogSvc();
                    logger.logError("Index", "StatesSvc", "s-tr-01", se.Message + "<br>" + se.StackTrace);
                }
            }
        }

        public IList<KeyValue<string, string>> getData() {
            return this.states;
        }
    }
}