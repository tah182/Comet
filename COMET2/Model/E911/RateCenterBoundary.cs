using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain.ArcGis;

namespace COMET.Model.E911 {
    public class RateCenterBoundary : ABoundary {
        
        public RateCenterBoundary(RateCenter_Features features)
            : base(features.Attributes.RCID, features.Attributes.ShapeFID) {
            this.Name = features.Attributes.Name;
        }

        public new string Name {
            get;
            private set;
        }
    }
}