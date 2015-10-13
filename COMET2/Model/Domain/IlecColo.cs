using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class IlecColo {
        public IlecColo(string nodeName,
                        string lifeCycleStatus,
                        string facilityType,
                        string recordOwner,
                        string primaryHomingGateway,
                        string clli) {

            if (nodeName == null)
                throw new ArgumentException("NodeName cannot be null. Location: " + clli);
            if (lifeCycleStatus == null)
                throw new ArgumentException("LifeCycleStatus cannot be null. Location: " + clli);
            if (facilityType == null)
                throw new ArgumentException("FacilityType cannot be null. Location: " + clli);
            if (recordOwner == null)
                throw new ArgumentException("RecordOwner cannot be null. Location: " + clli);
            this.NodeName = nodeName;
            this.LifeCycleStatus = lifeCycleStatus;
            this.FacilityType = facilityType;
            this.RecordOwner = recordOwner;
            this.PrimaryHomingGateway = primaryHomingGateway;
            this.CLLI = clli == null ? "UNKNOWN" : clli;
        }

        public string NodeName {
            get;
            private set;
        }

        public string LifeCycleStatus {
            get;
            private set;
        }

        public string FacilityType {
            get;
            private set;
        }

        public string RecordOwner {
            get;
            private set;
        }

        public string PrimaryHomingGateway {
            get;
            private set;
        }

        public string CLLI {
            get;
            private set;
        }
    }
}