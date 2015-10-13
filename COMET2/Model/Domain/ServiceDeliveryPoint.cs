using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class ServiceDeliveryPoint : IBuilding {
        /// <summary>
        /// Constructor setting readonly object
        /// </summary>
        /// <param name="services">The services available at this location in integer format</param>
        /// <param name="addressId">The addressID in the database for this object.</param>
        /// <param name="latlng">The latitude/longitude pair of the location</param>
        public ServiceDeliveryPoint(int services, int addressId, LatLng latlng) {
            this.Services = services;
            this.AddressID = addressId;
            this.LatLng = latlng;
        }

        /// <summary>
        /// Property: The integer representation of the services at the address.
        /// </summary>
        public int Services {
            get;
            private set;
        }

        /// <summary>
        /// Property: the database address id of this location.
        /// </summary>
        public int AddressID {
            get;
            private set;
        }

        /// <summary>
        /// Property: the latitude/longitude pair of this location.
        /// </summary>
        public LatLng LatLng {
            get;
            private set;
        }
    }
}