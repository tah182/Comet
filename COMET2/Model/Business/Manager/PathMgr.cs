using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Factory;
using COMET.Model.Business.Service;
using COMET.Model.Domain.ArcGis;
using COMET.Model.Domain.Shape;
using COMET.Model.Domain;

namespace COMET.Model.Business.Manager {
    public class PathMgr : IManager {
        ApplicationStore<Dictionary<string, IList<Line>>> vendorPoints;
        ApplicationStore<Dictionary<string, FiberVendorDetails>> vendorNames;
        IPathSvc svc;

        public PathMgr(IPathSvc svc) {
            this.svc = svc;

            this.vendorPoints = (ApplicationStore<Dictionary<string, IList<Line>>>)HttpContext.Current.Application["gisFiber"];
            this.vendorNames = (ApplicationStore<Dictionary<string, FiberVendorDetails>>)HttpContext.Current.Application["gisVendorNames"];
            if (vendorNames == null)
                refresh();
        }

        public List<string> getVendorNames() {
            if (this.vendorNames == null)
                return new List<string>();
            return this.vendorNames.Data.Keys.ToList();
        }

        public string getVendorColorHex(string key) {
            return this.vendorNames.Data[key].ColorHex;
        }

        public IList<Line> getPoints(string vendor, decimal bottomLeftLat, decimal bottomLeftLng, decimal topRightLat, decimal topRightLng, int zoom) {
            //double tolerance = Math.Pow(zoom, -(zoom / 5));

            IList<Line> coordinates;
            IList<Line> returnLine;
            coordinates = returnLine = new List<Line>();

            if (vendor.Equals("level3"))
                coordinates = this.svc.getPoints(vendor, bottomLeftLat, bottomLeftLng, topRightLat, topRightLng);
            else {
                if (vendorPoints != null && vendorPoints.Data != null)
                    vendorPoints.Data.TryGetValue(vendor, out coordinates);
                if (vendorPoints == null) 
                    vendorPoints = new ApplicationStore<Dictionary<string, IList<Line>>>(MainFactory.getCacheExpiration(), new Dictionary<string, IList<Line>>());
                if (coordinates == null || coordinates.Count() < 1) {
                    coordinates = this.svc.getPoints(vendor, (decimal)26.4, (decimal)-132.1, (decimal)49.4, (decimal)-59.1);
                    if (!vendorPoints.Data.ContainsKey(vendor)) {
                        vendorPoints.Data.Add(vendor, coordinates);
                        HttpContext.Current.Application["gisFiber"] = vendorPoints;
                    }
                }
            }


            if (zoom <= 12) {
                foreach (Line line in coordinates) {
                    DouglasPeuckerReduction dpr = new DouglasPeuckerReduction(line.Coordinates);
                    if (!vendor.Equals("level3")) {             // if it is a GIS vendor, filter out unnecessary coordinates
                        if (!isOutsideBounds(line, bottomLeftLat, bottomLeftLng, topRightLat, topRightLng)) {
                            var l = new Line(line.Name, dpr.reduceLine(zoom));
                            l.Color = vendorNames.Data[line.Name].ColorHex;
                            returnLine.Add(l);
                        }
                    } else {
                        var l = new Line(line.Name, dpr.reduceLine(zoom));
                        l.Color = "#f00";
                        returnLine.Add(l);
                    }
                }
            } else {
                if (!vendor.Equals("level3")) {             // if it is a GIS vendor, filter out unnecessary coordinates
                    foreach (Line line in coordinates
                                                .Where(x => 
                                                    x.Coordinates.Any(y => 
                                                        y.Lat >= bottomLeftLat && 
                                                        y.Lat <= topRightLat && 
                                                        y.Lng >= bottomLeftLng &&
                                                        y.Lng <= topRightLng))) {
                        var l = new Line(line.Name, line.Coordinates);
                        l.Color = vendorNames.Data[line.Name].ColorHex;
                        returnLine.Add(l);
                    }
                } else {
                    foreach (Line line in coordinates) {
                        var l = new Line(line.Name, line.Coordinates);
                        l.Color = "#f00";
                        returnLine.Add(l);
                    }
                }
            }
            
            return returnLine;
        }

        private bool isOutsideBounds(Line line, decimal bottomLeftLat, decimal bottomLeftLng, decimal topRightLat, decimal topRightLng) {
            int lastPoint = line.Coordinates.Count - 1;
            // if lastPoint == firstpoint, remove the last point
            while (line.Coordinates[0].Equals(line.Coordinates[lastPoint]))
                lastPoint--;
            
            if (line.Coordinates[0].Lat >= bottomLeftLat && line.Coordinates[0].Lat <= topRightLat)                         // check First Lat
                if (line.Coordinates[0].Lng > bottomLeftLng && line.Coordinates[0].Lng <= topRightLng)                      // check First Lng
                    return false;

            if  (line.Coordinates[lastPoint].Lat > bottomLeftLat && line.Coordinates[lastPoint].Lat <= topRightLat)         // check Last Lat
                if (line.Coordinates[lastPoint].Lat > bottomLeftLng && line.Coordinates[lastPoint].Lng <= topRightLng)      // check Last Lng
                    return false;

            // last and first point were outside bounds
            return true;
        }

        public void refresh() {
            if (this.svc.GetType() == typeof(PathSvcImplGIS)) {
                this.vendorNames = new ApplicationStore<Dictionary<string, FiberVendorDetails>>(MainFactory.getCacheExpiration(), this.svc.getFiberVendors());
                HttpContext.Current.Application["gisVendorNames"] = vendorNames;
            }
        }
    }
}