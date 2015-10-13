using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace COMET {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "Console_Manager",
                url: "Console/Manager/{action}/{type}/{id}",
                defaults: new { controller = "Manager", action = "Dashboard", type = "", id = UrlParameter.Optional },
                namespaces: new[] { "COMET.Controllers.Console" }
            );

            routes.MapRoute(
                name: "Console",
                url: "Console/{action}/{type}/{id}",
                defaults: new { controller = "Console", action = "Index", type = "", id = UrlParameter.Optional },
                namespaces: new[] { "COMET.Controllers.Console" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "COMET.Controllers" }
            );
        }
    }
}