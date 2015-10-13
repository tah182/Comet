using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using COMET.Model.Domain.ArcGis;
using COMET.Model.Domain.Shape;

namespace COMET.Model.E911 {
    [DataContract]
    public partial class RateCenterJson : ArcJson {
        public RateCenter_Features[] Features { get; set; }
    }

    [DataContract(Name = "features")]
    public partial class RateCenter_Features {
        [DataMember(Name = "attributes")]
        public RateCenter_Attributes Attributes { get; set; }

        [DataMember(Name = "ring")]
        public Ring Geometry { get; set; }
    }

    [DataContract(Name = "attributes")]
    public partial class RateCenter_Attributes {
        public long RCID { get; set; }
        [DataMemberAttribute]
        public string RC_ID { get; set; }
        [DataMemberAttribute]
        public string Name { get; set; }
        [DataMemberAttribute]
        public string Abbr { get; set; }
        [DataMemberAttribute]
        public string State { get; set; }
        [DataMemberAttribute]
        public string OCN { get; set; }
        [DataMemberAttribute]
        public string OCName { get; set; }
        [DataMemberAttribute(Name = "SHAPE.FID")]
        public int ShapeFID { get; set; }
    }

    [DataContract]
    public partial class PsapJson : ArcJson {
        [DataMember(Name = "features")]
        public Psap_Features[] Features { get; set; }
    }

    [DataContract]
    public partial class Psap_Features {
        [DataMember]
        public Psap_Attributes Attributes { get; set; }
        [DataMember]
        public Ring Geometry { get; set; }
    }

    [DataContract(Name = "attributes")]
    public partial class Psap_Attributes {
        [DataMemberAttribute(Name = "PSAPID")]
        public int PsapId { get; set; }
        [DataMemberAttribute(Name = "FCCID")]
        public int FccId { get; set; }
        [DataMemberAttribute(Name = "COUNTYNAME")]
        public string CountyName { get; set; }
        [DataMemberAttribute(Name = "COUNTYFIPS")]
        public string CountyFips { get; set; }
        [DataMemberAttribute(Name = "AGENCY")]
        public string Agency { get; set; }
        [DataMemberAttribute(Name = "COVERAGEAR")]
        public string CoverageAr { get; set; }
        [DataMemberAttribute(Name = "PSAPCOMMEN")]
        public string PsapCommen { get; set; }
        [DataMemberAttribute(Name = "OPERATORPH")]
        public string OperatorPh { get; set; }
        [DataMemberAttribute(Name = "CONTACTPRE")]
        public string ContactPre { get; set; }
        [DataMemberAttribute(Name = "CONTACTFIR")]
        public string ContactFir { get; set; }
        [DataMemberAttribute(Name = "CONTACTTIT")]
        public string ContactTit { get; set; }
        [DataMemberAttribute(Name = "CONTACTPH")]
        public string ContactPh { get; set; }
        [DataMemberAttribute(Name = "CONTACTCOM")]
        public string ContactCom { get; set; }
        [DataMemberAttribute(Name = "MAILINGCIT")]
        public string MailingCit { get; set; }
        [DataMemberAttribute(Name = "MAILINGSTA")]
        public string MailingSta { get; set; }
        [DataMemberAttribute(Name = "MAILINGZIP")]
        public string MailingZip { get; set; }
        [DataMemberAttribute(Name = "SITEPHONE")]
        public string SitePhone { get; set; }
        [DataMemberAttribute(Name = "SIDESTREET")]
        public string SiteStreet { get; set; }
        [DataMemberAttribute(Name = "SITESTATE")]
        public string SiteState { get; set; }
        [DataMemberAttribute(Name = "SITEZIP")]
        public string SiteZip { get; set; }
        [DataMemberAttribute(Name = "OBJECTID")]
        public int ObjectID { get; set; }
    }

    [DataContract]
    public partial class CountyJson : ArcJson {
        public County_Features[] Features { get; set; }
    }

    [DataContract]
    public partial class County_Features {
        [DataMember]
        public County_Attributes Attributes { get; set; }
        [DataMember]
        public Ring Geometry { get; set; }
    }

    [DataContract(Name = "attributes")]
    public partial class Lata_Attributes {
        [DataMemberAttribute(Name = "LATA")]
        public int Lata { get; set; }
        [DataMemberAttribute(Name = "SHAPE.FID")]
        public int ShapeFID { get; set; }
    }

    [DataContract]
    public partial class LataJson : ArcJson {
        public Lata_Features[] Features { get; set; }
    }

    [DataContract]
    public partial class Lata_Features {
        [DataMember]
        public Lata_Attributes Attributes { get; set; }
        [DataMember]
        public Ring Geometry { get; set; }
    }

    [DataContract(Name = "attributes")]
    public partial class County_Attributes {
        [DataMemberAttribute(Name = "OBJECTID")]
        public int ObjectID { get; set; }
        [DataMemberAttribute(Name = "NAME")]
        public string Name { get; set; }
        [DataMemberAttribute(Name = "COUNTY_KEY")]
        public string County_Key { get; set; }
    }

    public class JsonShapesOnly {
        public List<Polygon> Ring { get; set; }
        public int ObjectID { get; set; }
    }
}