using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using COMET.Model.Domain;

namespace COMET.Controllers {
    /// <summary>
    /// Allows for access to datamembers that are stored in HttpContext.Current
    /// </summary>
    public class ContextController : Controller {
        /// <summary>
        /// Retrieve the user saved in current context
        /// </summary>
        /// <returns>The User information</returns>
        public static IUser getContextUser() {
            return getContextUser(getContext());
        }

        /// <summary>
        /// Retreives the user saved in the context paassed to method
        /// </summary>
        /// <param name="context">The context in which data is saved</param>
        /// <returns>The User information</returns>
        public static IUser getContextUser(HttpContext context) {
            return ((IUser)context.Session["User"]);
        }

        /// <summary>
        /// Retreives the current context.
        /// </summary>
        /// <returns>The current context</returns>
        public static HttpContext getContext() {
            return System.Web.HttpContext.Current;
        }
    }
}
