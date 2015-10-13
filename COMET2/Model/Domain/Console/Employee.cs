using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using COMET.Model.Domain;
using COMET.Model.Business.Factory;

namespace COMET.Model.Domain.Console {
    public class Employee : AEmployee {
        private int? groupManagerID;

        public Employee(int employeeID) : base(employeeID) {
        }

        /// <summary>
        /// Creates an existing employee 
        /// </summary>
        /// <param name="employeeID">The employeeID of the employee</param>
        /// <param name="startDate">The start date of this assignment</param>
        /// <param name="endDate">The end date of this assignment</param>
        public Employee(int employeeID, DateTime startDate, DateTime? endDate, int groupManagerID) : base(employeeID, startDate, endDate){
            this.groupManagerID = groupManagerID;
        }

        /// <summary>
        /// Accessor for retrieiving this Employee's group ID
        /// </summary>
        /// <exception cref="NullReferenceException">if the employee is not valid.</exception>
        public int GroupManagerID {
            get {
                if (!this.isValid())
                    throw new NullReferenceException("Employee is not valid. Please fill in completely.");
                return (int)this.groupManagerID; 
            }
            set {
                if (!ConsoleFactory.getEmployeeSvc().isValidDirectorId(value))
                    throw new ArgumentOutOfRangeException("Group Manager is not valid. Please enter the manager first.");
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
