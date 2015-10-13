using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Server.Domain;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Interface for Services that retrieve buildings
    /// </summary>
    public interface IBuildingSvc {
        /// <summary>
        /// Closes the Connections
        /// </summary>
        void close();

        /// <summary>
        /// Retrieve a list of buildings
        /// </summary>
        /// <returns>The list of buildings with addresses</returns>
        List<IBuilding> getBuildings();

        IList<Building> getBuildingCllis(string clli);

        IList<EntranceFacility> getEntranceFacilities();

        IList<NNI> getNNIInArea(decimal lat, decimal lng, int range, int? addressID);

        IList<EntranceFacility> getEFInArea(decimal lat, decimal lng, int range, int ordering, bool? isHighSpeed, int? addressID);

        IList<SDP_TREND> getSDPTrend(int weeks);

        IList<LKP_VENDOR_LOCKED> getVendorContact(string vendor);
        
        IList<LKP_VENDOR_LOCKED> getVendorContact(int id);

        bool removeVendorContact(int id);

        int addVendorContact(string vendorParent, string name, string office, string mobile, string email);
    }
}