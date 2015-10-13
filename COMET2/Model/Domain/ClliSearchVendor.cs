using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class ClliSearchVendor {
        public string EnteredClli { get; set; }
        public string EnteredStreet { get; set; }
        public string EnteredCity { get; set; }
        public string EnteredState { get; set; }
        public decimal EnteredLat { get; set; }
        public decimal EnteredLng { get; set; }
        public short LATA { get; set; }
        public string Vendor { get; set; }
        public string VendorType { get; set; }
        public string CLLI_Code { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public decimal Distance_miles { get; set; }
    }
}