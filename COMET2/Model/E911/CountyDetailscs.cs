using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.E911 {
    public class CountyDetails : CountyBoundary {
        private readonly string countyKey;
        public CountyDetails(County_Features features)
            : base(features) {
            this.countyKey = features.Attributes.County_Key;
        }
        public string CountyKey {
            get { return this.countyKey; }
        }
    }
}