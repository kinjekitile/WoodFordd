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
using Woodford.UI.Web.Public.Code;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers
{
    public class MonthlyRatesController : Controller
    {
        private IQueryProcessor _query;
        private ICommandBus _commandBus;
        ISettingService _settings;
        

        

        public MonthlyRatesController(IQueryProcessor query, ICommandBus commandBus, ISettingService settings) {
            _query = query;
            _commandBus = commandBus;
            _settings = settings;
        }
        public ActionResult Index() {

            int minLeadTime = Convert.ToInt32(_settings.Get(Setting.Booking_Lead_Time_Hours).Value);
            int defaultLocationId = Convert.ToInt32(_settings.Get(Setting.Default_Location_Id).Value);

            var locations = _query.Process(new BranchesGetQuery { Filter = new BranchFilterModel { IsArchived = false } }).Items;

            //var monthlyLocation = locations.SingleOrDefault(x => x.Title.Contains("King Shaka International"));

            //if (monthlyLocation == null) {

            //} else {
            //    defaultLocationId = locations.SingleOrDefault(x => x.Title.Contains("King Shaka International")).Id;
            //}

            

            SearchResultsViewModel model = new SearchResultsViewModel();
            SearchCriteriaViewModel viewModel = new SearchCriteriaViewModel();
            viewModel.Criteria = new SearchCriteriaModel();

            viewModel.Criteria.PickupDate = DateTime.Today.AddHours(Math.Max(minLeadTime, 24));
            viewModel.Criteria.DropOffDate = viewModel.Criteria.PickupDate.AddDays(28);
            viewModel.PickupDate = viewModel.Criteria.PickupDate;
            viewModel.DropOffDate = viewModel.Criteria.DropOffDate;
            viewModel.AirportLocationIds = SearchCriteriaViewModelFactory.CreateInstance().AirportLocationIds;
            

            viewModel.Criteria.PickupTime = DateTime.Now.Hour + 2;
            viewModel.Criteria.DropOffTime = DateTime.Now.Hour + 2;

            viewModel.Criteria.PickupTimeFull = DateTime.Now.ToString("hh:mm tt");
            viewModel.Criteria.DropOffTimeFull = DateTime.Now.ToString("hh:mm tt");

            viewModel.Criteria.PickUpLocationId = defaultLocationId;
            viewModel.Criteria.DropOffLocationId = defaultLocationId;
            viewModel.Criteria.MinimumDays = 27;
            model.Criteria = viewModel;

            model.Results = new SearchResultsModel();
            model.Results.Criteria = viewModel.Criteria;
            model.Criteria = viewModel;


            if (ModelState.IsValid) {




                BookingSearchQuery query = new BookingSearchQuery { Criteria = viewModel.Criteria };
                model.Results = _query.Process(query);


            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Results([CustomizeValidator(RuleSet = SearchValidationRuleSets.Default)] SearchCriteriaViewModel viewModel) {

            SearchResultsViewModel model = new SearchResultsViewModel();

            viewModel.Criteria.MinimumDays = 27;
            viewModel.Criteria.PickupDate = viewModel.PickupDate;
            viewModel.Criteria.DropOffDate = viewModel.DropOffDate;


            viewModel.PickupDate = viewModel.Criteria.PickupDate;
            viewModel.DropOffDate = viewModel.Criteria.DropOffDate;

            model.Criteria = viewModel;

            model.Results = new SearchResultsModel();
            model.Results.Criteria = viewModel.Criteria;

          

            if (ModelState.IsValid) {

            
               

                BookingSearchQuery query = new BookingSearchQuery { Criteria = viewModel.Criteria };
                model.Results = _query.Process(query);


            }

            return View(model);
        }
    }
}