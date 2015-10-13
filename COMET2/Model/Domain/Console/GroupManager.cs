using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using COMET.Model.Business.Factory;

namespace COMET.Model.Domain.Console {
    public class GroupManager : AEmployee {
        private int? groupID;
        private int? directorID;

        public GroupManager(int managerID) : base(managerID) {
        }

        /// <summary>
        /// Creates an existing employee 
        /// </summary>
        /// <param name="employeeID">The employeeID of the employee</param>
        /// <param name="startDate">The start date of this assignment</param>
        /// <param name="endDate">The end date of this assignment</param>
        public GroupManager(int managerID, int directorID, DateTime startDate, DateTime? endDate, int groupID) : base(managerID, startDate, endDate){
            this.groupID = groupID;
            this.directorID = directorID;
        }

        /// <summary>
        /// Accessor for retrieiving this Manager's group ID
        /// </summary>
        /// <exception cref="NullReferenceException">if the employee is not valid.</exception>
        public int GroupID {
            get {
                if (!this.isValid())
                    throw new NullReferenceException("Employee is not valid. Please fill in completely.");
                return (int)this.groupID;
            }
            set {
                if (!ConsoleFactory.getEmployeeSvc().isValidDirectorId(value))
                    throw new ArgumentOutOfRangeException("Group is not valid. Please enter the Group information first.");
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
                    throw new NullReferenceException("Manager is not valid. Please fill in completely.");
                return (int)this.directorID;
            }
            set {
                if (!ConsoleFactory.getEmployeeSvc().isValidDirectorId(value))
                    throw new ArgumentOutOfRangeException("Director is not valid. Please enter Director information first.");

                this.directorID = value;
            }
        }


        /// <summary>
        /// Asssess the User, startdate and groupID is valid
        /// </summary>
        /// <returns>True if the user is valid. Overrides base</returns>
        public new bool isValid() {
            return base.isValid() && this.groupID != null && this.directorID != null;
        }
    }
}
