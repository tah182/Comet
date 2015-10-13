using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace COMET.Model.Business.Service {
    /// <summary>
    /// Retruns a JSON object with max text size capable
    /// </summary>
    /// <typeparam name="T">The object type to serialize</typeparam>
    public class Jsonify<T> {
        /// <summary>
        /// Serialize the object
        /// </summary>
        /// <param name="input">The object to serialize</param>
        /// <returns>JSON formatted text object</returns>
        public static ContentResult Serialize(T input, bool pretty = false) {
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            var JsonResult = new ContentResult {
                Content = JsonConvert.SerializeObject(input, pretty ? Formatting.Indented : Formatting.None),
                ContentType = "application/json"
            };

            return JsonResult;
        }
    }
}