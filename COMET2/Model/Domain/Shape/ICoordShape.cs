using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMET.Model.Domain.Shape {
    public interface ICoordShape {
        /// <summary>
        /// Returns a list of LatLng pairs that form the shape
        /// </summary>
        /// <returns>A list of Lat/Lngs</returns>
        List<LatLng> Coordinates { get; set; }

        /// <summary>
        /// Returns the identifying name of the object
        /// </summary>
        string Name { get; }

        /// <summary>
        /// override the ToString() method.
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}
