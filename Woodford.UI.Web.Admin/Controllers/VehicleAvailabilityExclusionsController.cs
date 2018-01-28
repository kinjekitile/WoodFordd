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
    public class VehicleAvailabilityExclusionsController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public VehicleAvailabilityExclusionsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index() {
            return View(new VehicleExclusionsViewModel());
        }


        public ActionResult Filter(int? branchId) {
            if (branchId.HasValue) {
                VehicleExclusionsViewModel model = new VehicleExclusionsViewModel();
                model.BranchId = branchId.Value;
                BranchVehiclesGetQuery query = new BranchVehiclesGetQuery { Filter = new BranchVehicleFilterModel { BranchId = model.BranchId, ShowPastExclusions = model.ShowPastExclusions } };

                model.BranchVehicles = _query.Process(query);
                model.Filtered = true;
                return View("Index", model);
            } else {
                return RedirectToAction("Index");
            }

        }

        private ListOf<BranchVehicleModel> getBranchVehicles(int branchId) {
            BranchVehiclesGetQuery query = new BranchVehiclesGetQuery { Filter = new BranchVehicleFilterModel { BranchId = branchId } };
            return _query.Process(query);
        }

        [HttpPost]
        public ActionResult Filter([CustomizeValidator(RuleSet = VehicleAvailabilityExclusionsFilterValidationRuleSets.Default)] VehicleExclusionsViewModel model) {

            if (ModelState.IsValid) {
                BranchVehiclesGetQuery query = new BranchVehiclesGetQuery { Filter = new BranchVehicleFilterModel { BranchId = model.BranchId, ShowPastExclusions = model.ShowPastExclusions } };

                model.BranchVehicles = _query.Process(query);
                model.Filtered = true;
            }

            return View("Index", model);
        }

        private BranchModel getBranch(int branchId) {
            BranchGetByIdQuery query = new BranchGetByIdQuery { Id = branchId, IncludePageContent = false };
            return _query.Process(query);
        }
        private VehicleModel getVehicle(int vehicleId) {
            VehicleGetByIdQuery query = new VehicleGetByIdQuery { Id = vehicleId, includePageContent = false };
            return _query.Process(query);
        }


        public ActionResult Add(int vehicleId, int branchId, int branchVehicleId) {
            VehicleExlusionViewModel model = new VehicleExlusionViewModel();
            model.Exclusion = new BranchVehicleExclusionModel { StartDate = DateTime.Now, EndDate = DateTime.Now };
            model.Exclusion.BranchVehicleId = branchVehicleId;
            model.Vehicle = getVehicle(vehicleId);
            model.Branch = getBranch(branchId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = VehicleAvailabilityExclusionValidationRuleSets.Default)] VehicleExlusionViewModel model) {
            if (ModelState.IsValid) {
                BranchVehicleExclusionsAddCommand command = new BranchVehicleExclusionsAddCommand { Model = model.Exclusion };
                _commandBus.Submit(command);
                return RedirectToAction("Filter", new { branchId = model.Branch.Id });
            }
            model.Vehicle = getVehicle(model.Vehicle.Id);
            model.Branch = getBranch(model.Branch.Id);
            return View(model);
        }

        private BranchVehicleExclusionModel getExclusion(int id) {
            BranchVehicleExclusionsGetByIdQuery query = new BranchVehicleExclusionsGetByIdQuery { Id = id };
            return _query.Process(query);
        }

        public ActionResult Edit(int id, int vehicleId, int branchId) {
            VehicleExlusionViewModel model = new VehicleExlusionViewModel();
            model.Exclusion = getExclusion(id);
            model.Vehicle = getVehicle(vehicleId);
            model.Branch = getBranch(branchId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit([CustomizeValidator(RuleSet = VehicleAvailabilityExclusionValidationRuleSets.Default)] VehicleExlusionViewModel model) {
            if (ModelState.IsValid) {
                BranchVehicleExclusionsEditCommand command = new BranchVehicleExclusionsEditCommand { Model = model.Exclusion };
                _commandBus.Submit(command);
                return RedirectToAction("Filter", new { branchId = model.Branch.Id });
            }
            model.Vehicle = getVehicle(model.Vehicle.Id);
            model.Branch = getBranch(model.Branch.Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int exclusionId, int branchId) {
            BranchVehicleExclusionsDeleteCommand command = new BranchVehicleExclusionsDeleteCommand { Id = exclusionId };
            _commandBus.Submit(command);
            return RedirectToAction("Filter", new { branchId = branchId });
        }

    }
}