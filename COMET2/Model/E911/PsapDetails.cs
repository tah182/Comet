using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.E911 {
    public class PsapDetails : PsapBoundary {
        public PsapDetails(Psap_Features features)
            : base(features) {
            this.FCC_ID         = features.Attributes.FccId == 0 ? 300000 + features.Attributes.ObjectID : features.Attributes.FccId;
            this.CountyName     = features.Attributes.CountyName;
            this.CountyFips     = features.Attributes.CountyFips;
            this.Agency         = features.Attributes.Agency;
            this.CoverageArea   = features.Attributes.CoverageAr;
            this.PsapComments   = features.Attributes.PsapCommen;
            this.OperatorPhone  = features.Attributes.OperatorPh;
            this.ContactPrefix  = features.Attributes.ContactPre;
            this.ContactFirst   = features.Attributes.ContactFir;
            this.ContactTitle   = features.Attributes.ContactTit;
            this.ContactPhone   = features.Attributes.ContactPh;
            this.SiteState      = features.Attributes.SiteState;
        }
        public int FCC_ID {
            get;
            private set;
        }

        public string CountyName {
            get;
            private set;
        }

        public string CountyFips {
            get;
            private set;
        }

        public string Agency {
            get;
            private set;
        }

        public string CoverageArea {
            get;
            private set;
        }

        public string PsapComments {
            get;
            private set;
        }

        public string OperatorPhone {
            get;
            private set;
        }

        public string ContactPrefix {
            get;
            private set;
        }

        public string ContactFirst {
            get;
            private set;
        }

        public string ContactTitle {
            get;
            private set;
        }

        public string ContactPhone {
            get;
            private set;
        }

        public string SiteState {
            get;
            private set;
        }
    }
}