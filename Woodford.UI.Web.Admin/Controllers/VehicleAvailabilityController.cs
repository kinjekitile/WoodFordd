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

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VehicleAvailabilityController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public VehicleAvailabilityController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index() {
            return View(new VehicleAvailabilityFilterAndUpdateModel());
        }
        [HttpPost]
        public ActionResult Index(VehicleAvailabilityFilterAndUpdateModel model, FormCollection fc) {
            if (ModelState.IsValid) {

                model.AllVehicles = getAllVehicles();

                List<int> addedVehicleIds = new List<int>();
                List<int> removedBranchVehicleIds = new List<int>();



                foreach (var vehicle in model.AllVehicles.Items) {
                    string branchVehicleVarId = "bvid_for_" + vehicle.Id;
                    string checkboxName = "vehicle_" + vehicle.Id;

                    if (string.IsNullOrEmpty(fc[branchVehicleVarId])) {
                        //no record in db so add if checked
                        if (fc[checkboxName].Contains("true")) {
                            addedVehicleIds.Add(vehicle.Id);
                        }
                    } else {
                        //has a record in db so remove if unchecked
                        if (fc[checkboxName].Contains("true") == false) {
                            int bvId = Convert.ToInt32(fc[branchVehicleVarId]);
                            removedBranchVehicleIds.Add(bvId);
                        }
                    }
                }
                BranchVehicleInsertDeleteCommand command = new BranchVehicleInsertDeleteCommand { BranchId = model.Filter.BranchId, VehicleIdsToAdd = addedVehicleIds, BranchVehicleIdsToRemove = removedBranchVehicleIds };
                _commandBus.Submit(command);


                //repopulate from db
                model.BranchVehicles = getBranchVehicles(model.Filter.BranchId);
                model.Filtered = true;
                model.Updated = true;

            }
            return View(model);
        }

        public ActionResult Filter() {
            return RedirectToAction("Index");
        }

        private ListOf<VehicleModel> getAllVehicles() {
            VehiclesGetQuery query = new VehiclesGetQuery { Filter = new VehicleFilterModel { IsArchived = false } };
            return _query.Process(query);
        }

        private ListOf<BranchVehicleModel> getBranchVehicles(int branchId) {
            BranchVehiclesGetQuery query = new BranchVehiclesGetQuery { Filter = new BranchVehicleFilterModel { BranchId = branchId } };
            return _query.Process(query);
        }

        [HttpPost]
        public ActionResult Filter([CustomizeValidator(RuleSet = VehicleAvailabilityFilterValidationRuleSets.Default)] VehicleAvailabilityFilterAndUpdateModel model) {

            if (ModelState.IsValid) {                
                model.AllVehicles = getAllVehicles();
                model.BranchVehicles = getBranchVehicles(model.Filter.BranchId);
                model.Filtered = true;
            }

            return View("Index", model);
        }

    }
}