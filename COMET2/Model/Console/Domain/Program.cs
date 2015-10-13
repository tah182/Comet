using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Server.Domain;

namespace COMET.Model.Console.Domain {
    public class Program : ALookup {
        public Program() {

        }

        public Program(PROGRAM program) :
            base (program.PROGRAM_ID, program.PROGRAM_NAME) {
        }

        /// <summary>
        /// Constructor for new Programs
        /// </summary>
        /// <param name="text">The Name of the Program</param>
        public Program(string text) {
            this.Text = text;
        }
    }
}