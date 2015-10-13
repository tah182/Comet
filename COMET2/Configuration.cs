using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

using COMET.Model.Business.Factory;
using COMET.Model.Business.Service;

namespace COMET {
    /// <summary>
    /// Configuration settings for the application stored in key, values.
    /// </summary>
    public class Configuration {
        private Dictionary<string, object> configMap = new Dictionary<string, object>();

        /// <summary>
        /// Creates a new Configuration instance and sets the database based on the instance of the application
        /// </summary>
        public Configuration() {
            addFromXml();
        }

        /// <summary>
        /// Adds a Database Connection string to the configuration
        /// </summary>
        /// <param name="connString">The connection string from the web.config</param>
        public void addDatabase(string connString) {
            configMap.Add("database", connString);
        }

        /// <summary>
        /// Retrieves values saved in the Config.xml file
        /// </summary>
        /// <returns>This class to allow method chaining</returns>
        private void addFromXml() {
            XmlDocument doc = new XmlDocument();
            doc.Load(HttpContext.Current.Server.MapPath("~/App_Data/Config.xml"));

            addXmlErrorEmail(doc);
            addEmail(doc);
            addConsoleEmail(doc);
        }

        private void addXmlErrorEmail(XmlDocument doc) {
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/config/ErrorEmail/to");
            string emails = "";
            string domain = doc.DocumentElement.SelectSingleNode("/config/ErrorEmail").Attributes["domain"].InnerText;
            foreach (XmlNode node in nodes) 
                emails += node.InnerText + domain + "|";

            if (emails.Length <= 0)
                throw new ArgumentNullException("emails", "Configuration file missing ErrorEmail element with <to> childs");

            configMap.Add("ErrorEmailTo", emails.Substring(0, emails.Length - 1));
        }

        private void addEmail(XmlDocument doc) {
            XmlNodeList nodes = doc.DocumentElement.SelectNodes("/config/Email/to");
            string emails = "";
            string domain = doc.DocumentElement.SelectSingleNode("/config/Email").Attributes["domain"].InnerText;
            foreach (XmlNode node in nodes)
                emails += node.InnerText + domain + "|";

            configMap.Add("EmailTo", emails.Substring(0, emails.Length - 1));
        }

        private void addConsoleEmail(XmlDocument doc) {
            XmlNode node = doc.DocumentElement.SelectSingleNode("/config/Console/emailFrom");
            configMap.Add("ConsoleFrom", node.InnerText);

            node = doc.DocumentElement.SelectSingleNode("/config/Console/BIManagerID");
            configMap.Add("BIManagerID", node.InnerText);

            node = doc.DocumentElement.SelectSingleNode("/config/Console/HoursCostMultiplierInternal");
            configMap.Add("HoursCostMultiplierInternal", node.InnerText);

            node = doc.DocumentElement.SelectSingleNode("/config/Console/HoursCostMultiplierExternal");
            configMap.Add("HoursCostMultiplierExternal", node.InnerText);

            node = doc.DocumentElement.SelectSingleNode("/config/Console/active");
            configMap.Add("EmailActive", node.InnerText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool add(string key, object val) {
            try {
                configMap.Add(key, val);
            } catch (Exception ae) {
                ILogSvc logger = MainFactory.getLogSvc();
                logger.logError("Configuration", "add", ae.InnerException.Source, ae.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Removes a key from the configuration Dictionary
        /// </summary>
        /// <param name="key">The key of the value</param>
        /// <returns>True if remove was successful</returns>
        public bool remove(string key) {
            try {
                configMap.Remove(key);
            } catch (Exception ae) {
                ILogSvc logger = MainFactory.getLogSvc();
                logger.logError("Configuration", "remove", ae.InnerException.Source, ae.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the object from the Dictionary using the key
        /// </summary>
        /// <param name="key">The key the value is mapped to</param>
        /// <returns>Th object from the key</returns>
        public virtual object get(string key) {
            object returnObject = null;
            configMap.TryGetValue(key, out returnObject);
            
            return returnObject;
        }
    }
}