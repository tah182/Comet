using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain.Shape {
    public abstract class ACoordShape : ICoordShape {

        /// <summary>
        /// Constructor creating based on points in a string delimited by pipe '|'
        /// </summary>
        /// <param name="coordinateString">The string of Latitude, Longitude pairs delimited by a pipe '|'</param>
        public ACoordShape(string name, string coordinateString) {
            this.Coordinates = new List<LatLng>();
            this.Name = name;
            string[] coordinates = coordinateString.Split('|');
            if (coordinates.Length <= 1)
                throw new FormatException("Must have more than 1 point (Latitude / Longitude pair). Received: " + coordinateString);

            foreach (string coordinate in coordinates)
                this.Coordinates.Add(new LatLng(coordinate));
        }

        /// <summary>
        /// Constructor creating from List&lt;LatLng&gt;
        /// </summary>
        /// <param name="coordinateString">The List of LatLng.</param>
        public ACoordShape(string name, List<LatLng> points) {
            this.Coordinates = new List<LatLng>();
            this.Name = name;
            if (points.Count() <= 1)
                throw new FormatException("Must have more than 1 point (Latitude / Longitude pair). List only had " + points[0].Lat + ", " + points[0].Lng);

            this.Coordinates = points;
        }

        public string Name {
            get;
            private set;
        }

        public List<LatLng> Coordinates {
            get;
            set;
        }

        public override string ToString() {
            string returnString = "";
            foreach (LatLng latLng in this.Coordinates)
                returnString += latLng.ToString() + "\n";

            return returnString;
        }
    }
}