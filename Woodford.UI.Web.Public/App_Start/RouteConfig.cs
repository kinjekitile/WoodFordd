using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Woodford.UI.Web.Public.Code;

namespace Woodford.UI.Web.Public {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "FileUploads",
            //    url: "Uploads/{id}/{filename}",
            //    defaults: new { controller = "Uploads", action = "Index", id = UrlParameter.Optional, filename = UrlParameter.Optional }
            //);            

            routes.LowercaseUrls = true;

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ).RouteHandler = new CustomPageRouteHandler();

            routes.MapRoute(
                  name: "CatchAll",
                  url: "{*any}",
                  defaults: new { controller = "UrlRedirects", action = "Handler" });

 //           routes.MapRoute(
 // name: "Dynamic",
 // url: "{*FriendlyUrl}"
 //).RouteHandler = new DynamicPageRouteHandler();






        }
    }
}
