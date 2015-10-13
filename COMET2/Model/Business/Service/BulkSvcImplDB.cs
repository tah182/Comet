using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using COMET.Model.Business.Factory;
using COMET.Model.Domain;
using COMET.Server.Domain;

namespace COMET.Model.Business.Service {
    public class BulkSvcImplDB : IBulkSvc {
        public IList<ClliSearchVendor> getBulkByCLLI(string clliString, float distance, int topCount) {
            List<ClliSearchVendor> clliList = new List<ClliSearchVendor>();
            try {
                using(BulkDataContext dc = (BulkDataContext)MainFactory.getDb("Bulk", true)) {
                    clliList = (from result in dc.VENDORS_BY_CLLI(clliString, (decimal)distance, topCount)
                                select new ClliSearchVendor {
                                    EnteredClli = result.EnteredClli,
                                    EnteredStreet = result.EnteredStreet,
                                    EnteredCity = result.EnteredCity,
                                    EnteredState = result.EnteredState,
                                    EnteredLat = result.EnteredLat,
                                    EnteredLng = result.EnteredLng,
                                    LATA = result.LATA == null ? (short)0 : Int16.Parse(result.LATA),
                                    Vendor = result.Vendor,
                                    VendorType = result.VendorType,
                                    CLLI_Code = result.AddressClli,
                                    Street = result.VendorStreet,
                                    City = result.VendorCity,
                                    State = result.VendorState,
                                    Lat = result.VendorLat,
                                    Lng = result.VendorLng,
                                    Distance_miles = result.Miles
                                }).ToList();
                }
           } catch (SqlException se) {
               ILogSvc logger = MainFactory.getLogSvc();
               logger.logError("BulklSvcImplDB", MethodBase.GetCurrentMethod().ToString(), se.ErrorCode.ToString(), "Bulk CLLI Lookup Failure. <br>" + se.StackTrace + "\n" + se.Message);
           }
            return clliList;
        }
    }
}