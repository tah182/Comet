using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Console.Domain {
    public class Group : ILookup {
        /// <summary>
        /// Used to create a new Group
        /// </summary>
        /// <param name="text">The name of the group.</param>
        public Group(string text) {
            this.Text = text;
        }

        /// <summary>
        /// Used to create an existing Group from the repository.
        /// </summary>
        /// <param name="id">The internal ID of the group</param>
        /// <param name="text">The text to display</param>
        public Group(int id, string text) {
            this.ID = id;
            this.Text = text;
        }

        public int ID {
            get;
            private set;
        }

        public string Text {
            get;
            private set;
        }
    }
}