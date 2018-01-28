using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Code {
    public class CustomPageRouteHandler : MvcRouteHandler {

        private IQueryProcessor _query;
        public CustomPageRouteHandler() {
            //_query = query;
            //_query = new DefaultQueryProcessor();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();
        }

        protected override IHttpHandler GetHttpHandler(RequestContext requestContext) {

            try {
                //try find controller in requestContext
                var controllerName = requestContext.RouteData.GetRequiredString("controller");

              
                    var controller = ControllerBuilder.Current.GetControllerFactory().
                CreateController(requestContext, controllerName);
               
                
                //if (controller != null) {
                //controller.Execute(requestContext);
                //}
            }
            catch (Exception ex) {

                //can't find the controller, so check in dynamic pages

                string requestUrl = requestContext.HttpContext.Request.Url.AbsolutePath;
                if (requestUrl.StartsWith("/")) {
                    requestUrl = requestUrl.Substring(1, requestUrl.Length - 1);
                }

                DynamicPageIdGetByUrlQuery query = new DynamicPageIdGetByUrlQuery { Url = requestUrl };

                int? pageId = _query.Process(query);

                //PageContentGetIdByUrlQuery query = new PageContentGetIdByUrlQuery { Url = requestUrl };

                //int? pageId = _query.Process(query);
                if (pageId.HasValue) {
                    if (pageId.Value == 0) {

                        //TODO look in url redirects before going to 404

                        string url = requestContext.HttpContext.Request.Url.AbsolutePath;

                        UrlRedirectGetByUrlQuery redirectQuery = new UrlRedirectGetByUrlQuery { Url = url };
                        var redirect = _query.Process(redirectQuery);

                        if (redirect != null) {


                            requestContext.HttpContext.Response.StatusCode = Convert.ToInt32(redirect.StatusCode);
                            switch (redirect.RedirectType) {
                                case UrlRedirectType.MovedPermanently:
                                    requestContext.HttpContext.Response.RedirectPermanent(redirect.NewUrl);
                                    break;
                                case UrlRedirectType.NotFound:
                                    requestContext.RouteData.Values["controller"] = "PageNotFound"; // " page.ControllerName;
                                    requestContext.RouteData.Values["action"] = "Index";
                                    break;

                                case UrlRedirectType.Gone:
                                    requestContext.RouteData.Values["controller"] = "PageRemoved"; // " page.ControllerName;
                                    requestContext.RouteData.Values["action"] = "Index";
                                    requestContext.HttpContext.Response.StatusCode = 410;
                                    break;
                            }



                        }
                        else {
                            requestContext.HttpContext.Response.Redirect("/PageNotFound");
                        }



                    }
                    else {
                        requestContext.RouteData.Values["controller"] = "DynamicPage"; // " page.ControllerName;
                        requestContext.RouteData.Values["action"] = "Index";
                        requestContext.RouteData.Values["id"] = pageId.Value;
                    }

                }
                else {
                    requestContext.HttpContext.Response.StatusCode = 404;
                    requestContext.HttpContext.Response.Redirect("/PageNotFound");
                }

            }
            return base.GetHttpHandler(requestContext);

        }

    }
}
