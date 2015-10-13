using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Model.Console.Domain;

namespace COMET.Model.Console.Domain.View {
    public class ARequestView : IRequestView {
        private DateTime? closedDate;

        public ARequestView(int id, 
                            LookupSorted status, 
                            DateTime OpenDate,
                            DateTime? closedDate,
                            DateTime LastUpdated,
                            string summary) {
            this.ID = id;
            this.Status = status;
            this.OpenDate = OpenDate;
            this.closedDate = closedDate;
            this.LastUpdated= LastUpdated;
            this.Summary = summary;
            this.isNew = false;
        }

        public ARequestView() {
            this.OpenDate = DateTime.Today;
            this.isNew = true;
        }

        public int ID { 
            get;
            set;
        }

        public LookupSorted Status { 
            get; 
            set; 
        }

        public DateTime OpenDate { 
            get; 
            set;
        }

        public DateTime? ClosedDate {
            get {
                if (this.isNew)
                    return null;
                return this.closedDate;
            }
            set {
                if (this.isNew)
                    this.closedDate = value;
            }
        }

        public void setClosed() {
            if (this.ClosedDate != null)
                throw new ArgumentException("Attempting to close " + this.ToString() + " - " + this.Summary + ". When it is already closed.");
            this.closedDate = DateTime.Today;
        }
        
        public DateTime LastUpdated { 
            get; 
            set;
        }

        public void setLastUpdated() {
            this.LastUpdated = DateTime.Now;
        }

        public string Summary { get; set; }

        public bool isNew {
            get;
            set;
        }

        public override string ToString() {
            return "ID: " + this.ID;
        }
    }
}