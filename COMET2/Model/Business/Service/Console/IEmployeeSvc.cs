using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain.Console;

namespace COMET.Model.Business.Service.Console {
    /// <summary>
    /// Service for creating employees
    /// </summary>
    public interface IEmployeeSvc {
        /// <summary>
        /// Gets a list of all employees in the console back-end.
        /// </summary>
        /// <returns>List of all employees</returns>
        IList<IEmployee> getAllEmployees();

        /// <summary>
        /// Returns a specific employee from the console back-end.
        /// </summary>
        /// <returns>The specific employee information</returns>
        IEmployee getEmployee(int employeeID);

        /// <summary>
        /// Returns an employee, Group Manager or Director depending on the Type
        /// </summary>
        /// <returns>The specific employee information</returns>
        IEmployee getEmployee(IEmployee employee);
        
        /// <summary>
        /// Saves the employee into the system.
        /// </summary>
        /// <returns>The fully validated Employee object.</returns>
        IEmployee saveEmployee(IEmployee employee);

        /// <summary>
        /// Updates the employee's information in the system.
        /// </summary>
        /// <returns>True if the update was successful.</returns>
        bool updateEmployee(IEmployee employee);

        /// <summary>
        /// Validates if the group ID exists as a valid ID
        /// </summary>
        /// <param name="groupID">The group ID to check</param>
        /// <returns>True if the group ID exists in the repository</returns>
        bool isValidGroupId(int groupID);

        /// <summary>
        /// Validates if the director ID exists as a valid ID
        /// </summary>
        /// <param name="directorID">The director ID to check</param>
        /// <returns>True if the Director's Employee number exists in the repository</returns>
        bool isValidDirectorId(int directorID);
    }
}