using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

using COMET.Model.Business.Factory;
using COMET.Model.Business.Service;

namespace COMET.Model.E911 {
    public class ValidPsaps {
        private HttpContext context;
        private List<int> psapList = new List<int>();
        private readonly DateTime cacheExpiration;
        public ValidPsaps(HttpContext context) {
            this.context = context;
            this.cacheExpiration = MainFactory.getCacheExpiration();
            updateValidPsapIds();
        }

        public bool isExpired() {
            if (DateTime.Now > this.cacheExpiration)
                return true;
            return false;
        }

        /**
         * Checks if psapId passed is valid
         **/
        public bool isValid(int psapId) {
            if (isExpired())
                updateValidPsapIds();

            return psapList.Contains(psapId);
        }

        /**
         * Creates an array of Valid PSAP Ids from the Arms MySql Database and stores into application cache memory
         **/
        public void updateValidPsapIds() {
            string queryString = "SELECT DISTINCT psap FROM arms.psap_contact";
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["ArmsMySqlServer"].ConnectionString)) {
                try {
                    MySqlCommand cmd = new MySqlCommand(queryString, conn);
                    cmd.Connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                        this.psapList.Add(Int32.Parse(reader["psap"].ToString()));
                } catch (MySqlException sqle) {
                    ILogSvc logger = MainFactory.getLogSvc();
                    logger.logError("ValidPSaps", MethodBase.GetCurrentMethod().ToString(), sqle.ErrorCode.ToString(), "Error Querying Arms in MySql. <br>" + sqle.StackTrace + "\n" + sqle.Message);
                }
            }
        }
    }
}