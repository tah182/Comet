using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace COMET.Model.Console.Domain {
    public interface IRequest {
        /// <summary>
        /// ID of the Request
        /// </summary>
        int ID {
            get;
        }

        /// <summary>
        /// ID for the Status
        /// </summary>
        int StatusID {
            get;
        }

        /// <summary>
        /// Sets the StatusID of the Request.
        /// </summary>
        /// <returns>True if the update was successful. False if there was an issue.</returns>
        bool setStatusID( int StatusID );

        
        /// <summary>
        /// DateTime of when the status of the Request was changed to "Closed", "Rejected", or "Cancelled"
        /// </summary>
        DateTime? ClosedDate {
            get;
        }

        /// <summary>
        /// Sets the ClosedDate
        /// </summary>
        void setClosedDate(bool reopen);

        /// <summary>
        /// DateTime of when the Request was updated last
        /// </summary>
        DateTime LastUpdatedDate {
            get;
        }

        /// <summary>
        /// Sets the Last Updated Date
        /// </summary>
        void setLastUpdatedDate();

        /// <summary>
        /// Summary or Subject of the Request
        /// </summary>
        string RequestSummary {
            get;
        }
        
    }
}
