using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain.ArcGis {
    public class GisLayer {
        public static readonly GisLayer RATE_CENTER     = new GisLayer(0, Type.TELECOM_BOUNDARY);
        public static readonly GisLayer PSAP_BOUNDARY   = new GisLayer(1, Type.TELECOM_BOUNDARY);
        public static readonly GisLayer LATA_BOUNDARY   = new GisLayer(6, Type.TELECOM_BOUNDARY);
        public static readonly GisLayer COUNTIES        = new GisLayer(28, Type.POLITICAL_BOUNDARY);
        public static readonly GisLayer NETWORK_FIBER   = new GisLayer(1001, Type.NETWORK_3RD_PARTY);

        public static IEnumerable<GisLayer> Values {
            get {
                yield return PSAP_BOUNDARY;
                yield return RATE_CENTER;
                yield return COUNTIES;
                yield return NETWORK_FIBER;
            }
        }

        private readonly string uri = "http://geo.corp.global.level3.com/ArcGIS/rest/services/";
        private readonly int layerId;
        private readonly string additionalUri;

        GisLayer(int layerId, Type type) {
            this.layerId = layerId;
            switch (type) {
                case Type.TELECOM_BOUNDARY:
                    this.additionalUri = "telecom_boundary_data_live";
                    break;
                case Type.NETWORK:
                    this.additionalUri = "network_data_level3";
                    break;
                case Type.POLITICAL_BOUNDARY:
                    this.additionalUri = "landbase_data";
                    break;
                case Type.NETWORK_3RD_PARTY:
                    this.additionalUri = "network_data_3rd_party_live";
                    break;
                default:
                    this.additionalUri = "";
                    break;
            }
            this.additionalUri += "/MapServer/";
        }

        public string UriString {
            get { return this.uri + this.additionalUri + layerId + "/"; }
        }

        private string getEnvelope(string geometry) {
            return "&geometry=" + geometry + "&geometryType=esriGeometryEnvelope";
        }

        private string getPoint(string geometry) {
            return "&geometry=" + geometry + "&geometryType=esriGeometryPoint";
        }

        private string getOutFields() {
            string returnString;
            switch (layerId) {
                case 0:
                    returnString = "RCID,RC_ID,NAME,ABBR,STATE,OCN,OCNAME,SHAPE.FID";
                    break;
                case 1:
                    returnString = "PSAPID,FCCID,COUNTYNAME,COUNTYFIPS,AGENCY,COVERAGEAR,PSAPCOMMEN,OPERATORPH,CONTACTPRE,CONTACTFIR,CONTACTTIT,CONTACTPH," +
                                    "CONTACTCOM,MAILINGSTR,MAILINGCIT,MAILINGSTA,MAILINGZIP,SITEPHONE,SITESTREET,SITECITY,SITESTATE,SITEZIP,OBJECTID";
                    break;
                case 6:
                    returnString = "OBJECTID,LATA";
                    break;
                case 1001:
                    returnString = "CARRIER";
                    break;
                default:
                    returnString = "OBJECTID,NAME,COUNTY_KEY";
                    break;
            }
            return returnString;
        }

        public string getQuery(string geometry) {
            return UriString + getQueryParameters(geometry);
        }

        public string getPointQuery(string geometry) {
            return UriString + getPointParameters(geometry);
        }

        public string getWhereQuery(string searchCriteria) {
            return UriString + getWhereParameter(searchCriteria);
        }

        public string getQueryParameters(string geometry) {
            return "query?text=" +
                getEnvelope(geometry) +
                "&inSR=&spatialRel=" +
                "&relationParam=&objectIds=&where=1%3D1&time=&returnCountOnly=false" +
                "&returnIdsOnly=false" +
                "&returnGeometry=true" +
                "&maxAllowableOffset=&outSR=&outFields=" + getOutFields() +
                "&f=json";
        }

        public string getPointParameters(string geometry) {
            return "query?text=" +
                getPoint(geometry) +
                "&inSR=&spatialRel=" +
                "&relationParam=&objectIds=&where=1%3D1&time=&returnCountOnly=false" +
                "&returnIdsOnly=false" +
                "&returnGeometry=true" +
                "&maxAllowableOffset=&outSR=&outFields=" + getOutFields() +
                "&f=json";
        }

        public string getWhereParameter(string searchCriteria) {
            return "query?text=" +
                "&geometry=&geometryType=esriGeometryPoint" +
                "&inSR=&spatialRel=" +
                "&relationParam=&objectIds=&where=" +
                searchCriteria + "&time=&returnCountOnly=false" +
                "&returnIdsOnly=false" +
                "&returnGeometry=true" +
                "&maxAllowableOffset=&outSR=&outFields=" + getOutFields() +
                "&f=json";
        }

        private enum Type {
            TELECOM_BOUNDARY,
            POLITICAL_BOUNDARY,
            NETWORK,
            NETWORK_3RD_PARTY
        }
    }
}