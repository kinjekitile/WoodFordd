using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdvanceController : Controller
    {
        // GET: Advance
        public ActionResult Index()
        {
            return View();
        }
    }
}