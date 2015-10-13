using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using COMET.Server.Domain;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Log Service Implementation
    /// </summary>
    public interface ILogSvc {

        /// <summary>
        /// Logs an action that a user makes to the database
        /// </summary>
        /// <param name="action">The action taken</param>
        void logAction(string action);

        /// <summary>
        /// Logs the Error that occurred for the user
        /// </summary>
        /// <param name="pageName">The page that the error occurred on</param>
        /// <param name="stepName">The step or method that the error occurred on</param>
        /// <param name="errorCode">The error code of the error</param>
        /// <param name="details">The details of the error</param>
        ApplicationErrors2 logError(string pageName, string stepName, string errorCode, string details);
    }
}
