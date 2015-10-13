using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Console.Domain {
    public class LookupActive : ALookup {
        public LookupActive() {

        }

        public LookupActive(string text) {
            this.Text = text;
        }

        public LookupActive(int id, string text, bool active, string comment) :
            base(id, text) {
            this.Active = active;
            this.Comment = comment;
        }

        public LookupActive(Server.Domain.REQUEST_CATEGORY requestCategory) :
            this(requestCategory.REQUEST_CATEGORY_ID, requestCategory.REQUEST_CATEGORY_TEXT, requestCategory.ACTIVE, requestCategory.COMMENT) {
        }

        public LookupActive(Server.Domain.REQUEST_TYPE requestType) :
            this(requestType.REQUEST_TYPE_ID, requestType.REQUEST_TYPE_TEXT, requestType.ACTIVE, requestType.COMMENT) {
        }

        public LookupActive(Server.Domain.PROJECT_TYPE projectType) :
            this(projectType.PROJECT_TYPE_ID, projectType.PROJECT_TYPE_TEXT, projectType.ACTIVE, projectType.COMMENT) {
        }

        public bool Active {
            get;
            set;
        }

        public string Comment {
            get;
            set;
        }

        public int Value {
            get { return this.ID; }
        }

        public bool Enabled {
            get { return this.Active; }
        }

        public override string ToString() {
            return this.ID + " - " + this.Text;
        }
    }
}