using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Factory;

namespace COMET.Model.E911 {
    public class Details911 {
        private readonly RateCenterDetails rcDetails;
        private readonly PsapDetails psapDetails;
        private readonly CountyDetails countyDetails;
        private readonly string zip;
        private readonly bool showIcon;

        public Details911(RateCenter_Features rcFeatures, Psap_Features psapFeatures, County_Features countyFeatures, string zip, bool showIcon) {
            if (rcFeatures != null)
                this.rcDetails = new RateCenterDetails(rcFeatures);
            if (psapFeatures != null)
                this.psapDetails = new PsapDetails(psapFeatures);
            if (countyFeatures != null)
                this.countyDetails = new CountyDetails(countyFeatures);
            this.zip = zip;
            this.showIcon = showIcon;
        }

        public string County {
            get {
                string county = this.countyDetails == null ? "__" : MainFactory.formatProvider(this.countyDetails.Name);
                string state = this.psapDetails == null ? "__" : this.psapDetails.SiteState;
                if (county.Equals("__") && state.Equals("__"))
                    return "UNKNOWN";
                if (county.Equals("__") && !state.Equals("__"))
                    return "Unknown County in " + state;
                if (!county.Equals("__") && state.Equals("__"))
                    return county + " in unknown state";
                return county + ", " + state; }
        }

        public string Fips {
            get {
                if (countyDetails == null)
                    return "UNKNOWN";
                return this.countyDetails.CountyKey; }
        }

        public string RateCenter {
            get {
                if (rcDetails == null)
                    return "UNKNOWN";
                return MainFactory.formatProvider(this.rcDetails.Name) + ", " + this.rcDetails.State; }
        }

        public long FCCID {
            get {
                if (psapDetails == null)
                    return 0L;
                return this.psapDetails.FCC_ID; }
        }

        public string Agency {
            get {
                if (psapDetails == null)
                    return "UNKNOWN";
                return MainFactory.formatProvider(this.psapDetails.Agency); }
        }

        public string CoverageArea {
            get {
                if (psapDetails == null)
                    return "None";
                return MainFactory.formatProvider(this.psapDetails.CoverageArea ?? "None"); }
        }

        public string PsapComments {
            get {
                if (psapDetails == null)
                    return null;
                return this.psapDetails.PsapComments; }
        }

        public string ZipCode {
            get {
                if (zip == null) return "UNKNOWN";
                return this.zip;
            }
        }

        public bool ShowIcon {
            get { return this.showIcon; }
        }
    }
}