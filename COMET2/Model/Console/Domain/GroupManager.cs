using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using COMET.Model.Domain;

namespace COMET.Model.Console.Domain {
    public class GroupManager : AEmployee {
        private int? groupID;
        private int? directorID;
        private int? groupManagerID;

        public GroupManager(IUser user) : base(user) {
        }

        /// <summary>
        /// Creates an existing employee 
        /// </summary>
        /// <param name="employeeID">The employeeID of the employee</param>
        /// <param name="startDate">The start date of this assignment</param>
        /// <param name="endDate">The end date of this assignment</param>
        /// <param name="groupID">The Group ID to determine the name of the group</param>
        /// <param name="groupManagerID">The unique Group Manager ID</param>
        public GroupManager(IUser user, int directorID, DateTime startDate, DateTime? endDate, int groupID, int? groupManagerID, string name) : base(user, startDate, endDate){
            this.Text = name;
            this.groupID = groupID;
            this.directorID = directorID;
            this.groupManagerID = groupManagerID;
        }

        /// <summary>
        /// Accessor for retrieiving this Manager's group ID
        /// </summary>
        /// <exception cref="NullReferenceException">if the employee is not valid.</exception>
        public int GroupID {
            get {
                if (!this.isValid())
                    throw new InvalidOperationException("Employee is not valid. Please fill in completely.");
                return (int)this.groupID;
            }
            set {
                this.groupID = value;
            }
        }

        /// <summary>
        /// Accessor for retrieiving this Manager's group ID
        /// </summary>
        /// <exception cref="NullReferenceException">if the employee is not valid.</exception>
        public int DirectorID {
            get {
                if (!this.isValid())
                    throw new InvalidOperationException("Manager is not valid. Please fill in completely.");
                return (int)this.directorID;
            }
            set {
                this.directorID = value;
            }
        }

        public int GroupManagerID {
            get {
                if (!this.isValid())
                    throw new InvalidOperationException("Group Manager ID is not valid. Please fill in completely.");
                return (int)this.groupManagerID;
            }
            set {
                this.groupManagerID = value;
            }
        }

        public string Text {
            get;
            set;
        }


        /// <summary>
        /// Asssess the User, startdate and groupID is valid
        /// </summary>
        /// <returns>True if the user is valid. Overrides base</returns>
        public new bool isValid() {
            return base.isValid() && this.groupID != null && this.directorID != null && this.groupManagerID != null;
        }
    }
}
