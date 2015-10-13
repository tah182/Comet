using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain.Shape {
    public class DouglasPeuckerReduction {
        List<LatLng> lineList;

        public DouglasPeuckerReduction(List<LatLng> lineList) {
            this.lineList = lineList;
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Uses the Douglas Peucker algorithm to reduce the number of point32s.
        /// </summary></span>
        /// <span class="code-SummaryComment"><param name="tolerance">The tolerance level.</param></span>
        /// <span class="code-SummaryComment"><returns></returns></span>
        //public List<LatLng> reduceLine(Double tolerance) {
        //    List<LatLng> result = Reduce(lineList, tolerance);
         
        //    return result;
        //}

        public List<LatLng> reduceLine(Int32 zoom, bool isShape = false) {
            Double tolerance = 360*1.0 / (256 * Math.Pow(zoom, 4));
            List<LatLng> result = Reduce(tolerance, isShape);

            return result;
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Uses the Douglas Peucker algorithm to reduce the number of point32s.
        /// </summary></span>
        /// <span class="code-SummaryComment"><param name="point32s">The point32s.</param></span>
        /// <span class="code-SummaryComment"><param name="Tolerance">The tolerance.</param></span>
        /// <span class="code-SummaryComment"><returns></returns></span>
        public List<LatLng> Reduce(double tolerance, bool isShape) {
            if (this.lineList == null || this.lineList.Count < 3)
                return this.lineList;

            List<LatLng> coordinates = new List<LatLng>();
            coordinates.Add(this.lineList[0]);
            //if (isShape)            // it's a shape. It needs 3 points so store the middle
            //    coordinates.Add(this.lineList[this.lineList.Count / 2]);
            
            LatLng baseCoord = this.lineList[0];
            double sqTolerance = tolerance * tolerance;
            for (int i = 1; i < this.lineList.Count - 1; i++) {
                // check to see if the distance between the base coordinate and the coordinate in question is outside the tolerance distance.
                if (Math.Pow((double)(this.lineList[i].Lat - baseCoord.Lat), 2) +
                    Math.Pow((double)(this.lineList[i].Lng - baseCoord.Lng), 2) > sqTolerance) {
                    // store the coordinate and make it the new base for comparison
                    coordinates.Add(this.lineList[i]);
                    baseCoord = this.lineList[i];
                }
            }

            // store the last coordinate
            coordinates.Add(this.lineList[this.lineList.Count - 1]);
            return coordinates;
        }

        /// <span class="code-SummaryComment"><summary>
        /// Douglases the peucker reduction.</summary></span>
        /// <span class="code-SummaryComment"><param name="point32s">The point32s.</param></span>
        /// <span class="code-SummaryComment"><param name="firstpoint32">The first point32.</param></span>
        /// <span class="code-SummaryComment"><param name="lastpoint32">The last point32.</param></span>
        /// <span class="code-SummaryComment"><param name="tolerance">The tolerance.</param></span>
        /// <span class="code-SummaryComment"><param name="point32IndexsToKeep">The point32 index to keep.</param></span>
        private void Reduce(List<LatLng> points, Int32 firstpoint, Int32 lastpoint, Double tolerance,
            ref List<Int32> pointIndexesToKeep) {
            if (firstpoint == lastpoint)
                return;
            Double maxDistance = 0;
            Int32 indexFarthest = 0;

            for (Int32 index = firstpoint; index < lastpoint; index++) {
                Double distance = PerpendicularDistance(
                                                    points[firstpoint],
                                                    points[lastpoint],
                                                    points[index]);
                if (distance > maxDistance) {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > tolerance && indexFarthest != 0) {
                //Add the largest point3232> that exceeds the tolerance
                pointIndexesToKeep.Add(indexFarthest);

                Reduce(points, firstpoint, indexFarthest, tolerance, ref pointIndexesToKeep);
                Reduce(points, indexFarthest, lastpoint, tolerance, ref pointIndexesToKeep);
            }
        }

        /// <span class="code-SummaryComment"><summary>
        /// The distance of a point32 from a line made from point321 and point322.
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="pt1">The PT1.</param></span>
        /// <span class="code-SummaryComment"><param name="pt2">The PT2.</param></span>
        /// <span class="code-SummaryComment"><param name="p">The p.</param></span>
        /// <span class="code-SummaryComment"><returns></returns></span>
        private Double PerpendicularDistance(LatLng point1, LatLng point2, LatLng point) {
            //Area = |(1/2)(x1y2 + x2y3 + x3y1 - x2y1 - x3y2 - x1y3)|   *Area of triangle
            //Base = v((x1-x2)²+(x1-x2)²)                               *Base of Triangle*
            //Area = .5*Base*H                                          *Solve for height
            //Height = Area/.5/Base

            Double pLat, pLng, p1Lat, p1Lng, p2Lat, p2Lng;
            p1Lat = Convert.ToDouble(point1.Lat);
            p1Lng = Convert.ToDouble(point1.Lng);
            p2Lat = Convert.ToDouble(point2.Lat);
            p2Lng = Convert.ToDouble(point2.Lng);
            pLat = Convert.ToDouble(point.Lat);
            pLng = Convert.ToDouble(point.Lng);

            Double area = Math.Abs(.5 * (
                        p1Lat * p2Lng + 
                        p2Lat * pLng *
                        pLat * p1Lng - 
                        p2Lat * p1Lng - 
                        pLat * p2Lng -
                        p1Lat * pLng));
            Double bottom = Math.Sqrt(Math.Pow(p1Lat - p2Lat, 2) +
                            Math.Pow(p1Lng - p2Lng, 2));
            Double height = area / bottom * 2;

            return height;
        }

        private Double DistanceBetweenOn2DPlane(LatLng p1, LatLng p2) {
            return Math.Sqrt(Math.Pow(Convert.ToDouble(p1.Lat - p2.Lat), 2) + Math.Pow(Convert.ToDouble(p1.Lng - p2.Lng), 2));
        }
    }
}