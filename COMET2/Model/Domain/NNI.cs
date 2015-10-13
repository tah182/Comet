using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class NNI : ABuilding {
        public NNI(
                    string trailName,
                    int? rfaid,
                    string vendor,
                    string ecckt,
                    int usedMbps,
                    int totalMbps,
                    int ranking,
                    int addressID,
                    string premise,
                    string street, 
                    string city,
                    string state,
                    string postalCode,
                    decimal lat,
                    decimal lng) : base(addressID) {

            this.TrailName = trailName;
            this.RFAID = rfaid;
            this.Vendor = vendor;
            this.ECCKT = ecckt;
            this.UsedMbps = usedMbps;
            this.TotalMbps = totalMbps;
            this.Ranking = ranking;
            this.Premise = premise;
            this.Street = street;
            this.City = city;
            this.State = state;
            this.PostalCode = postalCode;
            this.LatLng = new LatLng(lat, lng);
        }
        
        public string TrailName {
            get;
            private set;
        }

        public int? RFAID {
            get;
            private set;
        }

        public string Vendor {
            get;
            private set;
        }

        public string ECCKT {
            get;
            private set;
        }

        public int UsedMbps {
            get;
            private set;
        }

        public int TotalMbps {
            get;
            private set;
        }

        public double Utilization {
            get {
                return TotalMbps == 0 ? 0 : UsedMbps * 1.0 / TotalMbps;
            }
        }

        public int Ranking {
            get;
            private set;
        }
    }
}