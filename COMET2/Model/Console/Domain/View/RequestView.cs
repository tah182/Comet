using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Server.Domain;
using COMET.Model.Domain;
using COMET.Model.Console.Domain;

namespace COMET.Model.Console.Domain.View {
    public class RequestView : AProjectView {
        public RequestView() {
            
        }

        public RequestView(REQUEST request, 
                            User submittedBy,
                            User requestedBy,
                            User assignedTo,
                            IList<ElementView> elements,
                            LookupActive requestCategory,
                            SupportArea supportArea,
                            ProjectView project,
                            Program program,
                            ValueDriver valueDriver,
                            LookupSorted requestStatus,
                            LookupActive requestType,
                            decimal? internalHoursMultiplier,
                            decimal? externalHoursMultiplier) 
            : base(request, 
                    submittedBy, 
                    requestedBy,
                    supportArea,
                    program,
                    valueDriver,
                    requestStatus,
                    requestType,
                    internalHoursMultiplier,
                    externalHoursMultiplier,
                    assignedTo) {

            this.RequestCategory = requestCategory;
            this.Parent = project;
            this.ElementList = elements == null ? new List<ElementView>() : elements;
            this.Resolution = request.RESOLUTION;
            this.ITFeatures = request.IT_FEATURE;
            this.TopOffnetAttributeNumber = request.TOP_OFFNET_ATTRIBUTE_NUMBER;
        }

        public ProjectView Parent {
            get;
            set;
        }
       
        public LookupActive RequestCategory {
            get;
            set;
        }
        
        public decimal PercentComplete {
            get {
                if (this.Hours == 0)
                    return 0;
                decimal totalNeeded = this.ElementList.Sum(x => x.PercentComplete == 0 ? 0 : x.Hours / x.PercentComplete);
                return totalNeeded == 0 ? 0 : this.ElementList.Sum(x => x.Hours) / totalNeeded;
            }
        }

        public decimal Hours {
            get {
                if (this.ElementList == null)
                    return 0;
                return this.ElementList.Sum(x => x.Hours);
            }
        }

        public IList<ElementView> ElementList {
            get;
            set;
        }

        public string Resolution {
            get;
            set;
        }

        public string ITFeatures {
            get;
            set;
        }

        public string TopOffnetAttributeNumber {
            get;
            set;
        }

        public override bool Equals(object rv) {
            if (rv.GetType() != typeof(RequestView))
                return false;

            return this.ID == ((RequestView)rv).ID;
        }

        public override int GetHashCode() {
            return this.ID.GetHashCode();
        }
    }
}