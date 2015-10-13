using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Server.Domain;
using COMET.Model.Console.Domain;
using COMET.Model.Domain;

namespace COMET.Model.Console.Domain.View {
    public class ElementView : ARequestView {
        private decimal percentComplete, hours;

        public ElementView() {
            this.PercentComplete = 0;
            this.Hours = 0;
            this.Note = new List<Note>();
            this.LastUpdated = DateTime.Now;
        }
        
        public ElementView(RequestView parent) {
            this.OpenDate = DateTime.Today;
            this.PercentComplete = 0;
            this.Hours = 0;
            this.Note = new List<Note>();
            this.Parent = parent;
            this.LastUpdated = DateTime.Now;
        }

        public ElementView(ELEMENT element, IList<Note> noteList, IUser assignedTo, LookupSorted elementStatus, RequestView parent) 
            : base (element.ELEMENT_ID, elementStatus, element.ASSIGNED_DATE, element.CLOSED_DATE, element.LAST_UPDATED_DATE, element.ELEMENT_SUMMARY) {
            
            this.AssignedTo = (User)assignedTo;
            this.Parent = parent;
            this.percentComplete = element.PERCENT_COMPLETE;
            this.hours = element.HOURS;
            this.Resolution = element.RESOLUTION;
            this.Note = noteList == null ? new List<Note>() : noteList;
        }

        public RequestView Parent {
            get;
            set;
        }

        public User AssignedTo {
            get;
            set;
        }

        public decimal PercentComplete {
            get { return this.percentComplete; }
            set {
                if (value > 100 || value < 0)
                    throw new ArgumentException("Value must be between 0 and 1.");
                this.percentComplete = value;
            }
        }

        public decimal Hours {
            get { return this.hours; }
            set {
                if (value < this.hours)
                    throw new ArgumentException("Cannot subtract hours from what you've already worked.");
                this.hours = value;
            }
        }

        public string Resolution {
            get;
            set;
        }

        public IList<Note> Note {
            get;
            set;
        }

        public void addNote(Note note) {
            this.Note.Add(note);
        }
        
        public bool isValid() {
            if (!this.isNew)
                return true;

            if (this.Status.Text.ToLower().Equals("completed") || this.Status.Text.ToLower().Equals("cancelled")) {
                if (this.Resolution == null || this.Resolution.Length < 5)
                    throw new ArgumentNullException("Resolution", "You are marking the element as closed. Please provide a decent resolution");
            }

            if (this.Summary == null || this.Summary.Length < 1)
                throw new ArgumentException("Summary length is too short or incomplete");

            return true;
        }
    }
}