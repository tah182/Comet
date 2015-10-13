using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;

namespace COMET.Model.Console.Domain.View {
    public class Note {
        public Note() {
            this.Date = DateTime.Now;
        }

        public Note(COMET.Server.Domain.NOTE note, IUser updatedBy, ElementView parent){ //, ElementView parent) {
            this.ID = note.NOTE_ID;
            this.Date = note.NOTE_DATE;
            this.Text = note.NOTE_TEXT;
            this.UpdatedBy = updatedBy;
            this.Parent = parent;
        }

        public ElementView Parent {
            get;
            set;
        }

        public int ID {
            get;
            set;
        }

        public DateTime Date {
            get;
            set;
        }

        public string Text {
            get;
            set;
        }

        public IUser UpdatedBy {
            get;
            set;
        }
    }
}