using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers {
    public class UrlRedirectsController : Controller {
        private IQueryProcessor _query;
        private ICommandBus _commandBus;

        public UrlRedirectsController(IQueryProcessor query, ICommandBus commandBus) {
            _query = query;
            _commandBus = commandBus;
        }

        public ActionResult Handler() {

            string url = Request.Url.AbsolutePath;

            UrlRedirectGetByUrlQuery query = new UrlRedirectGetByUrlQuery { Url = url };
            var redirect = _query.Process(query);

            if (redirect != null) {

                Response.StatusCode = Convert.ToInt32(redirect.StatusCode);

                switch (redirect.RedirectType) {
                    case UrlRedirectType.MovedPermanently:
                        return RedirectPermanent(redirect.NewUrl);
                        
                    case UrlRedirectType.NotFound:
                        return RedirectToAction("Index", "PageNotFound");
                        
                    case UrlRedirectType.Gone:
                        return RedirectToAction("Index", "PageRemoved");
                }
                

                
            } else {
                return RedirectToAction("Index", "PageNotFound");
            }
            
            return View();
        }
    }
}