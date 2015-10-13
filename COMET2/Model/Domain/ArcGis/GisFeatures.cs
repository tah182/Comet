using System.Collections.Generic;
using System.Runtime.Serialization;

namespace COMET.Model.Domain.ArcGis {
    [DataContract]
    public class ArcJson {
        [DataMember(Name = "displayFieldName")]
        public string DisplayFieldName { get; set; }

        [DataMember(Name = "fieldAliases")]
        public IDictionary<string, string> FieldAliases { get; set; }

        [DataMember(Name = "geometryType")]
        public string GeometryType { get; set; }

        [DataMember(Name = "spatialReference")]
        public SpatialReference SpatialReference { get; set; }

        [DataMember(Name = "fields")]
        public Fields[] Fields { get; set; }
    }

    public class SpatialReference {
        [DataMember(Name = "wkid")]
        public int WkID { get; set; }
    }

    public class Fields {
        [DataMemberAttribute]
        public string name { get; set; }

        [DataMemberAttribute]
        public string type { get; set; }

        [DataMemberAttribute]
        public string alias { get; set; }

        [DataMemberAttribute]
        public int length { get; set; }
    }

    [DataContract(Name = "geometry")]
    public partial class Ring {
        [DataMemberAttribute(Name = "rings")]
        public List<List<decimal[]>> Rings { get; set; }
    }

    [DataContract(Name = "geometry")]
    public partial class LinePath {
        [DataMemberAttribute(Name = "paths")]
        public List<List<decimal[]>> Paths { get; set; }
    }

    [DataContract]
    public partial class FiberJson : ArcJson {
        [DataMember(Name = "features")]
        public GisFeatures[] Features { get; set; }
    }

    [DataContract]
    public partial class GisFeatures {
        [DataMember]
        public Fiber_Attributes Attributes { get; set; }
        [DataMember]
        public LinePath Geometry { get; set; }
    }

    [DataContract(Name = "attributes")]
    public partial class Fiber_Attributes {
        [DataMemberAttribute(Name = "CARRIER")]
        public string Carrier { get; set; }
    }
}