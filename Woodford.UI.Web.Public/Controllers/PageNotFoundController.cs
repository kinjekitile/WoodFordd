using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Woodford.UI.Web.Public.Controllers {
    public class PageNotFoundController : Controller {
        
        public ActionResult Index() {
            //Response.StatusCode = 404;
            return View();
        }
    }
}