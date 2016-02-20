using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace dashing.net
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Dashboards",
                url: "dashboard/{dashboardName}",
                defaults: new {controller = "Dashboard", action = "Index"}
            );

            routes.MapRoute(
                name: "adminPost",
                url: "admin/SendMessage",
                defaults: new { controller = "Admin", action = "SendMessage" }
            );

            routes.MapRoute(
                name: "adminLogin",
                url: "admin/login",
                defaults: new { controller = "Admin", action = "Login" }
            );

            //routes.MapRoute(
            //    name: "AdminStuff",
            //    url: "Admin/{action}/{id}",
            //    defaults: new { controller = "Admin", action = UrlParameter.Optional, id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "admin",
                url: "admin/{view}",
                defaults: new { controller = "Admin", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}