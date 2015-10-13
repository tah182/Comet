using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Console.Domain {
    public abstract class ALookup : ILookup {
        protected ALookup() {

        }

        protected ALookup(int id, string text) {
            this.ID = id;
            this.Text = text;
        }

        public int ID {
            get;
            set;
        }

        public string Text {
            get;
            set;
        }
    }
}