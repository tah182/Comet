using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain.ArcGis;
using COMET.Model.Domain.Shape;

namespace COMET.Model.Business.Service {
    public interface IPathSvc {
        IList<Line> getPoints(string vendorName, 
                              decimal bottomLeftLat, 
                              decimal bottomLeftLng, 
                              decimal topRightLat, 
                              decimal topRightLng);

        Dictionary<string, FiberVendorDetails> getFiberVendors();
        
        void close();
    }
}