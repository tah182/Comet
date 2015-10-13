using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Server.Domain;

namespace COMET.Model.Console.Domain {
    public class LookupSorted : ALookup{
        public LookupSorted() {

        }

        public LookupSorted(string text) {
            this.Text = text;
        }

        public LookupSorted(int id, string text, int sortOrder) :
            base (id, text) {
            this.SortOrder = sortOrder;
        }

        public LookupSorted(ELEMENT_STATUS elementStatus) :
            this(elementStatus.ELEMENT_STATUS_ID, elementStatus.ELEMENT_STATUS_TEXT, elementStatus.SORT_ORDER) {
        }

        public LookupSorted(REQUEST_STATUS requestStatus) :
            this(requestStatus.REQUEST_STATUS_ID, requestStatus.REQUEST_STATUS_TEXT, requestStatus.SORT_ORDER) {
        }

        public LookupSorted(PROJECT_STATUS projectStatus) :
            this(projectStatus.PROJECT_STATUS_ID, projectStatus.PROJECT_STATUS_TEXT, projectStatus.SORT_ORDER) {
        }
        
        public int SortOrder {
            get;
            set;
        }

        public override string ToString() {
            return this.ID + " - " + this.Text;
        }
    }
}