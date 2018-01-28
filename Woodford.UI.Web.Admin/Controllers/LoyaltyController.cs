using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator")]
    public class LoyaltyController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public LoyaltyController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        // GET: Loyalty
        public ActionResult Index(int p = 1) {
            LoyaltyTierGetBenefitsQuery query = new LoyaltyTierGetBenefitsQuery();
            query.Level = LoyaltyTierLevel.All;
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<LoyaltyTierBenefitModel> benefits = _query.Process(query);

            return View(benefits);
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            LoyaltyTierModel tier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = (LoyaltyTierLevel)id });
            return View(new LoyaltyTierViewModel { Tier = tier });
        }

        [HttpPost]
        public ActionResult Edit(LoyaltyTierViewModel viewModel) {
            if (ModelState.IsValid) {
                LoyaltyTierEditCommand command = new LoyaltyTierEditCommand { Model = viewModel.Tier };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddBenefit() {
            LoyaltyTierBenefitViewModel viewModel = new LoyaltyTierBenefitViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddBenefit([CustomizeValidator(RuleSet = LoyaltyValidatorRuleSets.Default)] LoyaltyTierBenefitViewModel viewModel) {

            if (ModelState.IsValid) {
                LoyaltyTierBenefitAddCommand addBenefit = new LoyaltyTierBenefitAddCommand();
                addBenefit.Model = viewModel.Benefit;
                _commandBus.Submit(addBenefit);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult EditBenefit(int id) {
            LoyaltyTierBenefitViewModel viewModel = new LoyaltyTierBenefitViewModel();
            viewModel.Benefit = _query.Process(new LoyaltyTierGetBenefitByIdQuery { Id = id });
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditBenefit([CustomizeValidator(RuleSet = LoyaltyValidatorRuleSets.Default)] LoyaltyTierBenefitViewModel viewModel) {

            if (ModelState.IsValid) {
                LoyaltyTierBenefitEditCommand editBenefit = new LoyaltyTierBenefitEditCommand();
                editBenefit.Model = viewModel.Benefit;
                _commandBus.Submit(editBenefit);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

    }
}