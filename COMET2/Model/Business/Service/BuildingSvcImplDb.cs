using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Server.Domain;
using COMET.Model.Business.Factory;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Service to retreieve SDP buildings
    /// </summary>
    public class BuildingSvcImplDB : IBuildingSvc {
        LocationsDataContext dc;

        public BuildingSvcImplDB(LocationsDataContext dc) {
            this.dc = dc;
        }

        public BuildingSvcImplDB() {
            dc = (LocationsDataContext)MainFactory.getDb("Locations", true);
        }

        public void close() {
            try {
                dc.Dispose();
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "i-c-01", se.Message + "<br>" + se.StackTrace);
            }
        }

        /// <summary>
        /// Retreives SDP buildings from the database
        /// </summary>
        /// <returns>A list of buildings that are Service Deliverypoints</returns>
        public List<IBuilding> getBuildings() {
            List<IBuilding> buildings = new List<IBuilding>();
            try {
                buildings = (from location in dc.GetTable<SDP_ALL>()
                             select new ServiceDeliveryPoint(
                             location.ATTRIBUTES ?? 0,
                             location.Address_ID,
                             new LatLng((decimal)location.Zside_Latitude, (decimal)location.Zside_Longitude)
                             )).Cast<IBuilding>().ToList();
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "i-gb-01", se.Message + "<br>" + se.StackTrace);
            }
            return buildings;
        }

        public IList<Building> getBuildingCllis(string search) {
            IList<Building> clliCodes = new List<Building>();
            try {
                var c = (from clli in dc.GetTable<ADDRESSES_VW>()
                             where clli.AddressCLLI != null
                                && clli.Zside_Latitude != null
                                && clli.Zside_Longitude != null
                                && clli.AddressCLLI.StartsWith(search)
                             select clli).Distinct().OrderBy(a => a.AddressCLLI).ToList();
                string clliString = "";
                foreach(var a in c) {
                    if (!a.AddressCLLI.Equals(clliString)) {
                        Building b = new Building(a.AddressId);
                        b.City = a.Zside_City;
                        b.CLLI = a.AddressCLLI;
                        b.Country = a.Zside_Country;
                        b.LatLng = new LatLng((decimal)a.Zside_Latitude, (decimal)a.Zside_Longitude);
                        b.PostalCode = a.Zside_PostalCode;
                        b.Premise = a.Zside_Premise;
                        b.State = a.Zside_State;
                        b.Street = a.Zside_Street;

                        clliCodes.Add(b);
                        clliString = a.AddressCLLI;
                    }
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "gbc-01", se.Message + "\n" + se.StackTrace);
            }
            return clliCodes;
        }

        public IList<EntranceFacility> getEntranceFacilities() {
            IList<EntranceFacility> efList = new List<EntranceFacility>();
            try {
                var query = from sdp in dc.GetTable<SDP_ALL>()
                            join assets in dc.GetTable<LOCATION_ASSET>()
                            on sdp.Address_ID equals assets.ADDRESS_ID
                            select new {
                                addressId = sdp.Address_ID,
                                latitutde = sdp.Zside_Latitude,
                                longitude = sdp.Zside_Longitude,
                                attributes = sdp.ATTRIBUTES,
                                address = new Building(sdp.Address_ID) {
                                    Premise = assets.Zside_Premise,
                                    Street = assets.Zside_Street,
                                    City = assets.Zside_City,
                                    State = assets.Zside_State,
                                    LatLng = new LatLng((decimal)assets.Zside_Latitude, (decimal)assets.Zside_Longitude),
                                    CLLI = assets.AddressCLLI
                                },
                                lata = (int)assets.LATA,
                                custIndPerc = (decimal)assets.CUST_IND_PERC,
                                custHybridIndPerc = (decimal)assets.CUST_HYBRID_IND_PERC,
                                switchIndPerc = (decimal)assets.SWITCH_IND_PERC,
                                ipNetIndPerc = (decimal)assets.IP_NET_IND_PERC,
                                netOtherIndPerc = (decimal)assets.NET_OTHER_IND_PERC,
                                noProdIndPerc = (decimal)assets.NO_PROD_IND_PERC,
                                sdpTrailNum = assets.SDP_TRAIL_NUM == null ? 0 : (int)assets.SDP_TRAIL_NUM,
                                lsefTrailNum = assets.NUM_OF_TRAIL_LSEF == null ? 0 : (int)assets.NUM_OF_TRAIL_LSEF,
                                hsefTrailNum = assets.NUM_OF_TRAIL_HSEF == null ? 0 : (int)assets.NUM_OF_TRAIL_HSEF,
                                nniTrailNum = assets.NUM_OF_NNI == null ? 0 : (int)assets.NUM_OF_NNI,
                                lsefUtil = assets.UTILIZATION_LSEF == null ? 0 : (decimal)assets.UTILIZATION_LSEF,
                                hsefUtil = assets.UTILIZATION_HSEF == null ? 0 : (decimal)assets.UTILIZATION_HSEF,
                                nniUtil = assets.UTILIZATION_NNI == null ? 0 : (decimal)assets.UTILIZATION_NNI,
                                assets.NODE_NAME,
                                assets.LIFECYCLE_STATUS,
                                assets.FACILITY_TYPE,
                                assets.RECORD_OWNER,
                                assets.PRIMARY_HOMING_GATEWAY,
                                assets.ILEC_CLLI,
                                lsefTrendSlop = assets.LSEF_TREND_SLOPE == null ? 0 : (decimal)assets.LSEF_TREND_SLOPE,
                                hsefTrendSlop = assets.HSEF_TREND_SLOPE == null ? 0 : (decimal)assets.HSEF_TREND_SLOPE,
                                nniTrendSlop = assets.NNI_TREND_SLOPE == null ? 0 : (decimal)assets.NNI_TREND_SLOPE,
                            };
                foreach (var info in query) {
                    if (info.NODE_NAME == null)
                        efList.Add(new EntranceFacility(info.address, info.lata, info.custIndPerc, info.custHybridIndPerc, info.switchIndPerc,
                                                        info.ipNetIndPerc, info.netOtherIndPerc, info.noProdIndPerc, info.sdpTrailNum, info.lsefTrailNum,
                                                        info.hsefTrailNum, info.nniTrailNum, info.lsefUtil, info.hsefUtil, info.nniUtil,
                                                        (decimal)info.lsefTrendSlop, (decimal)info.hsefTrendSlop, (decimal)info.nniTrendSlop));
                    else
                        efList.Add(new EntranceFacility(info.address, info.lata, info.custIndPerc, info.custHybridIndPerc, info.switchIndPerc,
                                                        info.ipNetIndPerc, info.netOtherIndPerc, info.noProdIndPerc, info.sdpTrailNum, info.lsefTrailNum,
                                                        info.hsefTrailNum, info.nniTrailNum, info.lsefUtil, info.hsefUtil, info.nniUtil, info.NODE_NAME,
                                                        info.LIFECYCLE_STATUS, info.FACILITY_TYPE, info.RECORD_OWNER, info.PRIMARY_HOMING_GATEWAY, info.ILEC_CLLI,
                                                        (decimal)info.lsefTrendSlop, (decimal)info.hsefTrendSlop, (decimal)info.nniTrendSlop));
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "gef-01", se.Message + "\n" + se.StackTrace);
            }
            return efList;
        }

        public IList<NNI> getNNIInArea(decimal lat, decimal lng, int range, int? addressID) {
            List<NNI> nniList = new List<NNI>();
            try {
                nniList = (from nni in dc.GeoCoding_NNI(lat, lng, range, addressID)
                           select new NNI(
                               nni.NNI_TRAIL_NAME,
                               nni.RFA_ID,
                               nni.NNI_FAC_ECCKT_VENDOR,
                               nni.NNI_FAC_ECCKT,
                               (int)nni.USED_MBPS,
                               (int)nni.TOTAL_MBPS,
                               (int)nni.RANK,
                               nni.Address_ID,
                               nni.Zside_Premise,
                               nni.Zside_Street,
                               nni.Zside_City,
                               nni.Zside_State,
                               nni.Zside_PostalCode,
                               (decimal)nni.Zside_Latitude,
                               (decimal)nni.Zside_Longitude
                               )).ToList();

            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "gnia-01", se.Message + "\n" + se.StackTrace);
            }
            return nniList;
        }

        public IList<EntranceFacility> getEFInArea(decimal lat, decimal lng, int range, int ordering, bool? isHighSpeed, int? addressID) {
            List<EntranceFacility> efList = new List<EntranceFacility>();
            try {
                efList = (from ef in dc.GeoCoding_EF(lat, lng, range, ordering, isHighSpeed, addressID)
                          select new EntranceFacility (
                                  new Building (ef.Address_ID) {
                                      Premise = ef.Premise,
                                      Street = ef.Street,
                                      City = ef.City,
                                      PostalCode = ef.PostalCode,
                                      State = ef.StateCd,
                                      LatLng = new LatLng((decimal)ef.Latitude, (decimal)ef.Longitude),
                                  },
                                  ef.LEGACY,
                                  ef.Trail_Name,
                                  ef.RFAID,
                                  ef.VENDOR,
                                  ef.ECCKT,
                                  ef.HIGH_SPEED == 1 ? true : false,
                                  ef.USED_SLOTS,
                                  ef.TOTAL_SLOTS,
                                  (int)ef.Ranking,
                                  ordering == 1 ? true : false
                              )).ToList();
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "gefir-01", se.Message + "\n" + se.StackTrace);
            }
            return efList;
        }

        public IList<SDP_TREND> getSDPTrend(int weeks) {
            IList<SDP_TREND> trendList = new List<SDP_TREND>();
            try {
                trendList = dc.SDP_TREND_PIVOT(weeks).ToList();
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "gst-01", se.Message + "\n" + se.StackTrace);
            }
            return trendList;
        }

        public IList<LKP_VENDOR_LOCKED> getVendorContact(string vendor) {
            IList<LKP_VENDOR_LOCKED> vendorList = new List<LKP_VENDOR_LOCKED>();
            try {
                vendorList = (from ven in dc.GetTable<LKP_VENDOR_LOCKED>()
                              where ven.XREF_PARENT_VENDOR.ToLower().Equals(vendor.ToLower())
                              select ven).ToList();
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "gvc-01", se.Message + "\n" + se.StackTrace);
            }
            return vendorList;
        }

        public IList<LKP_VENDOR_LOCKED> getVendorContact(int id) {
            IList<LKP_VENDOR_LOCKED> vendorList = new List<LKP_VENDOR_LOCKED>();
            try {
                vendorList = (from ven in dc.GetTable<LKP_VENDOR_LOCKED>()
                              where ven.XREF_PARENT_VENDOR.ToLower().Equals(
                                    (from names in dc.GetTable<LKP_VENDOR_LOCKED>()
                                    where names.ID == id
                                    select names.XREF_PARENT_VENDOR).FirstOrDefault()
                                )
                              select ven).ToList();
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "gvc-01", se.Message + "\n" + se.StackTrace);
            }
            return vendorList;
        }

        public bool removeVendorContact(int id) {
            try {
                var delRecord = from record in dc.GetTable<LKP_VENDOR_LOCKED>()
                                where record.ID == id
                                select record;
                foreach (var rec in delRecord)
                    dc.GetTable<LKP_VENDOR_LOCKED>().DeleteOnSubmit(rec);

                dc.SubmitChanges();
                return true;
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "rvc-01", se.Message + "\n" + se.StackTrace);
            }
            return false;
        }

        public int addVendorContact(string vendorParent, string name, string office, string mobile, string email) {
            try {
                LKP_VENDOR_LOCKED newRecord = new LKP_VENDOR_LOCKED();
                newRecord.VENDOR_NORM_NAME = vendorParent;
                newRecord.XREF_PARENT_VENDOR = vendorParent;
                newRecord.CONTACT_NAME = name;
                newRecord.CONTACT_OFFICE_PHONE = office;
                newRecord.CONTACT_MOBLE_PHONE = mobile;
                newRecord.CONTACT_EMAIL = email;

                dc.GetTable<LKP_VENDOR_LOCKED>().InsertOnSubmit(newRecord);

                dc.SubmitChanges();

                return newRecord.ID;
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "avc-01", se.Message + "\n" + se.StackTrace);
            }
            return -1;
        }
    }
}