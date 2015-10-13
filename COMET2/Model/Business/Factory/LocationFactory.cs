using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Service;
using COMET.Model.Domain;
using COMET.Model.Domain.Shape;
using COMET.Server.Domain;

namespace COMET.Model.Business.Factory {
    /// <summary>
    /// Factory for creating network locations
    /// </summary>
    public static class LocationFactory {
        /// <summary>
        /// Returns a Service to retrieve building with network connectivity
        /// </summary>
        /// <param name="type">The type of buildings to create</param>
        /// <returns>A service to create network buildings</returns>
        public static IBuildingSvc createBuilding(BuildingType type) {
            switch (type) {
                case BuildingType.SDP :
                    return new BuildingSvcImplDB();
                default :
                    return new BuildingSvcImplDB();
            }
        }

        /// <summary>
        /// Returns a service to retrieve boundaries or lines related to network areas.
        /// </summary>
        /// <param name="type">The type of shape to create</param>
        /// <returns>A service to retrieve these lines</returns>
        public static IPolygonSvc createShape(ShapeType type) {
            switch (type) {
                case ShapeType.SWC :
                    return new PolygonSWCImlDB();
                case ShapeType.LATA :
                    return new PolygonLATAImplDB();
                case ShapeType.MSO:
                    return new PolygonMSOImplDB();
                default:
                    return new PolygonSWCImlDB();
            }
        }

        public static IPathSvc createPath(BuildingType type) {
            switch (type) {
                case BuildingType.PATH_DB :
                    return new PathSvcImplDB((BoundaryDataContext)MainFactory.getDb("Boundary", true));
                case BuildingType.PATH_GIS :
                    return new PathSvcImplGIS();
                default:
                    return null;
            }
        }
    }
}