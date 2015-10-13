using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using COMET.Model.Domain.Shape;
using COMET.Model.Domain;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Interface for services that retreive polygon shapes.
    /// </summary>
    public interface IPolygonSvc {
        /// <summary>
        /// Retreive all the polygons
        /// </summary>
        /// <returns>List of Shapes</returns>
        IList<ICoordShape> getPolygons();

        /// <summary>
        /// Retreive the polygons in a specific region
        /// </summary>
        /// <param name="bottomLeftLat">The latitude of the lower left point</param>
        /// <param name="bottomLeftLng">The longitude of the lower left point</param>
        /// <param name="topRightLat">The latitude of the upper right point</param>
        /// <param name="topRightLng">The longitude of the upper right point</param>
        /// <returns></returns>
        IList<ICoordShape> getPolygons(decimal bottomLeftLat, decimal bottomLeftLng, decimal topRightLat, decimal topRightLng);

        /// <summary>
        /// Retrieve the polygons in a specific region
        /// </summary>
        /// <param name="bottomLeft">The latitude/longitude combination of the lower left point</param>
        /// <param name="bottomRight">The latitude/longitude combination of the upper right point</param>
        /// <returns></returns>
        IList<ICoordShape> getPolygons(LatLng bottomLeft, LatLng bottomRight);
    }
}
