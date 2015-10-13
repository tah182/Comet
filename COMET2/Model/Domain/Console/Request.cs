using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace COMET.Model.Domain.Console {
    public class Request : IRequest {

        public Request(int requestID) {
            //UserMgr userMgr = new UserMgr();
            //this.employee = userMgr.getUser(employeeID);
        }

        public int RequestID {
            get;
            private set;
        }

        public RequestStatus StatusID {
            get { throw new System.NotImplementedException(); }
        }

        public IEmployee Employee {
            get { throw new System.NotImplementedException(); }
        }

        public int ProjectID {
            get { throw new System.NotImplementedException(); }
        }

        public DateTime CreateDate {
            get { throw new System.NotImplementedException(); }
        }

        public string RequestSummary {
            get { throw new System.NotImplementedException(); }
        }
    }
}
