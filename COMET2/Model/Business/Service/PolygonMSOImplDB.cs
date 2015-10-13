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
    public class PolygonMSOImplDB : IPolygonSvc {
        private IList<ICoordShape> shapes;
        private Random rand;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public PolygonMSOImplDB() {
            shapes = (List<ICoordShape>)HttpRuntime.Cache["MsoBoundary"];
            rand = new Random(12);
        }

        public IList<ICoordShape> getPolygons(decimal bottomLeftLat,
                                              decimal bottomLeftLng,
                                              decimal topRightLat,
                                              decimal topRightLng) {

            IList<Polygon> shapeList = new List<Polygon>();
            try {
                using (BoundaryDataContext dc = (BoundaryDataContext)MainFactory.getDb("Boundary", true)) {
                    var query = (from mso in dc.MSO_IN_BOUNDARY(bottomLeftLat.ToString(), bottomLeftLng.ToString(), topRightLat.ToString(), topRightLng.ToString())
                                 select new {
                                     mso = mso.Xref_Parent_Vendor,
                                     coordinates = mso.BOUNDARY_LATLNG,
                                     centroidLat = mso.CentroidLat,
                                     centroidLng = mso.CentroidLng
                                 }).Distinct();

                    List<Polygon> vendors = getPolygons().Cast<Polygon>().ToList();
                    foreach (var r in query) {
                        string color = ColorFactory.getColor(vendors.FindIndex(x => x.Name.ToLower() == r.mso.ToLower()));
                        if (r.centroidLat == null || r.centroidLng == null)
                            shapeList.Add(new Polygon(r.mso, r.coordinates, color));
                        else
                            shapeList.Add(new Polygon(r.mso, r.coordinates, (decimal)r.centroidLat, (decimal)r.centroidLng, color));
                    }
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "p-p-mso-02", se.Message + "<br>" + se.StackTrace);
            }
            return shapeList.Cast<ICoordShape>().ToList();
        }

        public IList<ICoordShape> getPolygons(LatLng bottomLeft, LatLng topRight) {
            return getPolygons(bottomLeft.Lat, bottomLeft.Lng, topRight.Lat, topRight.Lng);
        }

        public IList<ICoordShape> getPolygons() {
            IList<Polygon> shapeList = new List<Polygon>();
            
            List<LatLng> bunk = new List<LatLng>();
            bunk.Add(new LatLng(1, 1));
            bunk.Add(new LatLng(2, 2));
            bunk.Add(new LatLng(3, 3));

            List<string> vendors = getVendors();
            int index = 0;
            foreach (var vendor in vendors) {
                string color = ColorFactory.getLightColor(index);
                shapeList.Add(new Polygon(vendor, bunk, color));
                index++;
            }
            return shapeList.Cast<ICoordShape>().ToList();
        }

        private List<string> getVendors() {
            try {
                using (ReferenceDataContext dc = (ReferenceDataContext)MainFactory.getDb("Reference", true)) {
                    var r = (from vendor in dc.GetTable<CABLE_OPERATOR_AREAS_FCC_EXCEL>()
                             join lkp in dc.GetTable<LKP_VENDOR_TYP_XREF_EXCEL>()
                             on vendor.Legal_Name equals lkp.UPPER_VENDOR_NAME
                             where lkp.VENDOR_TYPE.Equals("MSO")
                                 && vendor.CUID_Status.Equals("Active")
                             select lkp.XREF_PARENT_VENDOR).Distinct().OrderBy(x => x);

                    return r.Select(x => MainFactory.formatProvider(x)).ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "p-p-mso-01", se.Message + "<br>" + se.StackTrace);
            }
            return new List<string>();
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