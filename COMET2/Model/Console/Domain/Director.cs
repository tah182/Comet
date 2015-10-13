using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Domain;

namespace COMET.Model.Console.Domain {
    public class Director : AEmployee {
        public Director(IUser director)
            : base(director) {
        }

        /// <summary>
        /// Creates an existing employee 
        /// </summary>
        /// <param name="employeeID">The employeeID of the employee</param>
        /// <param name="startDate">The start date of this assignment</param>
        /// <param name="endDate">The end date of this assignment</param>
        public Director(IUser director, DateTime startDate, DateTime? endDate)
            : base(director, startDate, endDate) {
        }
    }
}