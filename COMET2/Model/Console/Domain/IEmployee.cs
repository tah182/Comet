using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using COMET.Model.Domain;
using COMET.Model.Console.Domain;

namespace COMET.Model.Console.Domain {
    public interface IEmployee {

        /// <summary>
        /// Employee's ID. Accessor only
        /// </summary>
        int ID { get; }

        /// <summary>
        /// The User Object data of the Employee
        /// </summary>
        IUser User { get; }
        
        /// <summary>
        /// Accessor only for the Start Date of this record
        /// </summary>
        DateTime StartDate { get; }

        /// <summary>
        /// Access when this record ended.
        /// </summary>
        DateTime? EndDate { get; }

        /// <summary>
        /// Sets the End date of the record.
        /// </summary>
        /// <returns>True if the update was successful. False if there was an issue.</returns>
        bool setEndDate(DateTime endDate);

        /// <summary>
        /// Sets the End date of the record to today.
        /// </summary>
        /// <returns>True if the update was successful. False if there was an issue.</returns>
        bool setEndDate();

        /// <summary>
        /// Returns if the employee has all necessary fields completed.
        /// </summary>
        /// <returns></returns>
        bool isValid();

        /// <summary>
        /// Returns is the employeeList end date is null
        /// </summary>
        /// <returns></returns>
        bool Active { get; }

        string ToString();
    }
}
