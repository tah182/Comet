using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

using COMET.Model.Domain;
using COMET.Model.Domain.ArcGis;
using COMET.Model.Domain.Shape;
using COMET.Model.Console.Domain.View;
using COMET.Model.Domain.View;
using COMET.Model.Business.Factory;
using COMET.Model.Business.Service;
using COMET.Model.Business.Manager;
using COMET.Server.Domain;

namespace COMET.Controllers {
    /// <summary>
    /// Controller binded to full views
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MainController : Controller {
        private ILogSvc logger = MainFactory.getLogSvc();
        
        /// <summary>
        /// The controller for the index page. Retrieves a list of states and providers and returns model to view
        /// </summary>
        /// <returns>The Index view with the model</returns>
        public ViewResult Index() {
            logger.logAction("Index Access");
            ViewBag.Message = "Planning Map";
            
            //IRegion stateThread = ReferenceFactory.get("states");
            //IRegion providerThread = ReferenceFactory.get("providers");

            PathMgr pathMgr = new PathMgr(LocationFactory.createPath(BuildingType.PATH_GIS));
            IPolygonSvc msoSvc = LocationFactory.createShape(ShapeType.MSO);
            IList<ICoordShape> cableVendors = msoSvc.getPolygons();

            Dictionary<string, string> fiberLinks = getFiberVendors(pathMgr);
            

            ViewData["fiberVendors"]    = fiberLinks;
            ViewData["msoVendors"]      = cableVendors;
            //IndexModel model = new IndexModel(stateThread.getData(), null);

            return View();
        }

        // ****************************** Errors *************************************** //
        // Calls:
        // Returns:
        [OutputCache(Duration = 60)]
        public ActionResult Errors() {
            logger.logAction("Access Error Logs");
            ViewBag.Message = "Error Logs";
            List<ApplicationErrors2> ler = new List<ApplicationErrors2>();
            using (UserDataContext dc = (UserDataContext)MainFactory.getDb("User", false)) {
                ler = (from err in dc.GetTable<ApplicationErrors2>()
                       where err.ApplicationName.ToLower().Equals("comet")
                       select err).ToList();
            }

            return View(ler);
        }

        /// <summary>
        /// Returns a List of SDP Locations
        /// </summary>
        /// <returns>JSON formatted SDP Locations. Exposed to Get</returns>
        [HttpGet]
        [OutputCache(Duration = 500)]
        public ContentResult getSDPLocations() {
            BuildingMgr buildingMgr = new BuildingMgr(LocationFactory.createBuilding(BuildingType.SDP));
            return Jsonify<IList<IBuilding>>.Serialize(buildingMgr.getBuildings());
        }

        [HttpGet]
        [OutputCache(Duration = 120)]
        public JsonResult JsonCLLIMatch(string input) {
            BuildingMgr buildingMgr = new BuildingMgr(LocationFactory.createBuilding(BuildingType.SDP));
            IList<Building> clliCodes = buildingMgr.getbuildingClli(input).Distinct().Take(20).ToList();
            return Json(clliCodes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [OutputCache(Duration = 500)]
        public ActionResult LocationSDP(int addressID) {
            BuildingMgr buildingMgr = new BuildingMgr(LocationFactory.createBuilding(BuildingType.SDP));
            IList<EntranceFacility> efList = buildingMgr.getEntranceFacility();
            EntranceFacility ef = efList.First(x => x.Address.AddressID == addressID);
            return PartialView("~/Views/Partial/_SDPInfoWindow.ascx", ef);
        }

        [HttpPost]
        [OutputCache(Duration = 500)]
        public ActionResult LocationDetails(decimal lat, decimal lng, string zip, int range, string addressConcat) {
            LocationsDataContext dc = (LocationsDataContext)MainFactory.getDb("Locations", true);
            LocDetail locationDetail = new LocDetail(lat, lng, zip, range, addressConcat, dc);

            PlanningSearch ps = new PlanningSearch();
            locationDetail.setBoundaryDetails();
            locationDetail.setClecDetails();

            ps.Lata = locationDetail.getLata();
            ps.SWC = locationDetail.getSWC();
            ps.LECVendor = locationDetail.getLecVendor();
            ps.MSO = locationDetail.getMSO();
            ps.NetBuildList = locationDetail.getClecDetails();

            dc.Dispose();
            return PartialView("~/Views/Partial/_PlanningSearchReturn.ascx", ps);
        }

        [HttpGet]
        [OutputCache(Duration = 500)]
        public JsonResult LocationCLEC(decimal lat, decimal lng, string zip, int range, string addressConcat) {
            LocationsDataContext dc = (LocationsDataContext)MainFactory.getDb("Locations", true);
            LocDetail locationDetail = new LocDetail(lat, lng, zip, range, addressConcat, dc);
            locationDetail.setClecDetails();

            List<NetworkBuilding> netBuildList = new List<NetworkBuilding>();
            netBuildList = locationDetail.getClecDetails();
            dc.Dispose();
            return Json(netBuildList, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult MSOContactList(string vendorName) {
            logger.logAction("MSO Contact Information");
            IBuildingSvc ibs = LocationFactory.createBuilding(BuildingType.SDP);
            IList<LKP_VENDOR_LOCKED> lvl = ibs.getVendorContact(vendorName);
            Dictionary<string, IList<LKP_VENDOR_LOCKED>> modal = new Dictionary<string, IList<LKP_VENDOR_LOCKED>>();
            modal.Add(vendorName, lvl);
            return PartialView("~/Views/Partial/_Contacts.ascx", modal);
        }

        [HttpPost]
        public ActionResult QuoteDetails(decimal lat, decimal lng, int range, string vendorName = null) {
            logger.logAction("QuoteDetail");
            IQuoteSvc quoteSvc = MainFactory.getQuoteSvc();

            IList<Quote> quoteList = quoteSvc.getNearQuotes(lat, lng, range);
            if (vendorName != null)
                quoteList = quoteList.Where(x => x.Vendor.ToLower().Equals(vendorName.ToLower())).ToList();
            return PartialView("~/Views/Partial/_Quotes.ascx", quoteList.OrderBy(x => x.Rank).ToList());
        }

        [HttpPost]
        public ActionResult EFInRange(decimal lat, decimal lng, int range, int ordering, bool? isHighSpeed = null, int? addressID = null) {
            logger.logAction("EF In Range");
            IBuildingSvc ibs = LocationFactory.createBuilding(BuildingType.SDP);

            IList<EntranceFacility> efList = ibs.getEFInArea(lat, lng, range, ordering, isHighSpeed, addressID);
            
            return PartialView("~/Views/Partial/_EFView.ascx", efList.OrderBy(x => x.Ranking).ToList());
        }

        [HttpPost]
        public ActionResult NNIInRange(decimal lat, decimal lng, int range, int? addressID = null) {
            logger.logAction("NNI In Range");
            IBuildingSvc ibs = LocationFactory.createBuilding(BuildingType.SDP);

            IList<NNI> nniList = ibs.getNNIInArea(lat, lng, range, addressID);

            return PartialView("~/Views/Partial/_NNIView.ascx", nniList.OrderBy(x => x.Ranking).ToList());
        }

        private Dictionary<string, string> getFiberVendors(PathMgr pathMgr) {
            Dictionary<string, string> fiberLinks = new Dictionary<string, string>();

            foreach (string vendor in pathMgr.getVendorNames()) 
                fiberLinks.Add(vendor, pathMgr.getVendorColorHex(vendor));

            return fiberLinks;
        }
    }

    class LocDetail {
        private decimal lat, lng;
        private string zip, addressConcat;
        private int range;
        private string lata = null;
        private string swc = null;
        private string lecVendor = null;
        private string postal = null;

        private List<string> mso = new List<string>();
        private List<NetworkBuilding> clecList = new List<NetworkBuilding>();

        private LocationsDataContext dc;

        public LocDetail(decimal lat, decimal lng, string zip, int range, string addressConcat, LocationsDataContext dc) {
            this.lat = lat;
            this.lng = lng;
            this.zip = zip;
            this.addressConcat = addressConcat;
            this.range = range;
            this.dc = dc;
        }

        public LocDetail(string clli, int range, LocationsDataContext dc) {
            this.range = range;

            var clliDetails = (from detail in dc.ADDRESSES_VWs
                              where detail.AddressCLLI.ToUpper().Equals(clli.ToUpper())
                              select detail).FirstOrDefault();

            this.lat = (decimal)clliDetails.Zside_Latitude;
            this.lng = (decimal)clliDetails.Zside_Longitude;
            this.addressConcat = clliDetails.Zside_Premise + " " + clliDetails.Zside_Street + "|"
                            + clliDetails.Zside_City + "|" + clliDetails.Zside_State;
            this.zip = clliDetails.Zside_PostalCode;
        }

        public string getLata() {
            return this.lata;
        }

        public string getSWC() {
            return this.swc;
        }

        public string getLecVendor() {
            return this.lecVendor;
        }

        public string getPostal() {
            return this.postal;
        }

        public string[] getMSO() {
            return this.mso.ToArray();
        }

        public List<NetworkBuilding> getClecDetails() {
            return this.clecList;
        }

        public void setBoundaryDetails() {
            try {
                var strings = (from detail in dc.POINT_DETAILS_BOUNDARY(this.lat.ToString(), this.lng.ToString(), this.zip)
                                select detail);

                foreach (var s in strings) {
                    lata = s.LATA == null ? this.lata : s.LATA.ToString();
                    swc = s.SWC == null ? this.swc : s.SWC;
                    lecVendor = s.LEC_VENDOR == null ? this.lecVendor : MainFactory.formatProvider(s.LEC_VENDOR);
                    postal = s.POSTAL_CODE == null ? this.postal : s.POSTAL_CODE;
                    if (s.MSO != null)
                        this.mso.Add(MainFactory.formatProvider(s.MSO));
                }
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Main", "Thread getBoundaryDetails", "mb-sbd-1", se.Message + "\n" + se.StackTrace);
            }
        }

        public void setClecDetails() {
            try {
                this.clecList = (from clec in dc.CLEC_MILES(this.lat, this.lng, this.range, this.addressConcat)
                                    select new NetworkBuilding {
                                        Premise = clec.ADDRESS_1.Substring(0, clec.ADDRESS_1.IndexOf(" ")),
                                        Street = clec.ADDRESS_1.Substring(clec.ADDRESS_1.IndexOf(" "), clec.ADDRESS_1.Length - clec.ADDRESS_1.IndexOf(" ")),
                                        City = clec.CITY,
                                        CLLI = clec.CLLI_CD.Length != 8 ? null : clec.CLLI_CD,
                                        LatLng = new LatLng(clec.Latitude, clec.Longitude),
                                        DistanceMiles = clec.Miles,
                                        Rank = (int)clec.RANK,
                                        State = clec.STATE_CD,
                                        Vendor = clec.XREF_PARENT_VENDOR,
                                        VendorType = clec.VENDOR_TYPE,
                                        PostalCode = clec.ZIPCODE
                                    }).ToList();
            } catch (SqlException se) {
                MainFactory.getLogSvc().logError("Main", "Thread getClecDetails", "mb-scd-1", se.Message + "\n" + se.StackTrace);
            } catch (Exception e) {
                MainFactory.getLogSvc().logError("Main", "Thread getClecDetails", "mb-scd-2", e.Message + "\n" + e.StackTrace);
            }
        }

        public bool hasExactMatch() {
            return this.clecList.Any(x => x.Rank == 0);
        }
    }
}
