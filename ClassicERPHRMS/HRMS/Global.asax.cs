using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HRMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            Application["TheException"] = exception;
            //Session["exception"] = exception.Message;
            Response.Clear();

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        break;
                    case 500:
                        // server error
                        action = "HttpError500";
                        break;
                    default:
                        action = "General";
                        break;
                }
                // clear error on server
                Server.ClearError();
                //Response.Redirect(String.Format("~/Error/{0}/?message={1}", action, exception.Message));
                Response.Redirect("~/Auth/Login", true);
            }
            else
            {
                Response.Redirect("~/Auth/Login",true);
            }
        }
    }
}
