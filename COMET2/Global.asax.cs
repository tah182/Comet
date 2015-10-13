using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using COMET.Model.Domain;
using COMET.Model.Business.Factory;
using COMET.Model.Business.Manager;
using COMET.Model.Business.Service;

namespace COMET {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication {
        private readonly string TEST_CONN_STRING = "GAM_Complex_OpConnectionStringTest";
        private readonly string PROD_CONN_STRING = "GAM_Complex_OpConnectionString";

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        void Application_BeginRequest(Object source, EventArgs e) {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;

            // Setup the Configuration for the application
            Configuration config = new Configuration();
            string hostName = FirstRequestInitialization.Initialize(context);

            //#warning REMEMBER: Global.asax needs to be corrected!
            if (hostName.StartsWith("localho")) {
                config.add("application", "Comet-test");
                config.addDatabase(TEST_CONN_STRING);
            } else if (hostName.ToLower().StartsWith("comet-test") || hostName.StartsWith("localho")) {
                config.add("application", "Comet-test");
                config.addDatabase(TEST_CONN_STRING);
            } else {
                config.add("application", "Comet");
                config.addDatabase(PROD_CONN_STRING);
            }

            ////Application["Configuration"] = config;
        }

        class FirstRequestInitialization {
            private static bool initializedAlready = false;
            private static string host = "";
            private static Object _lock = new Object();

            // Initialize only on the first request
            public static string Initialize(HttpContext context) {
                if (initializedAlready)
                    return host;
                lock (_lock) {
                    if (initializedAlready) 
                        return host;

                    initializedAlready = true;
                    host = HttpContext.Current.Request.Url.Host.ToLower();
                    return host;
                }
            }
        }

        void Application_Error(object sender, EventArgs e) {
            Exception exc = Server.GetLastError();
            if (exc.GetType() == typeof(HttpException)) {
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;
                Server.ClearError();
                Response.Clear();
                Response.Redirect("~/Error/Error404");
                return;
            }

            Response.Write("<h2>Comet Page Error</h2>\n");
            Response.Write("<p>" + exc.Message + "</p>\n");
            COMET.Server.Domain.ApplicationErrors2 ae = MainFactory.getLogSvc().logError("Application", "Global", exc.Message, exc.StackTrace);
            Session["ApplicationError"] = exc.Message + "<br>" + exc.StackTrace;
            EmailSvc.EmailError(ae, exc.Message, exc.StackTrace);
            Server.ClearError();
        }

        void Session_Start(object sender, EventArgs e) {
            string sessionId = Session.SessionID;
            string userNT = Environment.UserName.ToLower();

            //if (userNT == "grodsky.holly")
            //    userNT = "long.kim";
            //if (userNT == "tatsumoto.takashi")
            //    userNT = "ranger.sherri";
            
            // Get The User Information from the Factory
            if (userNT.Length > 0 && !userNT.ToLower().Equals("iusr")) {
                UserMgr userMgr = new UserMgr(MainFactory.getUserSvc());
                HttpContext.Current.Session["User"] = userMgr.getUser(userNT);
            }

            if (HttpContext.Current.Request.Url.Host.ToLower().StartsWith("comet") && ((IUser) Session["User"]).UserType == UserType.PLANNER_911)
                Response.Redirect("~/Home/E911");

            //if (DateTime.Now.Hour < 12)
            //    HttpContext.Current.Session["Greeting"] = "Good Morning";
            //else if (DateTime.Now.Hour < 17)
            //    HttpContext.Current.Session["Greeting"] = "Good Afternoon";
            //else
            //    HttpContext.Current.Session["Greeting"] = "Good Evening";

        }

        void Page_Stage(object sender, EventArgs e) {

        }
    }
}