using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.E911 {
    public class RateCenterDetails : RateCenterBoundary {
        
        public RateCenterDetails(RateCenter_Features features)
            : base(features) {
                this.Abbreviation = features.Attributes.Abbr;
                this.State = features.Attributes.State;
                this.OCName = features.Attributes.OCName;
                this.OCN = features.Attributes.OCN;
        }

        public string Abbreviation {
            get;
            private set;
        }

        public string State {
            get;
            private set;
        }

        public string OCName {
            get;
            private set;
        }

        public string OCN {
            get;
            private set;
        }
    }
}