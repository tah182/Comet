using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using COMET.Model.Business.Service.Console;

namespace COMET.Model.Business.Factory {
    public class ConsoleFactory {
        /// <summary>
        /// Retreives the service to create employees with
        /// </summary>
        /// <returns>The active service for creating employees</returns>
        public static IEmployeeSvc getEmployeeSvc() {
            return new EmployeeSvcImplDB();
        }
    }
}