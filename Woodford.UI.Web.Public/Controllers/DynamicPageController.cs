using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Controllers {
    public class DynamicPageController : Controller {
        private IQueryProcessor _query;

        public DynamicPageController(IQueryProcessor query) {
            _query = query;
        }

        public ActionResult Index(int id) {
            DynamicPageGetByIdQuery query = new DynamicPageGetByIdQuery { Id = id, IncludePageContent = true };
            DynamicPageModel model = _query.Process(query);                        
            return View(model);            
        }
    }
}