using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace COMET.Model.Domain.Console {
    /// <summary>
    /// Interface for Requests
    /// </summary>
    public interface IRequest {
        /// <summary>
        /// Retrieve the ID of the Request
        /// </summary>
        int RequestID { get; }

        RequestStatus StatusID { get; }

        IEmployee Employee { get; }

        int ProjectID { get; }

        DateTime CreateDate { get; }

        string RequestSummary { get; }
    }
}
