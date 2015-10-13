using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Interface of Service for creating or getting a user
    /// </summary>
    public interface IUserSvc {
        IList<IUser> getAllUsers();
        
        /// <summary>
        /// Adds a user to the system
        /// </summary>
        /// <param name="user">The User to add to the system</param>
        /// <returns>The validated user</returns>
        IUser addUser(IUser user);

        /// <summary>
        /// Sets the user's Comet acknowledgement
        /// </summary>
        /// <param name="user">The user to change the acknowledgement in the repository</param>
        /// <returns>True if the update was successful</returns>
        bool setUserAcknowledge(IUser user);

        /// <summary>
        /// Uses the COMET.XXHR_HEIRARCHY table function to retrieve 1 down, and 4 up heirarchy
        /// </summary>
        /// <param name="user">The User to search</param>
        /// <returns>Users that match the heirarchy rules</returns>
        IList<IUser> retreiveHeirarchy(IUser user);

        IList<Activity> usagebyWeek();

        IList<string> usersInGroup(string group);
    }
}