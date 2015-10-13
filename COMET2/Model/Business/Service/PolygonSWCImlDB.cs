using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Model.Domain.ArcGis;
using COMET.Model.Domain.Shape;
using COMET.Model.Business.Factory;

using COMET.Server.Domain;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Service that retrieves serving wire centers from the GAM database
    /// </summary>
    public class PolygonSWCImlDB : IPolygonSvc {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public PolygonSWCImlDB() {

        }

        public IList<ICoordShape> getPolygons(decimal bottomLeftLat, 
                                              decimal bottomLeftLng, 
                                              decimal topRightLat, 
                                              decimal topRightLng) {
            List<ICoordShape> shapes = new List<ICoordShape>();
            try {
                using (BoundaryDataContext dc = (BoundaryDataContext) MainFactory.getDb("Boundary", true)) {
                    var query = (from swc in dc.SWC_IN_BOUNDARY(bottomLeftLat.ToString(), bottomLeftLng.ToString(), topRightLat.ToString(), topRightLng.ToString())
                                 where swc.COORDINATES.Length - swc.COORDINATES.Replace("|", "").Length > 0
                                 select new {
                                     clli = swc.SWC_CLLI,
                                     coordinates = swc.COORDINATES,
                                     centroidLat = swc.CentroidLat,
                                     centroidLng = swc.CentroidLng
                                 }).Distinct();

                    foreach(var r in query) {
                        if (r.centroidLat == null || r.centroidLng == null)
                            shapes.Add(new Polygon(r.clli, r.coordinates));
                        else
                            shapes.Add(new Polygon(r.clli, r.coordinates, (decimal)r.centroidLat, (decimal)r.centroidLng));
                    }
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "p-p-swc-02", se.Message + "<br>" + se.StackTrace);
            }
            return shapes;
        }

        public IList<ICoordShape> getPolygons(LatLng bottomLeft, LatLng topRight) {
            return getPolygons(bottomLeft.Lat, bottomLeft.Lng, topRight.Lat, topRight.Lng);
        }

        public IList<ICoordShape> getPolygons() {
            List<ICoordShape> shapes = new List<ICoordShape>();
            try {
                using (BoundaryDataContext dc = (BoundaryDataContext)MainFactory.getDb("Boundary", true)) {
                    var query = (from swc in dc.GetTable<SWC_LKP>()
                                 where swc.COORDINATES.Length - swc.COORDINATES.Replace("|", "").Length > 0
                                 select new {
                                     clli = swc.SWC_CLLI,
                                     coordinates = swc.COORDINATES,
                                     centroidLat = swc.CentroidLat,
                                     centroidLng = swc.CentroidLng
                                 }).Distinct();
                    foreach (var r in query) {
                        if (r.centroidLat == null || r.centroidLng == null)
                            shapes.Add(new Polygon(r.clli, r.coordinates));
                        else
                            shapes.Add(new Polygon(r.clli, r.coordinates, (decimal)r.centroidLat, (decimal)r.centroidLng));
                    }
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "p-p-swc-01", se.Message + "<br>" + se.StackTrace);
            }
            return shapes;
        }
    }
}