using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Net;
using System.Web;

using COMET.Model.Business.Factory;
using COMET.Model.Domain;
using COMET.Model.Domain.ArcGis;
using COMET.Model.Domain.Shape;

namespace COMET.Model.Business.Service {
    public class PathSvcImplGIS : IPathSvc {
        private static readonly string FIBER_LAYER_URL = "http://geo.corp.global.level3.com/ArcGIS/rest/services/network_data_3rd_party_live/MapServer/";
        private static readonly string FIBER_LAYER_LAYERS_URL = FIBER_LAYER_URL + "layers?f=json&pretty=false";

        public Dictionary<string, FiberVendorDetails> getFiberVendors() {
            return getFiberVendors(0);
        }

        //private Dictionary<string, int> getFiberVendors(int trial) {
        //    if (trial > 5)
        //        return null;

        //    Dictionary<string, int> fiberLayers = new Dictionary<string, int>();
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FIBER_LAYER_LAYERS_URL);
        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
        //        if (response.StatusCode == HttpStatusCode.OK) {
        //            JObject layerObject = JsonConvert.DeserializeObject<JObject>(new StreamReader(response.GetResponseStream()).ReadToEnd());
        //            if ((JObject)layerObject.GetValue("error") != null)
        //                return getFiberVendors(++trial);
        //            JArray layerArray = (JArray)layerObject.GetValue("layers");
        //            JObject groupObject = (JObject)layerArray.First();
        //            JArray subLayerArray = (JArray)groupObject.GetValue("subLayers");

        //            foreach (JObject layerDetails in subLayerArray) {
        //                // exclude WOW and FiberLight
        //                if (!((string)layerDetails.GetValue("name")).Equals("WOW")
        //                    && !((string)layerDetails.GetValue("name")).Equals("Fiberlight")
        //                    && !((string)layerDetails.GetValue("name")).Equals("Time Warner Telecom"))
        //                    fiberLayers.Add((string)layerDetails.GetValue("name"), (int)layerDetails.GetValue("id"));
        //            }
        //        }
        //    }
        //    return fiberLayers;
        //}

        private Dictionary<string, FiberVendorDetails> getFiberVendors(int trial) {
            if (trial > 5)
                return null;

            Dictionary<string, FiberVendorDetails> fiberLayers = new Dictionary<string, FiberVendorDetails>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FIBER_LAYER_LAYERS_URL);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.OK) {
                    JObject layerObject = JsonConvert.DeserializeObject<JObject>(new StreamReader(response.GetResponseStream()).ReadToEnd());
                    if ((JObject)layerObject.GetValue("error") != null)
                        return getFiberVendors(++trial);
                    JArray layerArray = (JArray)layerObject.GetValue("layers");
                    int index = 0;
                    foreach (JObject layerDetails in layerArray) {
                        // exclude WOW and FiberLight
                        if (!((string)layerDetails.GetValue("name")).Equals("WOW")
                            && !((string)layerDetails.GetValue("name")).Equals("Fiberlight")
                            && !((string)layerDetails.GetValue("name")).Equals("Time Warner Telecom")
                            && !((string)layerDetails.GetValue("name")).Equals("3rd Pary Metro Providers")
                            && !((string)layerDetails.GetValue("name")).Equals("3rd Party Intercity Providers")) {

                                JObject drawingInfo = (JObject)layerDetails.GetValue("drawingInfo");
                                JObject renderer = (JObject)drawingInfo.GetValue("renderer");
                                JObject symbol = (JObject)renderer.GetValue("symbol");
                            JArray color = (JArray)symbol.GetValue("color");

                            //string colors = Color.getColor(index % Color.getColors().Length);
                            index++;
                            if (!fiberLayers.ContainsKey((string)layerDetails.GetValue("name")))
                                //fiberLayers.Add((string)layerDetails.GetValue("name"), new FiberVendorDetails((int)layerDetails.GetValue("id"), MainFactory.hex2RGB(colors)));
                                fiberLayers.Add((string)layerDetails.GetValue("name"), new FiberVendorDetails((int)layerDetails.GetValue("id"), new int[] { (int)color[0], (int)color[1], (int)color[3] }));
                        }
                    }
                }
            }
            return fiberLayers;
        }
        
        private int getLayerId(string vendorName) {
            FiberVendorDetails layer;
            getFiberVendors().TryGetValue(vendorName, out layer);
            return layer.ID;
        }

        public IList<Line> getPoints(string vendorName, decimal bottomLeftLat, decimal bottomLeftLng, decimal topRightLat, decimal topRightLng) {
            string geometry = bottomLeftLng + "," + bottomLeftLat + "," + topRightLng + "," + topRightLat;
            string query = FIBER_LAYER_URL + getLayerId(vendorName) + "/" + GisLayer.NETWORK_FIBER.getQueryParameters(geometry);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(query);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                if (response.StatusCode == HttpStatusCode.OK) {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;

                    string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    FiberJson fj = serializer.Deserialize<FiberJson>(json);
                    List<Line> lines = new List<Line>();
                    if (fj.Features != null)
                    foreach (GisFeatures features in fj.Features) {
                        foreach (List<decimal[]> ring in features.Geometry.Paths) {
                            List<LatLng> location_list = new List<LatLng>();
                            foreach (decimal[] latLng in ring)
                                location_list.Add(new LatLng(latLng[1], latLng[0]));
                            lines.Add(new Line(vendorName, location_list));
                        }
                    }
                    return lines;
                }
            }
            return null;
        }

        public void close() {

        }
    }
}