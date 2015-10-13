using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class Building : ABuilding {
        private string clli;
        public Building(int addressID)
            : base(addressID) {

        }

        public Building()
            : base() {
        }

        public Building(string clli)
            : base() {
                this.CLLI = clli;
        }

        public string CLLI {
            get { return this.clli; }
            set {
                if (value != null && value.Length != 8)
                    throw new ArgumentException("Expected CLLI length of 8. Got " + value.Length);
                this.clli = value;
            }
        }
    }
}