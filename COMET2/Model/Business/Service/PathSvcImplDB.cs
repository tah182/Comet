using COMET.Server.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using COMET.Model.Domain.ArcGis;
using COMET.Model.Domain.Shape;
using COMET.Model.Business.Service;
using COMET.Model.Business.Factory;

namespace COMET.Model.Business.Service {
    public class PathSvcImplDB : IPathSvc {
        private BoundaryDataContext dc;
        public PathSvcImplDB(BoundaryDataContext dc) {
            this.dc = dc;
        }

        public Dictionary<string, FiberVendorDetails> getFiberVendors() {
            Dictionary<string, FiberVendorDetails> vendor = new Dictionary<string, FiberVendorDetails>();
            vendor.Add("Level3", new FiberVendorDetails(1, new int[] { 255, 0, 0 }));
            vendor.Add("TWTelecom", new FiberVendorDetails(1, new int[] { 0, 0, 255 }));
            return vendor;
        }

        public IList<Line> getPoints(string vendorName,
                              decimal bottomLeftLat,
                              decimal bottomLeftLng,
                              decimal topRightLat,
                              decimal topRightLng) {
            List<Line> fiber = new List<Line>();
            try {
                using (dc) {
                    fiber = (from f in dc.L3FIBER_INBOUNDARY_REDUCE(bottomLeftLat.ToString(),
                                                                    bottomLeftLng.ToString(),
                                                                    topRightLat.ToString(),
                                                                    topRightLng.ToString())
                             select new Line(vendorName, f.Coordinates))
                             .ToList();
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("PathSvcImplDB", "getPoints", "gp-01", se.Message + "\n" + se.StackTrace);
            }
            return fiber;
        }
        
        public void close() {
            if (this.dc != null)
                dc.Dispose();
        }
    }
}