using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain {
    public class Activity {

        public Activity(string weekStart, string group, int distinctLogins) {
            this.WeekStart = weekStart;
            this.Group = group;
            this.DistinctLogins = distinctLogins;
        }

        public string WeekStart { 
            get; 
            private set; 
        }
            
        public string Group { 
            get; 
            private set; 
        }

        public int DistinctLogins { 
            get; 
            private set; 
        }
    }
}