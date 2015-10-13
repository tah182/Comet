using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using COMET.Model.Business.Service;
using COMET.Server.Domain;

namespace COMET.Model.Business.Factory {
    public class ReferenceFactory {

        /// <summary>
        /// Creates a Svc Object for getting region data such as state, provider, city, locale.
        /// </summary>
        /// <param name="type">The type of object to expect back.</param>
        /// <returns>IRegion data</returns>
        public static IRegion get(string type) {
            switch (type.ToLower()) {
                case "states" :
                    return new StatesSvcImplDB();
                case "providers" :
                    return new ProviderSvcImplDB();
                default :
                    return new StatesSvcImplDB();
            }
        }

        /// <summary>
        /// Returns the latest database time stamp of the column from COMET2.ReferenceTableUpdate
        /// </summary>
        /// <param name="key">The Column to look at</param>
        /// <returns>The Datestamp from the DB</returns>
        public static DateTime lastUpdated(string key) {
            using (SqlConnection conn = MainFactory.getConnection())
            using (SqlCommand cmd = new SqlCommand(String.Format("SELECT TOP(1) {0} FROM COMET.ReferenceTableUpdates;", key), conn)) {
                try {
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                        return reader.GetDateTime(0);
                } catch (SqlException se) {
                    MainFactory.getLogSvc().logError("Index", MainFactory.getCurrentMethod(), "rf-lu-01", se.Message + "<br>" + se.StackTrace);
                }
            }
            return getDefaultDate();
        }

        /// <summary>
        /// Returns 1900.01.01 as a DateTime
        /// </summary>
        /// <returns>1900.01.01</returns>
        public static DateTime getDefaultDate() {
            return DateTime.Parse("1/1/1900");
        }
    }
}