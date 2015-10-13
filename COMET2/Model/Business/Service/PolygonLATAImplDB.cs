using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Model.Domain.ArcGis;
using COMET.Model.Domain.Shape;
using COMET.Model.Business.Factory;

using COMET.Server.Domain;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Service that retrieves LATA from the GAM database
    /// </summary>
    public class PolygonLATAImplDB : IPolygonSvc {
        private IList<ICoordShape> shapes;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public PolygonLATAImplDB() {
            shapes = (List<ICoordShape>)HttpRuntime.Cache["LataBoundary"];
        }

        public IList<ICoordShape> getPolygons(decimal bottomLeftLat,
                                              decimal bottomLeftLng,
                                              decimal topRightLat,
                                              decimal topRightLng) {

            IList<Polygon> shapeList = new List<Polygon>();
            try {
                using (BoundaryDataContext dc = (BoundaryDataContext)MainFactory.getDb("Boundary", true)) {
                    var query = (from lata in dc.LATA_IN_BOUNDARY(bottomLeftLat.ToString(), bottomLeftLng.ToString(), topRightLat.ToString(), topRightLng.ToString())
                                 select new {
                                     lata = lata.LATA.ToString(),
                                     coordinates = lata.COORDINATES,
                                     centroidLat = lata.CentroidLat,
                                     centroidLng = lata.CentroidLng
                                 }).Distinct();

                    foreach (var r in query) {
                        if (r.centroidLat == null || r.centroidLng == null)
                            shapeList.Add(new Polygon(r.lata, r.coordinates));
                        else
                            shapeList.Add(new Polygon(r.lata, r.coordinates, (decimal)r.centroidLat, (decimal)r.centroidLng));
                    }
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "p-p-lata-02", se.Message + "<br>" + se.StackTrace);
            }
            return shapeList.Cast<ICoordShape>().ToList();
        }

        public IList<ICoordShape> getPolygons(LatLng bottomLeft, LatLng topRight) {
            return getPolygons(bottomLeft.Lat, bottomLeft.Lng, topRight.Lat, topRight.Lng);
        }

        public IList<ICoordShape> getPolygons() {
            if (this.shapes == null) {
                this.shapes = new List<ICoordShape>();
                try {
                    using (BoundaryDataContext dc = (BoundaryDataContext)MainFactory.getDb("Boundary", true)) {
                        var query = (from centroid in dc.GetTable<LATA_CENTROID>()
                                     join boundary in dc.GetTable<LATA_BOUNDARY_LKP>()
                                         on centroid.LATA equals boundary.LATA
                                     select new {
                                         lata = centroid.LATA.ToString(),
                                         coordinates = boundary.COORDINATES,
                                         centroidLat = centroid.CentroidLat,
                                         centroidLng = centroid.CentroidLng
                                     });

                        foreach (var r in query) {
                            if (r.centroidLat == null || r.centroidLng == null)
                                this.shapes.Add(new Polygon(r.lata, r.coordinates));
                            else
                                this.shapes.Add(new Polygon(r.lata, r.coordinates, (decimal)r.centroidLat, (decimal)r.centroidLng));
                        }
                    }
                } catch (SqlException se) {
                    MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "p-p-lata-01", se.Message + "<br>" + se.StackTrace);
                }
            }
            return (IList<ICoordShape>)this.shapes;
        }

        /// <summary>
        /// Retrieves a list of Polygons where the Lata matches the group breakout.
        /// </summary>
        /// <param name="group">The zero-index group number to retreive. Must be less than Groups</param>
        /// <returns>The list of Lata coordinates that mathces the group parameter.</returns>
        public IList<ICoordShape> getPolygons(int group) {
            if (this.shapes == null)
                this.shapes = (List<ICoordShape>)getPolygons();

            List<ICoordShape> shapeList = new List<ICoordShape>();
            List<Polygon> filteredList = (List<Polygon>)this.shapes.Where(x => Int32.Parse(x.Name) == group);
            decimal avgLat = filteredList.Average(x => x.Centroid.Lat);
            decimal avgLng = filteredList.Average(x => x.Centroid.Lng);

            foreach (Polygon polygon in filteredList) 
                shapeList.Add(new Polygon(polygon.Name, 
                                          polygon.Coordinates,
                                          (decimal)filteredList.Average(x => x.Centroid.Lat),
                                          (decimal)filteredList.Average(x => x.Centroid.Lng)));
            
            return (IList<ICoordShape>)shapeList;
        }

        /// <summary>
        /// Calculates the necessary amount of groups to break out the Latas evenly for Http Requests
        /// </summary>
        public int Groups {
            get {
                if (this.shapes == null)
                    this.shapes = (List<ICoordShape>)getPolygons();

                int min = this.shapes.Min(x => Int32.Parse(x.Name));
                int max = this.shapes.Max(x => Int32.Parse(x.Name));
                int count = this.shapes.Select(x => x.Name).Distinct().Count();

                return (max - min) / count;
            }
        }
    }
}