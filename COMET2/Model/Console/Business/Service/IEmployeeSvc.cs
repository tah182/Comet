using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Console.Domain;

namespace COMET.Model.Console.Business.Service {
    /// <summary>
    /// Service for creating employees
    /// </summary>
    public interface IEmployeeSvc {
        /// <summary>
        /// Gets a list of all employees in the console back-end.
        /// </summary>
        /// <returns>List of all employees</returns>
        IList<Employee> getAllEmployees();
        
        /// <summary>
        /// Gets a list of all group managers in the console back-end.
        /// </summary>
        /// <returns>List of all group managers</returns>
        IList<GroupManager> getAllGroupManagers();
        
        /// <summary>
        /// Gets a list of all directors in the console back-end.
        /// </summary>
        /// <returns>List of all employees</returns>
        IList<Director> getAllDirectors();
        
        /// <summary>
        /// Saves the employee into the system.
        /// </summary>
        /// <param name="employee">The employee object to save to system.</param>
        /// <returns>True if the save was successful.</returns>
        Employee saveEmployee(Employee employee);

        /// <summary>
        /// Updates the employee's information in the system.
        /// </summary>
        /// <returns>True if the update was successful.</returns>
        bool updateEmployee(IEmployee employee);

        /// <summary>
        /// Saves the Group into the system.
        /// </summary>
        /// <param name="group">The group object to save to the system.</param>
        /// <returns>The fully validated Group object.</returns>
        ILookup saveGroup(Group group);

        /// <summary>
        /// Saves a GroupManager into the system.
        /// </summary>
        /// <param name="groupID">The ID of the group</param>
        /// <param name="managerID">The manager of the group</param>
        /// <param name="directorID">The director of the group</param>
        /// <returns>An object of type GroupManager from Database return</returns>
        GroupManager saveGroupManager(int groupID, int managerID, int directorID);

        /// <summary>
        /// Saves a Director into the system.
        /// </summary>
        /// <param name="employeeID">The employee ID of the Director</param>
        /// <returns>a Director Object from the database</returns>
        Director saveDirector(int employeeID);

        /// <summary>
        /// Returns the Text name of the group from the database
        /// </summary>
        /// <param name="groupId">The ID identifier of the group</param>
        /// <returns>The Text name of the group</returns>
        string getGroupName(int groupId);

        /// <summary>
        /// Validates if the group ID exists as a valid ID
        /// </summary>
        /// <param name="groupID">The group ID to check</param>
        /// <returns>True if the group ID exists in the repository</returns>
        //bool isValidGroupId(int groupID);

        /// <summary>
        /// Validates if the gorup Manager Start key is a valid key
        /// </summary>
        /// <param name="groupManagerID">The group manager start key to check</param>
        /// <returns>True if the gorup manager start key exists in the repository.</returns>
        bool isValidGroupManagerId(int groupManagerID);

        /// <summary>
        /// Validates if the director ID exists as a valid ID
        /// </summary>
        /// <param name="directorID">The director ID to check</param>
        /// <returns>True if the Director's Employee number exists in the repository</returns>
        bool isValidDirectorId(int directorID);
    }
}