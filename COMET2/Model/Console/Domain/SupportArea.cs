using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Server.Domain;

namespace COMET.Model.Console.Domain {
    public class SupportArea : ALookup {
        public SupportArea() {

        }

        public SupportArea(string text) {
            this.Text = text;
        }

        public SupportArea(int id, 
                           string text, 
                           int supportUnitId, 
                           int? businessTypeId, 
                           int? supportId,
                           int? developerId) :
            base (id, text) {
            this.SupportUnitID = supportUnitId;
            this.BusinessTypeID = businessTypeId;
            this.SupportID = supportId;
            this.DeveloperID = developerId;
        }

        public SupportArea(SUPPORT_AREA sa) :
            base(sa.SUPPORT_AREA_ID, sa.SUPPORT_AREA_TEXT) {

            this.SupportUnitID = sa.SUPPORT_UNIT_ID;
            this.BusinessTypeID = sa.BUSINESS_TYPE_ID;
            this.SupportID = sa.SUPPORT_ID;
            this.DeveloperID = sa.DEVELOPER_ID;
        }
        
        public int? SupportUnitID {
            get;
            private set;
        }

        public void setSupportUnitID(int supportUnitID) {
            this.SupportUnitID = supportUnitID;
        }

        public int? BusinessTypeID {
            get;
            private set;
        }

        public void setBusinessTypeID(int businessTypeID) {
            this.BusinessTypeID = businessTypeID;
        }

        public int? SupportID {
            get;
            private set;
        }

        public void setSupportID(int supportID) {
            this.SupportID = supportID;
        }

        public int? DeveloperID {
            get;
            private set;
        }

        public void setDeveloperID(int developerID) {
            this.DeveloperID = developerID;
        }
    }
}