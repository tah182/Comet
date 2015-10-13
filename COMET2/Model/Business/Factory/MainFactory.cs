using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Diagnostics;

using COMET.Model.Business.Service;
using COMET.Model.Domain;

using COMET.Controllers;

namespace COMET.Model.Business.Factory {
    /// <summary>
    /// The Main Factory for the application
    /// </summary>
    public class MainFactory {
        private static readonly string NAMESPACE = "COMET.Server.Domain";
        private static readonly string TEST_CONN_STRING = "GAM_Complex_OpConnectionStringTest";
        private static readonly string PROD_CONN_STRING = "GAM_Complex_OpConnectionString";

        /// <summary>
        /// Creates a UserSvc Implementation
        /// </summary>
        /// <returns>The Implemented User Service</returns>
        public static IUserSvc getUserSvc() {
            return new UserSvcImplDB();
        }

        /// <summary>
        /// Creates a LogSvc Implementation 
        /// </summary>
        /// <returns>The Implemented Log Service</returns>
        public static ILogSvc getLogSvc() {
            return new LogSvcImplDB();
        }

        /// <summary>
        /// Creates a BulkSvc Implementation 
        /// </summary>
        /// <returns>The Implemented Bulk Service</returns>
        public static IBulkSvc getBulkSvc() {
            return new BulkSvcImplDB();
        }

        /// <summary>
        /// Creates a QuoteSvc Implementation
        /// </summary>
        /// <returns></returns>
        public static IQuoteSvc getQuoteSvc() {
            return new QuoteSvcImplDB();
        }

        /// <summary>
        /// Returns the Configuration managed by the configuration xml in app_data
        /// </summary>
        /// <returns>The Configuration for the runtime environment</returns>
        public static Configuration getConfiguration() {
            string hostName = HttpContext.Current.Request.Url.Host.ToLower();
            
            Configuration config = new Configuration();
            //#warning Host Application Database connection strings are modified.
            if (hostName.ToLower().StartsWith("comet-test") || hostName.StartsWith("localho")) {
                config.add("application", "Comet-test");
                config.addDatabase(TEST_CONN_STRING);
            } else {
                config.add("application", "Comet");
                config.addDatabase(PROD_CONN_STRING);
            }
            
            return config;
        }

        /// <summary>
        /// Returns a DataContext for the class passed dynamically
        /// </summary>
        /// <param name="className">The prefix name of the DataContext class</param>
        /// <returns>The DataContext for the Linq-to-sql</returns>
        public static DataContext getDb(string className, bool readOnly) {
            Configuration config = getConfiguration();

            string connString = global::System.Configuration.ConfigurationManager.ConnectionStrings[(string)config.get("database")].ConnectionString;
            
            DataContext dc = (DataContext)Activator.CreateInstance(Type.GetType(NAMESPACE + "." + className + "DataContext"), connString);
            if (readOnly) {
                dc.ObjectTrackingEnabled = false;
                dc.DeferredLoadingEnabled = false;
            }

            // give bulk sql requests up to 3 minutes to complete.
            if (className.Equals("Bulk") || className.Equals("User"))
                dc.CommandTimeout = 3 * 60;

            return (DataContext) dc;
        }

        /// <summary>
        /// Returns a Connection for the MS-SQL Database for queries
        /// </summary>
        /// <returns>The connection used for the context instance of the application.</returns>
        public static SqlConnection getConnection() {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[(string)getConfiguration().get("database")].ConnectionString);
        }

        /// <summary>
        /// Returns the instance of this application run time.
        /// </summary>
        /// <returns>The string name of the instance.</returns>
        public static string getInstance() {
            return ContextController.getContext().Request.Url.Host.ToLower();
        }

        /// <summary>
        /// Pretifies a Vendor Name string passed into it
        /// </summary>
        /// <param name="provider">The name of the Vendor to format</param>
        /// <returns>A foramatted string of the Vendor's name</returns>
        public static string formatProvider(string provider) {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo
                        .ToTitleCase(provider.ToLower())
                        .Replace("Llc", "LLC")
                        .Replace("L.p", "L.P")
                        .Replace("At&T", "AT&T")
                        .Replace("Tw", "TW");
        }

        /// <summary>
        /// Returns a static Time of 06:00:00 of the next day.
        /// </summary>
        /// <returns>06:00:00 of the next day</returns>
        public static DateTime getCacheExpiration() {
            DateTime date = DateTime.Now.AddDays(1);
            return new DateTime(date.Year, date.Month, date.Day, 6, 0, 0);
        }

        /// <summary>
        /// Returns the method's name that calls this method.
        /// </summary>
        /// <returns>Returns method A if A calls getCurrentMethod()</returns>
        public static string getCurrentMethod() {
            return new StackTrace().GetFrame(1).GetMethod().Name;
        }
    }
}