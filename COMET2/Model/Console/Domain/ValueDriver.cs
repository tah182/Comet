using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Console.Domain {
    public class ValueDriver : ALookup {
        public ValueDriver() {

        }

        public ValueDriver(string text) {
            this.Text = text;
        }

        public ValueDriver(int id, string text, string comment) :
            base(id, text){
                this.Comment = comment;
        }

        public string Comment {
            get;
            set;
        }
    }
}