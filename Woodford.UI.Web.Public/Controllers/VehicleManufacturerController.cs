using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.Code;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers {
    public class VehicleManufacturerController : Controller {

        private IQueryProcessor _query;
        public VehicleManufacturerController(IQueryProcessor query) {
            _query = query;
        }


        [Route("vehicle-hire")]
        public ActionResult Index() {
            return View();
        }

        [Route("vehicle-hire/{pageUrl}")]
        public ActionResult Manufacturer(string pageUrl) {
            VehicleManufacturerGetByUrlQuery query = new VehicleManufacturerGetByUrlQuery();
            query.Url = pageUrl;
            query.IncludePageContent = true;
            var model = _query.Process(query);

            VehicleManufacturerViewModel viewModel = new VehicleManufacturerViewModel();
            viewModel.Manufacturer = model;

            VehiclesGetQuery getVehicles = new VehiclesGetQuery();
            getVehicles.Filter = new VehicleFilterModel { VehicleManufacturerId = model.Id };


            var vehicles = _query.Process(getVehicles);

            viewModel.Items = vehicles.Items;

            return View(viewModel);
        }
    }
}