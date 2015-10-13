using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain.ArcGis {
    public class Fiber : ABoundary {
        public Fiber(GisFeatures features)
            : base(features.Attributes.Carrier) {
        }
    }
}