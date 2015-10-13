using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMET.Model.Domain {
    public interface IBuilding {

        /// <summary>
        /// The Services available at the location in bit format.
        /// </summary>
        int Services { get; }

        /// <summary>
        /// The ID of the address
        /// </summary>
        int AddressID { get; }

        /// <summary>
        /// The LatLng coordinates of the address.
        /// </summary>
        LatLng LatLng { get; }
    }
}
