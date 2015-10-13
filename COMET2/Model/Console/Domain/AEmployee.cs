using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;
using COMET.Model.Business.Manager;
using COMET.Model.Business.Factory;

namespace COMET.Model.Console.Domain {
    public abstract class AEmployee : IEmployee {
        IUser user;
        private DateTime? endDate;

        /// <summary>
        /// Creates a new employee using employee ID. The start date will default to today
        /// </summary>
        /// <param name="employeeID">The employee ID of the employee</param>
        public AEmployee(IUser user) {
            this.StartDate = DateTime.Today;
            this.user = user;
        }

        /// <summary>
        /// Creates an existing employee 
        /// </summary>
        /// <param name="employeeID">The employeeID of the employee</param>
        /// <param name="startDate">The start date of this assignment</param>
        /// <param name="endDate">The end date of this assignment</param>
        public AEmployee(IUser user, DateTime startDate, DateTime? endDate) {
            this.StartDate = startDate;
            this.endDate = endDate;

            this.user = user;
        }

        public int ID {
            get { return this.user.EmployeeID; }
        }

        public IUser User {
            get { return this.user; }
        }

        public DateTime StartDate {
            get;
            private set;
        }

        public DateTime? EndDate {
            get { return this.endDate; }
        }

        public bool setEndDate(DateTime endDate) {
            this.endDate = endDate;

            if (this.endDate.Equals(this.StartDate)) {
                this.endDate = null;
                throw new ArgumentException("Employee cannot start and end a position on the same day.");
            }
            return true;
        }

        public bool setEndDate() {
            return this.setEndDate(DateTime.Today);
        }

        public bool isValid() {
            return this.user != null && this.StartDate != null;
        }

        public bool Active {
            get { return this.endDate == null; }
        }

        public override string ToString() {
            return "ID:" + this.ID + " Name:" + this.User.EnglishName;
        }
    }
}