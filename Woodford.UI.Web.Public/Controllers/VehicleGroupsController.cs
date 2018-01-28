using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.Controllers
{
    public class VehicleGroupsController : Controller
    {

        private IQueryProcessor _query;

        public VehicleGroupsController(IQueryProcessor query) {
            _query = query;
        }

        [Route("vehiclegroups")]
        public ActionResult Index()
        {
            VehicleGroupsGetQuery query = new VehicleGroupsGetQuery { Filter = new VehicleGroupFilterModel { IsArchived = false } } ;
            return View(_query.Process(query));
        }

        [Route("vehiclegroups/{vehicleGroupUrl?}")]
        public ActionResult VehicleGroup(string vehicleGroupUrl) {
            VehicleGroupGetByUrlQuery query = new VehicleGroupGetByUrlQuery { Url = vehicleGroupUrl, IncludePageContent = true };            
            VehicleGroupModel vg = _query.Process(query);
            return View(vg);
        }
    }
}