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
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator")]
    public class BranchRateCodeExclusionsController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public BranchRateCodeExclusionsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        public ActionResult Index() {
            return View(new BranchRateCodeExclusionsViewModel());
        }


        public ActionResult Filter(int? branchId) {
            if (branchId.HasValue) {
                BranchRateCodeExclusionsViewModel model = new BranchRateCodeExclusionsViewModel();
                model.BranchId = branchId.Value;

                BranchRateCodeExclusionsGetQuery query = new BranchRateCodeExclusionsGetQuery { Filter = new BranchRateCodeExclusionFilterModel { BranchId = model.BranchId, ShowPastExclusions = model.ShowPastExclusions } };


                model.BranchRateCodeExclusions = _query.Process(query);
                model.Filtered = true;
                return View("Index", model);
            } else {
                return RedirectToAction("Index");
            }

        }

        private ListOf<BranchRateCodeExclusionModel> getBranchRateCodeExclusions(int branchId) {
            BranchRateCodeExclusionsGetQuery query = new BranchRateCodeExclusionsGetQuery { Filter = new BranchRateCodeExclusionFilterModel { BranchId = branchId } };
            return _query.Process(query);
        }

        [HttpPost]
        public ActionResult Filter([CustomizeValidator(RuleSet = BranchRateCodeExclusionsFilterValidationRuleSets.Default)] BranchRateCodeExclusionsViewModel model) {

            if (ModelState.IsValid) {
                BranchRateCodeExclusionsGetQuery query = new BranchRateCodeExclusionsGetQuery { Filter = new BranchRateCodeExclusionFilterModel { BranchId = model.BranchId, ShowPastExclusions = model.ShowPastExclusions } };

                model.BranchRateCodeExclusions = _query.Process(query);
                model.Filtered = true;
            }

            return View("Index", model);
        }

        private BranchModel getBranch(int branchId) {
            BranchGetByIdQuery query = new BranchGetByIdQuery { Id = branchId, IncludePageContent = false };
            return _query.Process(query);
        }
        private RateCodeModel getRateCode(int rateCodeId) {
            RateCodeGetByIdQuery query = new RateCodeGetByIdQuery { Id = rateCodeId };
            return _query.Process(query);
        }


        public ActionResult Add(int branchId) {
            BranchRateCodeExlusionViewModel model = new BranchRateCodeExlusionViewModel();
            model.Exclusion = new BranchRateCodeExclusionModel { StartDate = DateTime.Now, EndDate = DateTime.Now, BranchId = branchId };
            model.Branch = getBranch(branchId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = BranchRateCodeExclusionValidationRuleSets.Default)] BranchRateCodeExlusionViewModel model) {
            if (ModelState.IsValid) {
                BranchRateCodeExclusionsAddCommand command = new BranchRateCodeExclusionsAddCommand { Model = model.Exclusion };
                _commandBus.Submit(command);
                return RedirectToAction("Filter", new { branchId = model.Exclusion.BranchId });
            }
            model.Branch = getBranch(model.Exclusion.BranchId);
            return View(model);
        }

        private BranchRateCodeExclusionModel getExclusion(int id) {
            BranchRateCodeExclusionsGetByIdQuery query = new BranchRateCodeExclusionsGetByIdQuery { Id = id };
            return _query.Process(query);
        }

        public ActionResult Edit(int id) {
            BranchRateCodeExlusionViewModel model = new BranchRateCodeExlusionViewModel();
            model.Exclusion = getExclusion(id);
            model.RateCode = getRateCode(model.Exclusion.RateCodeId);
            model.Branch = getBranch(model.Exclusion.BranchId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit([CustomizeValidator(RuleSet = VehicleAvailabilityExclusionValidationRuleSets.Default)] BranchRateCodeExlusionViewModel model) {
            if (ModelState.IsValid) {
                BranchRateCodeExclusionsEditCommand command = new BranchRateCodeExclusionsEditCommand { Model = model.Exclusion };
                _commandBus.Submit(command);
                return RedirectToAction("Filter", new { branchId = model.Branch.Id });
            }
            model.RateCode = getRateCode(model.RateCode.Id);
            model.Branch = getBranch(model.Branch.Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int exclusionId, int branchId) {
            BranchRateCodeExclusionsDeleteCommand command = new BranchRateCodeExclusionsDeleteCommand { Id = exclusionId };
            
            _commandBus.Submit(command);
            return RedirectToAction("Filter", new { branchId = branchId });
        }

    }
}