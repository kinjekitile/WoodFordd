using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers {
    public class HomeController : Controller {
        private IQueryProcessor _query;
        private ICommandBus _commandBus;

        public HomeController(IQueryProcessor query, ICommandBus commandBus) {
            _query = query;
            _commandBus = commandBus;
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult LoginPartial() {
            var currentUser = _query.Process(new UserGetCurrentQuery { });
            if (currentUser != null) {
                return PartialView("_LoginPartial", currentUser);
            } else {
                return PartialView("_LoginPartial", null);
            }
            
        }

        public ActionResult Herospace() {
            HerospaceItemsGetQuery query = new HerospaceItemsGetQuery { Filter = new HerospaceItemFilterModel { IsArchived = false } };
            return PartialView("_HerospacePartial", _query.Process(query));
        }

        public ActionResult Navigation() {

            NewsCategoryGetQuery query = new NewsCategoryGetQuery { Filter = new NewsCategoryFilterModel { IsArchived = false }, Pagination = null };
            NavigationViewModel model = new NavigationViewModel();
            ListOf<NewsCategoryModel> cats = _query.Process(query);
            if (cats.Items != null) {
                model.NewsCategories = cats.Items;
            }


            return PartialView("_NavigationPartial", model);
        }

        public ActionResult Signup() {
            return PartialView("_SignupPartial", new SubscribeViewModel());
        }

        [HttpPost]
        public ActionResult Signup([CustomizeValidator(RuleSet = SubscribeValidationRuleSets.Default)] SubscribeViewModel model) {
            if (ModelState.IsValid) {
                BulkMailingSignupCommand command = new BulkMailingSignupCommand { Email = model.SubscribeEmail };
                _commandBus.Submit(command);
                model.Success = true;                
            }
            return PartialView("_SignupPartial", model);
        }

        public ActionResult Test() {
            SearchCriteriaViewModel model = new SearchCriteriaViewModel();
            model.Foobar = DateTime.Today;
            return View(model);
        }

        public ActionResult ManufacturerDropDown() {
            VehicleManufacturerGetQuery query = new VehicleManufacturerGetQuery();
            var result = _query.Process(query);
            return View(result.Items);
        }
    }
}