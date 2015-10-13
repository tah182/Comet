using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class TrendSlope {
        public static readonly TrendSlope UP = new TrendSlope("~/Images/GreenUp.png");
        public static readonly TrendSlope DOWN = new TrendSlope("~/Images/RedDown.png");
        public static readonly TrendSlope FLAT = new TrendSlope("~/Images/BlackRight.png");
        public static readonly TrendSlope NONE = new TrendSlope("~/Images/Blank.png");

        public static IEnumerable<TrendSlope> Values {
            get {
                yield return UP;
                yield return DOWN;
                yield return FLAT;
                yield return NONE;
            }
        }

        private readonly string image;

        TrendSlope(string image) {
            this.image = image;
        }

        public string Image {
            get { return this.image; }
        }
    }
}