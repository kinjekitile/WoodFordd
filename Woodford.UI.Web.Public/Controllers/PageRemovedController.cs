using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Woodford.UI.Web.Public.Controllers {
    public class PageRemovedController : Controller {

        public ActionResult Index() {
            Response.StatusCode = 410;
            return View();
        }
    }
}