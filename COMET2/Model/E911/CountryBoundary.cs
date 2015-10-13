using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain.ArcGis;

namespace COMET.Model.E911 {
    public class CountyBoundary : ABoundary {
        
        public CountyBoundary(County_Features features)
            : base((long)features.Attributes.ObjectID, features.Attributes.ObjectID) {
            this.Name = features.Attributes.Name;
        }

        public new string Name {
            get;
            private set;
        }
    }
}