using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain.ArcGis;

namespace COMET.Model.E911 {
    public class PsapBoundary : ABoundary {
        public PsapBoundary(Psap_Features features)
            : base(features.Attributes.PsapId, features.Attributes.ObjectID) {
        }
    }
}