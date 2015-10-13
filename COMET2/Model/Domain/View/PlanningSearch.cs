using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain.View {
    public class PlanningSearch {
        public PlanningSearch() {

        }

        public PlanningSearch(List<NetworkBuilding> netBuilding, string lata, string swc, string lecVendor, string postal, string[] mso) {
            this.NetBuildList = netBuilding;
            this.Lata = lata;
            this.SWC = swc;
            this.LECVendor = lecVendor;
            this.PostalCode = postal;
            this.MSO = mso;
        }

        public List<NetworkBuilding> NetBuildList {
            get;
            set;
        }

        public string Lata {
            get;
            set;
        }

        public string SWC {
            get;
            set;
        }

        public string LECVendor {
            get;
            set;
        }

        public string PostalCode {
            get;
            set;
        }

        public string[] MSO {
            get;
            set;
        }
    }
}