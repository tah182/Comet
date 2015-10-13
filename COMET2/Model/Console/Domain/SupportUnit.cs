using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Console.Domain {
    public class SupportUnit : ALookup {

        /// <summary>
        /// Used for creating a new Support Unit to enter into database
        /// </summary>
        /// <param name="text">The text to display</param>
        public SupportUnit(string text) {
            this.Text = text;
        }

        /// <summary>
        /// Used for existing Support Units in Database
        /// </summary>
        /// <param name="id">The internal id of the support unit</param>
        /// <param name="text">The text to display</param>
        public SupportUnit(int id, string text) :
            base(id, text) {
        }
    }
}