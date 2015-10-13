using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain.Shape {
    public class Polygon : ACoordShape {
        private readonly LatLng centroid;
        
        public Polygon(string name, string coordinateString) : base(name, coordinateString) {
            this.centroid = createCentroid(coordinateString);
            this.Color = this.Color ?? "";
        }

        public Polygon(string name, string coordinateString, string color)
            : this(name, coordinateString) {
                this.Color = color;
        }

        /// <summary>
        /// Constrcutor which includes a lat lng for the centroid of the shape
        /// </summary>
        /// <param name="name">The name or identifier for the polygon</param>
        /// <param name="coordinateString">String representation of Latitude Longitude points</param>
        /// <param name="lat">The Latitude of the centroid</param>
        /// <param name="lng">The Longitude of the centroid</param>
        public Polygon(string name, string coordinateString, decimal lat, decimal lng) : base(name, coordinateString) {
            this.centroid = new LatLng(lat, lng);
            this.Color = this.Color ?? "";
        }

        public Polygon(string name, string coordinateString, decimal lat, decimal lng, string color)
            : this(name, coordinateString, lat, lng) {
                this.Color = color;
        }

        public Polygon(string name, List<LatLng> points) : base(name, points) {
            this.centroid = createCentroid(points);
            this.Color = this.Color ?? "";
        }

        public Polygon(string name, List<LatLng> points, string color) : this(name, points){
            this.Color = color;
        }

        /// <summary>
        /// Constructor for a List of Lat/Lng for points and a lat/lng for the centroid of the shape
        /// </summary>
        /// <param name="name">The name or identifier for the polygon</param>
        /// <param name="points">The List of Lat/Lng for points</param>
        /// <param name="lat">The Latitude of the centroid</param>
        /// <param name="lng">The Longitude of the centroid</param>
        public Polygon(string name, List<LatLng> points, decimal lat, decimal lng): base(name, points) {
            this.centroid = new LatLng(lat, lng);
        }

        private LatLng createCentroid(string coordinateString) {
            List<LatLng> boundaryPoints = new List<LatLng>();

            string[] coordinates = coordinateString.Split('|');
            for (int i = 0; i < coordinates.Length; i++) {
                string[] coordinate = coordinates[i].Split(',');
                if (coordinate.Length != 2)
                    throw new FormatException("Coordinate String improperly formatted. Please use \"Lng,Lat|Lng,Lat\", received: " + coordinates);
                boundaryPoints.Add(new LatLng(decimal.Parse(coordinate[1]), decimal.Parse(coordinate[0])));
            }

            return createCentroid(boundaryPoints);
        }

        private LatLng createCentroid(List<LatLng> points) {
            // base assignment
            decimal minLat, minLng, maxLat, maxLng;
            minLat = minLng = 200;
            maxLat = maxLng = -200;

            if (points.Count() < 3) {
                string exception = "Shape requires at least 3 points to create, received (" + points.Count() + ") - ";
                foreach (LatLng latlng in points)
                    exception += latlng.ToString() + "\n";
                throw new FormatException(exception);
            }
            foreach (LatLng coordinate in points) {
                minLat = minLat < coordinate.Lat ? minLat : coordinate.Lat;
                maxLat = maxLat > coordinate.Lat ? maxLat : coordinate.Lat;
                minLng = minLng < coordinate.Lng ? minLng : coordinate.Lng;
                maxLng = maxLng > coordinate.Lng ? maxLng : coordinate.Lng;
            }
            return new LatLng((minLat + maxLat) / 2, (minLng + maxLng) / 2);
        }

        /// <summary>
        /// Returns the centroid either calculated or provided by the database
        /// </summary>
        public LatLng Centroid {
            get { return this.centroid; }
        }

        public string Color {
            get;
            set;
        }
    }
}