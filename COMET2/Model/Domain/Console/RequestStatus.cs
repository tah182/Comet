using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace COMET.Model.Domain.Console {
    public class RequestStatus {
        public static readonly RequestStatus SUBMITTED   = new RequestStatus(1, "Submitted", 0);
        
        public static IEnumerable<RequestStatus> Values {
            get {
                yield return SUBMITTED;
            }
        }

        private readonly int IDENTITY;
        private readonly string IDENTITY_TEXT;
        private readonly int ORDER;

        RequestStatus(int id, string text, int order) {
            this.IDENTITY = ID;
            this.IDENTITY_TEXT = text;
            this.ORDER = order;
        }

        public int ID {
            get { return this.IDENTITY; }
        }

        public string TEXT {
            get { return this.TEXT; }
        }
    }
}
