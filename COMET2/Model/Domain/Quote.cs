using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class Quote : ABuilding {
        public Quote(int AddressId) : base(AddressId) { }

        public Quote(			
                        int rank,
                        int address_ID,
			            int entityLineItemId, 
			            string bandwidthDesc,
			            string productDesc,
			            decimal mrcUSD,
			            string vendor,
			            string vendor_Type,
			            int priceTermId,
			            decimal incrementalMrCost,
			            decimal? incrementalNrCost,
			            bool isWin,
			            int? bandwidthEthernet,
			            int bandwidthCapacity,
			            DateTime quoteCreateDate,
			            string premise,
			            string street,
			            string city,
			            string state,
                        string addressCLLI,
                        string swcCLLI,
                        decimal distance,
                        LatLng latLng) : base(address_ID) {

            this.Premise = premise;
            this.Street = street;
            this.City = city;
            this.State = state;
            this.AddressClli = addressCLLI;
            this.SwcCLLI = swcCLLI;

            this.Rank = rank;
            this.EntityLineItemId = entityLineItemId;
            this.BandwidthDesc = bandwidthDesc;
            this.ProductDesc = productDesc;
            this.mrcUSD = mrcUSD;
            this.Vendor = vendor;
            this.VendorType = vendor_Type;
            this.PriceTermId = priceTermId;
            this.distance = distance;
            this.IncrementalMrCost = incrementalMrCost;
            this.IncrementalNrCost = incrementalNrCost;
            this.IsWin = isWin;
            this.BandwidthEthernet = bandwidthEthernet;
            this.BandwidthCapacity = bandwidthCapacity;
            this.QuoteCreateDate = quoteCreateDate;
            this.LatLng = latLng;
        }

        public int Rank {
            get;
            private set;
        }

        public string AddressClli {
            get;
            private set;
        }

        public string SwcCLLI {
            get;
            private set;
        }

        public int EntityLineItemId {
            get;
            private set;
        }

        public string BandwidthDesc {
            get;
            private set;
        }

        public string ProductDesc {
            get;
            private set;
        }

        public decimal mrcUSD {
            get;
            private set;
        }

        public string Vendor {
            get;
            private set;
        }

        public string VendorType {
            get;
            private set;
        }

        public int PriceTermId {
            get;
            private set;
        }

        public decimal? IncrementalNrCost {
            get;
            private set;
        }

        public decimal IncrementalMrCost {
            get;
            private set;
        }

        public bool IsWin {
            get;
            private set;
        }

        public int? BandwidthEthernet {
            get;
            private set;
        }

        public int BandwidthCapacity {
            get;
            private set;
        }

        public DateTime QuoteCreateDate {
            get;
            private set;
        }

        public decimal distance {
            get;
            private set;
        }
    }
}