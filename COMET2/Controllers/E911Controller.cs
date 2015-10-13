using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using COMET.Model.Business.Service;
using COMET.Model.Business.Factory;
using COMET.Model.Domain.ArcGis;
using COMET.Model.Domain;
using COMET.Model.E911;

namespace COMET.Controllers {
    public class E911Controller : Controller {
        private ILogSvc logger = MainFactory.getLogSvc();

        // GET: E911
        public ActionResult Index() {
            ViewBag.Message = "911 Planning";
            logger.logAction(ViewBag.Message);
            
            return View("../Main/E911");
        }

        public ActionResult Search() {
            throw new NotImplementedException();
        }

        
        [HttpGet]
        public ActionResult JsonCountyBoundaries(decimal bottomLeftLat, decimal bottomLeftLng,
                                               decimal topRightLat, decimal topRightLng) {
            List<CountyBoundary> counties = new List<CountyBoundary>();

            GisLayer countyBoundary = GisLayer.COUNTIES;
            string requestParameter = bottomLeftLng + "," + bottomLeftLat + "," + topRightLng + "," + topRightLat;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(countyBoundary.getQuery(requestParameter));

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.OK) {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;

                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    CountyJson rc = serializer.Deserialize<CountyJson>(json);
                    foreach (County_Features features in rc.Features) {
                        CountyBoundary boundary = new CountyBoundary(features);
                        foreach (List<decimal[]> ring in features.Geometry.Rings) {
                            List<LatLng> location_list = new List<LatLng>();
                            foreach (decimal[] latLng in ring)
                                location_list.Add(new LatLng(latLng[1], latLng[0]));
                            boundary.addShape(location_list);
                        }
                        counties.Add(boundary);
                    }
                }
            }
            ContentResult jsonResult = Jsonify<List<CountyBoundary>>.Serialize(counties);
            return jsonResult;
        }

        [HttpGet]
        public ActionResult JsonPsapBoundaries(decimal bottomLeftLat, decimal bottomLeftLng,
                                               decimal topRightLat, decimal topRightLng) {
            List<PsapBoundary> psaps = new List<PsapBoundary>();
            ValidPsaps validIds = (ValidPsaps)HttpContext.Application["ValidPsapIds"];
            if (validIds == null) 
                validIds = new ValidPsaps(HttpContext.ApplicationInstance.Context);
            else if (validIds.isExpired())
                validIds.updateValidPsapIds();

            GisLayer psapBoundary = GisLayer.PSAP_BOUNDARY;
            string requestParameter = bottomLeftLng + "," + bottomLeftLat + "," + topRightLng + "," + topRightLat;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(psapBoundary.getQuery(requestParameter));
            
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.OK) {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;

                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    PsapJson psap = serializer.Deserialize<PsapJson>(json);
                    foreach (Psap_Features features in psap.Features) {
                        if (validIds.isValid(features.Attributes.PsapId)) {
                            PsapBoundary boundary = new PsapBoundary(features);
                            foreach (List<decimal[]> ring in features.Geometry.Rings) {
                                List<LatLng> location_list = new List<LatLng>();
                                foreach (decimal[] latLng in ring) 
                                    location_list.Add(new LatLng(latLng[1], latLng[0]));
                                boundary.addShape(location_list);
                            }
                            psaps.Add(boundary);
                        }
                    }
                    ContentResult jsonResult = Jsonify<List<PsapBoundary>>.Serialize(psaps);
                    return jsonResult;
                }
            }
            return null;
        }

        [HttpGet]
        public ActionResult JsonRcBoundaries(decimal bottomLeftLat, decimal bottomLeftLng,
                                               decimal topRightLat, decimal topRightLng) {
            List<RateCenterBoundary> rateCenter = new List<RateCenterBoundary>();

            GisLayer rateCenterBoundary = GisLayer.RATE_CENTER;
            string requestParameter = bottomLeftLng + "," + bottomLeftLat + "," + topRightLng + "," + topRightLat;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rateCenterBoundary.getQuery(requestParameter));

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.OK) {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;

                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    RateCenterJson rc = serializer.Deserialize<RateCenterJson>(json);
                    foreach (RateCenter_Features features in rc.Features) {
                        RateCenterBoundary boundary = new RateCenterBoundary(features);
                        foreach (List<decimal[]> ring in features.Geometry.Rings) {
                            List<LatLng> location_list = new List<LatLng>();
                            foreach (decimal[] latLng in ring)
                                location_list.Add(new LatLng(latLng[1], latLng[0]));
                            boundary.addShape(location_list);
                        }
                        rateCenter.Add(boundary);
                    }
                    ContentResult jsonResult = Jsonify<List<RateCenterBoundary>>.Serialize(rateCenter);
                    return jsonResult;
                }
            }
            return null;
        }

        [HttpGet]
        public ActionResult JsonLataBoundaries(decimal bottomLeftLat, decimal bottomLeftLng,
                                               decimal topRightLat, decimal topRightLng) {
            List<LataBoundary> latas = new List<LataBoundary>();

            GisLayer lataBoundary = GisLayer.LATA_BOUNDARY;
            string requestParameter = bottomLeftLng + "," + bottomLeftLat + "," + topRightLng + "," + topRightLat;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(lataBoundary.getQuery(requestParameter));

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.OK) {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;

                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    LataJson rc = serializer.Deserialize<LataJson>(json);
                    foreach (Lata_Features features in rc.Features) {
                        LataBoundary boundary = new LataBoundary(features);
                        foreach (List<decimal[]> ring in features.Geometry.Rings) {
                            List<LatLng> location_list = new List<LatLng>();
                            foreach (decimal[] latLng in ring)
                                location_list.Add(new LatLng(latLng[1], latLng[0]));
                            boundary.addShape(location_list);
                        }
                        latas.Add(boundary);
                    }
                }
            }
            ContentResult jsonResult = Jsonify<List<LataBoundary>>.Serialize(latas);
            return jsonResult;
        }


        [HttpGet]
        [OutputCache(Duration = 150)]
        public ActionResult LocationDetails(decimal lat, decimal lng, string zip, bool showIcon) {
            string requestParameter = lng + "," + lat;

            MultiThread<RateCenter_Features> rcThread   = new MultiThread<RateCenter_Features>(GisLayer.RATE_CENTER, requestParameter);
            MultiThread<Psap_Features> psapThread       = new MultiThread<Psap_Features>(GisLayer.PSAP_BOUNDARY, requestParameter);
            MultiThread<County_Features> countyThread   = new MultiThread<County_Features>(GisLayer.COUNTIES, requestParameter);

            Thread rc       = new Thread(new ThreadStart(rcThread.ThreadRun));
            Thread psap     = new Thread(new ThreadStart(psapThread.ThreadRun));
            Thread county   = new Thread(new ThreadStart(countyThread.ThreadRun));
            rc.Start();
            psap.Start();
            county.Start();

            rc.Join();
            psap.Join();
            county.Join();

            Details911 viewDetails = new Details911(rcThread.getFeature(), psapThread.getFeature(), countyThread.getFeature(), zip, showIcon);
            return PartialView("~/Views/Partial/_911Details.ascx", viewDetails);
        }

        [HttpGet]
        [OutputCache(Duration=300)]
        public ActionResult ShowProject(string search) {
            List<string[]> returnString = new List<string[]>();
            string queryString = "SELECT Field, Id " +
                                    "FROM ( " +
                                    "   SELECT CONCAT('<b><u>Counties</u></b>: ', county, ', ', state) AS Field, CONCAT('COUNTY_KEY=\'\'', LPAD(CAST(fips AS CHAR(5)), 5, '0'), '\'\'') AS Id  FROM arms.psap_contact " +
                                    "   UNION " +
                                    "   SELECT CONCAT('<b><u>Rate Center</u></b>: ', rc_name, ', ', rc_state), CONCAT('ABBR=\'\'', rc_name, '\'\'+and+STATE=\'\'', rc_state, '\'\'') FROM arms.rate_center " +
                                    "   UNION " +
                                    "   SELECT CONCAT('<b><u>PSAP Agency</u></b>: ', agency), CONCAT('AGENCY=\'\'', agency, '\'\'') FROM arms.psap_contact " +
                                    "   UNION " +
                                    "   SELECT CONCAT('<b><u>Fips</u></b>: ', LPAD(CAST(fips AS CHAR(5)), 5, '0')), CONCAT('COUNTY_KEY=\'\'', LPAD(CAST(fips AS CHAR(5)), 5, '0'), '\'\'') FROM arms.psap_contact " +
                                    "   UNION " +
                                    "   SELECT CONCAT('<b><u>FCC ID</u></b>:', CAST(FCCID AS CHAR(5))), CONCAT('FCCID=\'\'', CAST(FCCID AS CHAR(5)), '\'\'') FROM arms.psap_contact " + 
                                    ") Q " +
                                    "WHERE Field LIKE '%" + search + "%'";

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ArmsMySqlServer"].ConnectionString)) {
                try {
                    MySqlCommand cmd = new MySqlCommand(queryString, conn);
                    //cmd.Parameters.Add("@search", MySqlDbType.VarChar).Value = search;
                    cmd.CommandText = queryString;
                    cmd.Connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                        returnString.Add(new string[] { getNearSearchHtml(reader["Field"].ToString(), search), reader["Id"].ToString() });
                } catch (MySqlException sqle) {
                    ILogSvc logSvc = MainFactory.getLogSvc();
                    logger.logError("E911", MethodBase.GetCurrentMethod().ToString(), sqle.ErrorCode.ToString(), "Error Querying Arms in MySql. <br>" + sqle.StackTrace + "\n" + sqle.Message);
                }
            }
            ContentResult jsonResult = Jsonify<List<string[]>>.Serialize(returnString);
            return jsonResult;
        }


        [HttpGet]
        [OutputCache(Duration = 300)]
        public ActionResult ProjectCentroid(string searchType, string searchString) {
            GisLayer layer = GisLayer.COUNTIES;
            switch (searchType.ToLower()) {
                case "fips":
                case "counties":
                    layer = GisLayer.COUNTIES;
                    break;
                case "fcc id":
                case "psap agency":
                    layer = GisLayer.PSAP_BOUNDARY;
                    break;
                case "rate center":
                    layer = GisLayer.RATE_CENTER;
                    break;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(layer.getWhereQuery(searchString));
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.OK) {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;

                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    if (layer == GisLayer.RATE_CENTER) {
                        RateCenterJson rc = serializer.Deserialize<RateCenterJson>(json);
                        if (rc.Features != null) {
                            List<RateCenterBoundary> boundaries = new List<RateCenterBoundary>();
                            foreach (RateCenter_Features features in rc.Features) {
                                RateCenterBoundary boundary = new RateCenterBoundary(features);
                                foreach (List<decimal[]> ring in features.Geometry.Rings) {
                                    List<LatLng> location_list = new List<LatLng>();
                                    foreach (decimal[] latLng in ring)
                                        location_list.Add(new LatLng(latLng[1], latLng[0]));
                                    boundary.addShape(location_list);
                                }
                                boundaries.Add(boundary);
                            }
                            return Json(boundaries, JsonRequestBehavior.AllowGet);
                        }
                    } else if (layer == GisLayer.PSAP_BOUNDARY) {
                        PsapJson rc = serializer.Deserialize<PsapJson>(json);
                        if (rc.Features != null) {
                            List<PsapBoundary> boundaries = new List<PsapBoundary>();
                            foreach (Psap_Features features in rc.Features) {
                                PsapBoundary boundary = new PsapBoundary(features);
                                foreach (List<decimal[]> ring in features.Geometry.Rings) {
                                    List<LatLng> location_list = new List<LatLng>();
                                    foreach (decimal[] latLng in ring)
                                        location_list.Add(new LatLng(latLng[1], latLng[0]));
                                    boundary.addShape(location_list);
                                }
                                boundaries.Add(boundary);
                            }
                            return Json(boundaries, JsonRequestBehavior.AllowGet);
                        }
                    } else if (layer == GisLayer.COUNTIES) {
                        CountyJson rc = serializer.Deserialize<CountyJson>(json);
                        if (rc.Features != null) {
                            List<CountyBoundary> boundaries = new List<CountyBoundary>();
                            foreach (County_Features features in rc.Features) {
                                CountyBoundary boundary = new CountyBoundary(rc.Features[0]);
                                foreach (List<decimal[]> ring in features.Geometry.Rings) {
                                    List<LatLng> location_list = new List<LatLng>();
                                    foreach (decimal[] latLng in ring)
                                        location_list.Add(new LatLng(latLng[1], latLng[0]));
                                    boundary.addShape(location_list);
                                }
                                boundaries.Add(boundary);
                            }
                            return Json(boundaries, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return null;
        }


        private class MultiThread<T> {
            T feature;
            GisLayer layer;
            string queryString;
            public MultiThread(GisLayer layer, string queryString) {
                this.layer = layer;
                this.queryString = queryString;
            }

            public void ThreadRun() {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(layer.getPointQuery(queryString));
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                    if (response.StatusCode == HttpStatusCode.OK) {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        serializer.MaxJsonLength = int.MaxValue;

                        string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        // if searching for Rate Center
                        if (typeof(T) == typeof(RateCenter_Features)) {
                            RateCenterJson rc = serializer.Deserialize<RateCenterJson>(json);
                            if (rc.Features.Length > 0)
                                feature = (T)Convert.ChangeType(rc.Features[0], typeof(T));
                        // if searching for Psap
                        } else if (typeof(T) == typeof(Psap_Features)) {
                            PsapJson psap = serializer.Deserialize<PsapJson>(json);
                            if (psap.Features.Length > 0)
                                feature = (T)Convert.ChangeType(psap.Features[0], typeof(T));
                        // if searching for county
                        } else if (typeof(T) == typeof(County_Features)) {
                            CountyJson county = serializer.Deserialize<CountyJson>(json);
                            if (county.Features.Length > 0)
                                feature = (T)Convert.ChangeType(county.Features[0], typeof(T));
                        }
                    }
                }
            }

            public T getFeature() {
                return this.feature;
            }
        }

        private string getNearSearchHtml(string text, string search) {
            string returnText = text;
            text = text.ToLower();
            search = search.ToLower();

            int start = text.IndexOf(search);
            int end = start + search.Length;

            text = returnText.Substring(start, end - start);
            returnText = returnText.Replace(text, "<mark>" + text + "</mark>");
            return returnText;
        }
    }
}