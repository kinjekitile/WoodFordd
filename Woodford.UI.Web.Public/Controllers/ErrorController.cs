using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Woodford.UI.Web.Public.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult ServerError()
        {
            //Response.StatusCode = 500;
            return View();
        }

        public ActionResult Test() {
            int x = 5;
            int y = 0;
            int z = x / y;
            return View();
        }
    }
}