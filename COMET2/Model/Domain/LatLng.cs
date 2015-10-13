using System;

namespace COMET.Model.Domain {
    /// <summary>
    /// Object defining a Geographic Point using Latitude and Longitude
    /// </summary>
    public class LatLng {
        /// <summary>
        /// Public Constructor for latitude and longitude.
        /// </summary>
        /// <param name="latitude">The latitude of the point</param>
        /// <param name="longitude">The longitude of the point</param>
        /// <exception cref="FormatException">Latitude must be between -90 and 90.</exception>
        /// <exception cref="FormatException">Longitude must be between -180 and 180.</exception>
        public LatLng(decimal latitude, decimal longitude) {
            if (!latitudeInBounds(latitude))
                throw new FormatException ("Latitude must be between -90 and 90 degrees.");
            if (!longitudeInBounds(longitude))
                throw new FormatException("Longitude must be between -180 and 180 degrees.");

            this.Lat = Math.Round(latitude, 6);
            this.Lng = Math.Round(longitude, 6);
        }

        /// <summary>
        /// Creates object using a Comma delimmited string of Lng,Lat.
        /// </summary>
        /// <param name="lngLat">The "Lng,Lat" string pair</param>
        public LatLng(string lngLat)
            : this(parseString(lngLat, true), parseString(lngLat, false)) {
        }

        /// <summary>
        /// Parses a comma separated string of lng,lat.
        /// </summary>
        /// <param name="lngLat">The string to parse</param>
        /// <param name="isLat">Flag for getting latitude back</param>
        /// <returns></returns>
        private static decimal parseString(string lngLat, bool isLat) {
            string[] ll = lngLat.Split(',');
            if (ll.Length != 2)
                throw new ArgumentException("Poorly formed LngLat combination. Received: " + lngLat);
            return isLat ? decimal.Parse(ll[1]) : decimal.Parse(ll[0]);
        }

        /// <summary>
        /// Validates the latitude is in bounds.
        /// </summary>
        /// <param name="latitude">The latitude to check</param>
        /// <returns>True if latitude is in bounds</returns>
        private bool latitudeInBounds(decimal latitude) {
            return latitude >= -90 && latitude <= 90;
        }

        /// <summary>
        /// Validates the longitude is in bounds.
        /// </summary>
        /// <param name="longitude">The longitude to check</param>
        /// <returns>True if longitude is in bounds</returns>
        private bool longitudeInBounds(decimal longitude) {
            return longitude >= -180 && longitude <= 180;
        }

        /// <summary>
        /// Returns the latitude of this point
        /// </summary>
        public decimal Lat {
            get;
            private set;
        }

        /// <summary>
        /// Returns the longitude of this point
        /// </summary>
        public decimal Lng {
            get;
            private set;
        }

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != typeof (LatLng))
                return false;
            return ((LatLng)obj).Lat == this.Lat && ((LatLng)obj).Lng == this.Lng;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return "Latitude: " + this.Lat + " -- Longitude: " + this.Lng;
        }
    }
}