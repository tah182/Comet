using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain.ArcGis;

namespace COMET.Model.E911 {
    public class LataBoundary : ABoundary {
        public LataBoundary(Lata_Features features)
            : base((long)features.Attributes.Lata, features.Attributes.Lata) {
        }
    }
}