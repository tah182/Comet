using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Factory;

namespace COMET.Model.Domain.ArcGis {
    public class FiberVendorDetails {
        private int[] color;
        public FiberVendorDetails(int id, int[] color) {
            this.ID = id;
            this.color = color;
        }

        public int ID {
            get;
            private set;
        }

        public string ColorHex {
            get {
                return ColorFactory.rgb2Color((byte)color[0], 
                                             (byte)color[1],
                                             (byte)color[2]);
            }
        }

        public int[] ColorRGB {
            get {
                return color;
            }
        }
    }
}