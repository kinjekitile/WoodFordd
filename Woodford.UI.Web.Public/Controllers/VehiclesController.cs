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

namespace Woodford.UI.Web.Public.Controllers
{
    public class VehiclesController : Controller
    {
        private IQueryProcessor _query;

        public VehiclesController(IQueryProcessor query) {
            _query = query;
        }

        [Route("vehicles")]
        public ActionResult Index()
        {
            VehiclesGetQuery query = new VehiclesGetQuery { Filter = new VehicleFilterModel { IsArchived = false } };
            var results = _query.Process(query);
            results.Items = results.Items.OrderBy(x => x.VehicleGroup.SortOrder).ThenBy(x => x.SortOrder).ToList();
            return View(results);
        }

        [Route("vehicles/{vehicleUrl?}")]
        public ActionResult Vehicle(string vehicleUrl) {
            VehicleGetByUrlQuery query = new VehicleGetByUrlQuery { Url = vehicleUrl, IncludePageContent = true };
            VehicleModel v = _query.Process(query);
            return View(v);
        }

        [Route("vehicles/{vehicleUrl?}/book")]
        public ActionResult Book(string vehicleUrl) {
            VehicleGetByUrlQuery query = new VehicleGetByUrlQuery { Url = vehicleUrl, IncludePageContent = true };

            BookVehicleViewModel model = new BookVehicleViewModel();
            model.Vehicle = _query.Process(query);
            model.SearchCriteria = SearchCriteriaViewModelFactory.CreateInstance();
            model.SearchCriteria.Criteria.VehicleId = model.Vehicle.Id;
            return View(model);
        }

        [HttpPost]
        [Route("vehicles/{vehicleUrl?}/book")]
        public ActionResult Book(BookVehicleViewModel viewModel) {

            VehicleGetByIdQuery query = new VehicleGetByIdQuery { Id = viewModel.SearchCriteria.Criteria.VehicleId.Value, includePageContent = true };
            //BookVehicleViewModel model = new BookVehicleViewModel();
            //model.Vehicle = _query.Process(query);
            //model.SearchCriteria = viewModel.SearchCriteria;
            //model.SearchCriteria.Criteria.VehicleId = model.Vehicle.Id;

            viewModel.SearchCriteria.Criteria.PickupDate = viewModel.SearchCriteria.PickupDate;
            viewModel.SearchCriteria.Criteria.DropOffDate = viewModel.SearchCriteria.DropOffDate;

            
            BookingSearchQuery searchQuery = new BookingSearchQuery { Criteria = viewModel.SearchCriteria.Criteria };
            var results = _query.Process(searchQuery);

            SearchResultsViewModel model = new SearchResultsViewModel();
            model.Criteria = viewModel.SearchCriteria;
            

            model.Criteria.Criteria = viewModel.SearchCriteria.Criteria;


            model.Criteria.Criteria.PickupDate = viewModel.SearchCriteria.PickupDate;
            model.Criteria.PickupDate = viewModel.SearchCriteria.PickupDate;
            model.Criteria.Criteria.PickupTime = viewModel.SearchCriteria.Criteria.PickUpTimeFullInt;

            model.Criteria.Criteria.DropOffDate = viewModel.SearchCriteria.DropOffDate;
            model.Criteria.DropOffDate = viewModel.SearchCriteria.DropOffDate;
            model.Criteria.Criteria.DropOffTime = viewModel.SearchCriteria.Criteria.DropOffTimeFullInt;

            model.Results = results;
            return View("Results", model);
        }
    }
}