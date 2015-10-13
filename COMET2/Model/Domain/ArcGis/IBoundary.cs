using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using COMET.Model.Domain.Shape;

namespace COMET.Model.Domain.ArcGis {
    public interface IBoundary {
        /// <summary>
        /// Accessor only for ID
        /// </summary>
        long ID { get; }

        /// <summary>
        /// Accessor only for List&lt;ICoordShape&gt;
        /// </summary>
        IList<ICoordShape> Polygon { get; }

        /// <summary>
        /// Accessor for the name of the object
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Allows to add a Shape using a list of LatLng.
        /// </summary>
        /// <param name="latLngs"></param>
        void addShape(List<LatLng> latLngs);

        /// <summary>
        /// Accessor only for the internal ID of the object.
        /// </summary>
        int ObjectID { get; }
    }
}
