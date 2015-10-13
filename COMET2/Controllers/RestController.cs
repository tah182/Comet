using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Business.Factory;
using COMET.Model.Business.Manager;
using COMET.Model.Business.Service;
using COMET.Model.Domain;
using COMET.Model.Console.Domain.View;
using COMET.Model.Domain.Shape;

using COMET.Server.Domain;

namespace COMET.Controllers {
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class RestController : Controller {
        public ActionResult ShowNav() {
            return PartialView("~/Views/Partial/_NewVersion.ascx");
        }

        [HttpGet]
        public void MarkNewVersionAcknowledgement() {
            using (UserDataContext dudc = (UserDataContext)MainFactory.getDb("User", false)) {
                var qry = from user in dudc.GetTable<Application_Role>()
                            where user.EmployeeID == ((IUser)Session["User"]).EmployeeID
                            select user;
                var ack = qry.Single();
                ack.AckVersion = true;
                dudc.SubmitChanges();
            }
            IUser u = (IUser)Session["User"];
            u.setAcknowledged();
            Session["User"] = u;
        }

        [HttpPost]
        public JsonResult Requests() {
            IList<RequestView> requests = (List<RequestView>)HttpContext.ApplicationInstance.Application["newRequests"];
            if (requests == null)
                requests = new List<RequestView>();

            return Json(requests.Where(x => x.AssignedTo.Equals((IUser)Session["user"])));
        }

        [HttpPost]
        public void SendEmail(string stepName, string errorTime, string etlError, string sendTo, string subject) {
            subject = subject.Length <= 1 ? "Java ETL Process." : subject;
            string msgBody = "<html><body><table style=\"font-family:Arial;font-size:9pt;border-collapse:collapse;border:solid .5pt;border-color:RGB(150,150,150);padding-left:10px;padding-right:10px;\">";
            msgBody = msgBody + "<tr><td style=\"border:solid .5pt RGB(150,150,150);padding-left:10px;padding-right:10px;\" align=\"left\">Application: </td><td align=\"right\">ETL Process</td></tr>";
            msgBody = msgBody + "<tr><td style=\"border:solid .5pt RGB(150,150,150);padding-left:10px;padding-right:10px;\" align=\"left\">Step: </td><td align=\"right\">" + stepName + "</td></tr>";
            msgBody = msgBody + "<tr><td style=\"border:solid .5pt RGB(150,150,150);padding-left:10px;padding-right:10px;\" align=\"left\">Error Time: </td><td align=\"right\">" + errorTime + "</td></tr>";
            msgBody = msgBody + "</table><br><br><b>Stack Trace: </b><br>" + etlError + "<br /><br /></body></html>";

            EmailSvc.Email(sendTo, "", subject, msgBody);
        }

        [HttpPost]
        [OutputCache(Duration = 120)]
        public ActionResult EmployeeDetails(int employeeID) {
            UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
            return PartialView("~/Views/Partial/_EmployeeDetails.ascx", userMgr.getUser(employeeID));
        }

        [HttpPost]
        public JsonResult RefreshAppData() {
            List<IManager> mgrList = new List<IManager>();
            try {
                mgrList.Add(new UserMgr(MainFactory.getUserSvc()));
                mgrList.Add(new BuildingMgr(LocationFactory.createBuilding(BuildingType.SDP)));
                mgrList.Add(new EmployeeMgr(ConsoleFactory.getEmployeeSvc()));
                mgrList.Add(new LookupMgr(ConsoleFactory.getRequestSvc()));
                mgrList.Add(new RequestMgr(ConsoleFactory.getRequestSvc()));

                foreach (IManager manager in mgrList)
                    manager.refresh();

            } catch (Exception e) {
                return Json(e);
            }

            return Json("All data has been refreshed.");
        }

        [HttpPost]
        public ActionResult DeleteContact(int id) {
            IBuildingSvc bldgSvc = LocationFactory.createBuilding(BuildingType.SDP);
            bldgSvc.removeVendorContact(id);

            return getContactPartial(id);
        }

        [HttpPost]
        public ActionResult AddContact(string company, string name, string office, string mobile, string email) {
            IBuildingSvc bldgSvc = LocationFactory.createBuilding(BuildingType.SDP);
            int id = bldgSvc.addVendorContact(company, name, office, mobile, email);

            return getContactPartial(id);
        }

        [HttpGet]
        [OutputCache(Duration = 500)]
        public JsonResult SDPTrend(int addressID, int type) {
            BuildingMgr buildingMgr = new BuildingMgr(LocationFactory.createBuilding(BuildingType.SDP));
            return Json(buildingMgr.getSDPTrend(addressID, type), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FiberPoints(string vendorName, 
                                        decimal bottomLeftLat, decimal bottomLeftLng, 
                                        decimal topRightLat, decimal topRightLng, 
                                        int zoom, 
                                        bool pretty = false) {
            MainFactory.getLogSvc().logAction("Vendor Fiber " + vendorName);

            PathMgr pathMgr = new PathMgr(LocationFactory.createPath(vendorName.Equals("level3") ? BuildingType.PATH_DB : BuildingType.PATH_GIS));
            IList<Line> lineList = pathMgr.getPoints(vendorName, bottomLeftLat, bottomLeftLng, topRightLat, topRightLng, zoom);

            ContentResult jsonResult = Jsonify<IList<Line>>.Serialize(lineList, pretty);

            return jsonResult;
        }
                
        [HttpGet]
        public ActionResult JsonSwc(decimal? bottomLeftLat, decimal? bottomLeftLng,
                                    decimal? topRightLat, decimal? topRightLng,
                                    int zoom = 14,
                                    bool pretty = false) {
            MainFactory.getLogSvc().logAction("Swc Boundary");
            IPolygonSvc swcSvc = LocationFactory.createShape(ShapeType.SWC);

            IList<ICoordShape> sm = getPolygons(swcSvc, bottomLeftLat, bottomLeftLng, topRightLat, topRightLng, zoom);

            ContentResult jsonResult = Jsonify<IList<ICoordShape>>.Serialize(sm, pretty);

            return jsonResult;
        }

        [HttpGet]
        [OutputCache(Duration = 500)]
        public ActionResult JsonLata(decimal? bottomLeftLat, decimal? bottomLeftLng,
                                     decimal? topRightLat, decimal? topRightLng,
                                     int zoom = 14,
                                     bool pretty = false) {
            MainFactory.getLogSvc().logAction("Lata Boundary");
            IPolygonSvc lataSvc = LocationFactory.createShape(ShapeType.LATA);

            zoom = zoom >= 8 ? 11 : zoom;
            IList<ICoordShape> lata = getPolygons(lataSvc, bottomLeftLat, bottomLeftLng, topRightLat, topRightLng, zoom);

            ContentResult jsonResult = Jsonify<IList<ICoordShape>>.Serialize(lata, pretty);

            return jsonResult;
        }

        [HttpGet]
        [OutputCache(Duration = 500)]
        public ActionResult JsonMso(decimal? bottomLeftLat, decimal? bottomLeftLng,
                                     decimal? topRightLat, decimal? topRightLng,
                                     int zoom = 14,
                                     bool pretty = false) {
            MainFactory.getLogSvc().logAction("MSO Boundary");
            IPolygonSvc msoSvc = LocationFactory.createShape(ShapeType.MSO);

            zoom = zoom >= 8 ? 11 : zoom;
            IList<ICoordShape> mso = getPolygons(msoSvc, bottomLeftLat, bottomLeftLng, topRightLat, topRightLng, zoom);

            ContentResult jsonResult = Jsonify<IList<ICoordShape>>.Serialize(mso, pretty);

            return jsonResult;
        }

        private ActionResult getContactPartial(int id) {
            IBuildingSvc bldgSvc = LocationFactory.createBuilding(BuildingType.SDP);
            IList<LKP_VENDOR_LOCKED> lvl = bldgSvc.getVendorContact(id);
            Dictionary<string, IList<LKP_VENDOR_LOCKED>> modal = new Dictionary<string, IList<LKP_VENDOR_LOCKED>>();
            modal.Add(lvl[0].XREF_PARENT_VENDOR, lvl);
            return PartialView("~/Views/Partial/_Contacts.ascx", modal);
        }

        private IList<ICoordShape> getPolygons(IPolygonSvc svc, 
                                               decimal? bottomLeftLat, decimal? bottomLeftLng,
                                               decimal? topRightLat, decimal? topRightLng, 
                                               int zoom) {
            IList<ICoordShape> shapeList = null;
            //double tolerance = Math.Pow(zoom, -(zoom / 5));
            if (bottomLeftLat != null && bottomLeftLng != null && topRightLat != null && topRightLng != null)
                shapeList = svc.getPolygons(new LatLng((decimal)bottomLeftLat, (decimal)bottomLeftLng),
                                        new LatLng((decimal)topRightLat, (decimal)topRightLng));
            else
                shapeList = svc.getPolygons();

            if (zoom <= 9) {
                foreach (ICoordShape shape in shapeList) {
                    DouglasPeuckerReduction dpr = new DouglasPeuckerReduction(shape.Coordinates);
                    shape.Coordinates = dpr.reduceLine(zoom, true);
                }
            }

            return shapeList;
        }
    }
}
