using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Factory;

namespace COMET.Model.Domain {
    public class NetworkBuilding : Building {
        private string vendor;

        public NetworkBuilding(int addressID, string clli)
            : base(addressID) {
                this.CLLI = clli;
        }
        
        public NetworkBuilding(int addressID)
            : base(addressID) {

        }

        public NetworkBuilding(string clli)
            : base(clli) {
        }

        public NetworkBuilding()
            : base() {
        }

        public string Vendor {
            get { return this.vendor; }
            set {
                this.vendor = MainFactory.formatProvider(value);
            }
        }

        public string VendorType {
            get;
            set;
        }

        public int Rank {
            get;
            set;
        }

        public decimal DistanceMiles {
            get;
            set;
        }
    }
}