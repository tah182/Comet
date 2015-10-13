using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain.Shape {
    public class Line : ACoordShape {
        public Line(string name, string coordinateString) : base(name, coordinateString) {
        }

        public Line(int polygon, string coordinateString)
            : base(polygon.ToString(), coordinateString) {

        }

        public Line(string name, List<LatLng> points) : base(name, points) {
        }

        public Line(int polygon, List<LatLng> points)
            : base(polygon.ToString(), points) {

        }

        public string Color { get; set; }
    }
}