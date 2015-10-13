using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using COMET.Model.Domain;
using COMET.Model.Business.Factory;

namespace COMET.Model.Console.Domain {
    public class Employee : AEmployee {
        private int? groupManagerID;

        public Employee(IUser user) : base(user) {
        }

        /// <summary>
        /// Creates an existing employee 
        /// </summary>
        /// <param name="employeeID">The employeeID of the employee</param>
        /// <param name="startDate">The start date of this assignment</param>
        /// <param name="endDate">The end date of this assignment</param>
        public Employee(IUser user, DateTime startDate, DateTime? endDate, int groupManagerID) : base(user, startDate, endDate){
            this.groupManagerID = groupManagerID;
        }

        /// <summary>
        /// Accessor for retrieiving this Employee's group ID
        /// </summary>
        /// <exception cref="NullReferenceException">if the employee is not valid.</exception>
        public int GroupManagerID {
            get {
                if (!this.isValid())
                    throw new InvalidOperationException("Employee is not valid. Please fill in completely.");
                return (int)this.groupManagerID; 
            }
            set {
                if (!ConsoleFactory.getEmployeeSvc().isValidGroupManagerId(value))
                    throw new InvalidOperationException("Group Manager is not valid. Please enter the manager first.");
                this.groupManagerID = value;
            }
        }

        /// <summary>
        /// Asssess the User, startdate and groupID is valid
        /// </summary>
        /// <returns>True if the user is valid. Overrides base</returns>
        public new bool isValid() {
            return base.isValid() && this.groupManagerID != null;
        }
    }
}
