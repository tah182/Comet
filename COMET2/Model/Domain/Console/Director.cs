using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMET.Model.Domain.Console {
    public class Director : AEmployee {
        public Director(int directorID) : base(directorID) {
        }

        /// <summary>
        /// Creates an existing employee 
        /// </summary>
        /// <param name="employeeID">The employeeID of the employee</param>
        /// <param name="startDate">The start date of this assignment</param>
        /// <param name="endDate">The end date of this assignment</param>
        public Director(int directorID, DateTime startDate, DateTime? endDate) : base(directorID, startDate, endDate) {
        }
    }
}