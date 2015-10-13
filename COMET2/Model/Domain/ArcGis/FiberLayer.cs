//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Script.Serialization;

//using COMET.Model.Business.Factory;
//using COMET.Model.Domain.Shape;

//namespace COMET.Model.Domain.ArcGis {
//    public class FiberLayer {
//        private static readonly string FIBER_LAYER_URL = "http://geo.corp.global.level3.com/ArcGIS/rest/services/network_data_3rd_party_live/MapServer/";
//        private static readonly string FIBER_LAYER_LAYERS_URL = FIBER_LAYER_URL + "layers?f=json&pretty=false";

//        private IDictionary<string, int> fiberLayers;

//        public FiberLayer() {

//        }

//        public IDictionary<string, int> getFiberVendors() {
//            this.fiberLayers = (IDictionary<string, int>)HttpRuntime.Cache["FiberLayers"];
//            if (this.fiberLayers == null) {
//                this.fiberLayers = new Dictionary<string, int>();
//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FIBER_LAYER_LAYERS_URL);
//                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
//                    if (response.StatusCode == HttpStatusCode.OK) {
//                        JObject layerObject = JsonConvert.DeserializeObject<JObject>(new StreamReader(response.GetResponseStream()).ReadToEnd());
//                        if ((JObject)layerObject.GetValue("error") != null)
//                            return null;
//                        JArray layerArray = (JArray)layerObject.GetValue("layers");
//                        JObject groupObject = (JObject)layerArray.First();
//                        JArray subLayerArray = (JArray)groupObject.GetValue("subLayers");

//                        foreach (JObject layerDetails in subLayerArray) {
//                            // exclude WOW and FiberLight
//                            if (!((string)layerDetails.GetValue("name")).Equals("WOW") && !((string)layerDetails.GetValue("name")).Equals("Fiberlight")
//                                                                                       && !((string)layerDetails.GetValue("name")).Equals("Time Warner Telecom"))
//                                fiberLayers.Add((string)layerDetails.GetValue("name"), (int)layerDetails.GetValue("id"));
//                        }

//                        HttpRuntime.Cache.Insert("FiberLayers", fiberLayers,
//                            null, MainFactory.getCacheExpiration(), System.Web.Caching.Cache.NoSlidingExpiration);
//                    }
//                }
//            }
//            return this.fiberLayers;
//        }

//        public List<String> getVendorNames() {
//            List<String> returnKeys = new List<string>();
//            IDictionary<string, int> fiberVendors = getFiberVendors();
//            if (fiberVendors == null)
//                return returnKeys;
//            IEnumerable<string> keys = getFiberVendors().Keys;
//            foreach (string key in keys)
//                returnKeys.Add(key);

//            return returnKeys;
//        }

//        public int getLayerId(string vendorName) {
//            int layer;
//            getFiberVendors().TryGetValue(vendorName, out layer);
//            return layer;
//        }

//        public List<Line> getPoints(string vendorName, decimal bottomLeftLat, decimal bottomLeftLng, decimal topRightLat, decimal topRightLng) {
//            string geometry = bottomLeftLng + "," + bottomLeftLat + "," + topRightLng + "," + topRightLat;
//            string query = FIBER_LAYER_URL + getLayerId(vendorName) + "/" + GisLayer.NETWORK_FIBER.getQueryParameters(geometry);
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(query);
//            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
//                if (response.StatusCode == HttpStatusCode.OK) {
//                    JavaScriptSerializer serializer = new JavaScriptSerializer();
//                    serializer.MaxJsonLength = int.MaxValue;

//                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
//                    FiberJson fj = serializer.Deserialize<FiberJson>(json);
//                    List<Line> lines = new List<Line>();
//                    foreach (GisFeatures features in fj.Features) {
//                        foreach (List<decimal[]> ring in features.Geometry.Paths) {
//                            List<LatLng> location_list = new List<LatLng>();
//                            foreach (decimal[] latLng in ring)
//                                location_list.Add(new LatLng(latLng[1], latLng[0]));
//                            lines.Add(new Line(vendorName, location_list));
//                        }
//                    }
//                    return lines;
//                }
//            }
//            return null;
//        }
//    }
//}