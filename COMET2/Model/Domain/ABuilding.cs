using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    /// <summary>
    /// Abstract building type for any physical location with an address
    /// </summary>
    public abstract class ABuilding {

        /// <summary>
        /// Creates a building with an address ID 
        /// </summary>
        /// <param name="addressID">The Identifier of the Location</param>
        public ABuilding(int addressID) {
            this.AddressID = addressID;
        }

        /// <summary>
        /// Creates an empty building
        /// </summary>
        public ABuilding() {

        }

        /// <summary>
        /// Access only for the AddressID of the building
        /// </summary>
        public int AddressID {
            get;
            private set;
        }

        /// <summary>
        /// The Premise of the Street Address, typically with a numerical value
        /// </summary>
        public string Premise { get; set; }

        /// <summary>
        /// The street of the Address
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Combines the Premise and Street of the Address
        /// </summary>
        public string FullStreet {
            get { return Premise + " " + Street; }
        }

        /// <summary>
        /// The City in which the building is located
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The state in which the building is located
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The Postal Code in which the building is located
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// The Country in which the building is located
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The LatLng coordinates of the address.
        /// </summary>
        public LatLng LatLng { get; set; }

        /// <summary>
        /// Overrides the ToString method by providing the FullStreet, City, State and PostalCode
        /// </summary>
        /// <returns>The formatted Premise, Street, City, State and PostalCode</returns>
        override
        public string ToString() {
            string returnString =
                FullStreet + "\n" +
                City + ", " + State + " " + PostalCode;
            return returnString;
        }
    }
}