using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Factory;
using COMET.Model.Business.Service;
using COMET.Model.Domain;
using COMET.Server.Domain;

namespace COMET.Model.Business.Manager {
    public class BuildingMgr : IManager {
        IBuildingSvc svc;
        const int TREND_WEEKS = 8;

        ApplicationStore<IList<IBuilding>> sdpAssetList;
        ApplicationStore<IList<EntranceFacility>> entranceFacilityList;
        ApplicationStore<IList<SDP_TREND>> trendList;

        public BuildingMgr(IBuildingSvc svc) {
            this.svc = svc;
            this.sdpAssetList = (ApplicationStore<IList<IBuilding>>)HttpContext.Current.Application["sdpAsset"];
            this.entranceFacilityList = (ApplicationStore<IList<EntranceFacility>>)HttpContext.Current.Application["entranceFacility"];
            this.trendList = (ApplicationStore<IList<SDP_TREND>>)HttpContext.Current.Application["trendList"];
            if (this.sdpAssetList == null || !this.sdpAssetList.isValid() ||
                this.entranceFacilityList == null || !this.entranceFacilityList.isValid() ||
                this.trendList == null || !this.trendList.isValid())
                    refresh();
        }

        public IList<IBuilding> getBuildings() {
            return this.sdpAssetList.Data;
        }

        public IList<Building> getbuildingClli(string search) {
            IList<Building> clli = this.svc.getBuildingCllis(search);
            return clli;
        }

        public IList<EntranceFacility> getEntranceFacility() {
            return this.entranceFacilityList.Data;
        }

        public IList<decimal> getSDPTrend(int addresssID, int type) {
            List<decimal> thisTrend = new List<decimal>();
            SDP_TREND t = this.trendList.Data.FirstOrDefault(x => x.ADDRESS_ID == addresssID);
            switch (type) {
                // lsefShared
                case 0:
                    thisTrend.Add(t.LSEF1 ?? 0);
                    thisTrend.Add(t.LSEF2 ?? 0);
                    thisTrend.Add(t.LSEF3 ?? 0);
                    thisTrend.Add(t.LSEF4 ?? 0);
                    thisTrend.Add(t.LSEF5 ?? 0);
                    thisTrend.Add(t.LSEF6 ?? 0);
                    thisTrend.Add(t.LSEF7 ?? 0);
                    thisTrend.Add(t.LSEF8 ?? 0);
                    break;
                // hsefShared
                case 1:
                    thisTrend.Add(t.HSEF1 ?? 0);
                    thisTrend.Add(t.HSEF2 ?? 0);
                    thisTrend.Add(t.HSEF3 ?? 0);
                    thisTrend.Add(t.HSEF4 ?? 0);
                    thisTrend.Add(t.HSEF5 ?? 0);
                    thisTrend.Add(t.HSEF6 ?? 0);
                    thisTrend.Add(t.HSEF7 ?? 0);
                    thisTrend.Add(t.HSEF8 ?? 0);
                    break;
                // nniShared
                case 2:
                    thisTrend.Add(t.NNI1 ?? 0);
                    thisTrend.Add(t.NNI2 ?? 0);
                    thisTrend.Add(t.NNI3 ?? 0);
                    thisTrend.Add(t.NNI4 ?? 0);
                    thisTrend.Add(t.NNI5 ?? 0);
                    thisTrend.Add(t.NNI6 ?? 0);
                    thisTrend.Add(t.NNI7 ?? 0);
                    thisTrend.Add(t.NNI8 ?? 0);
                    break;
                default:
                    break;
            }
            return thisTrend;
        }

        public void refresh() {
            this.sdpAssetList = new ApplicationStore<IList<IBuilding>>(MainFactory.getCacheExpiration(), this.svc.getBuildings());
            this.entranceFacilityList = new ApplicationStore<IList<EntranceFacility>>(MainFactory.getCacheExpiration(), this.svc.getEntranceFacilities());
            this.trendList = new ApplicationStore<IList<SDP_TREND>>(MainFactory.getCacheExpiration(), this.svc.getSDPTrend(TREND_WEEKS));

            HttpContext.Current.Application["sdpAsset"] = sdpAssetList;
            HttpContext.Current.Application["entranceFacility"] = entranceFacilityList;
            HttpContext.Current.Application["trendList"] = trendList;
            this.svc.close();
        }
    }
}