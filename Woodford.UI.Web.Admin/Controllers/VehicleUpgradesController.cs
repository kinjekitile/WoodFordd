using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VehicleUpgradesController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public VehicleUpgradesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index(int p = 1)
        {
            VehicleUpgradesGetQuery query = new VehicleUpgradesGetQuery { Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 } };
            return View(_query.Process(query));
        }

        public ActionResult Add() {
            return View(new VehicleUpgradeViewModel());
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = VehicleUpgradeValidationRuleSets.Default)] VehicleUpgradeViewModel model) {
            if (ModelState.IsValid) {                
                VehicleUpgradeAddCommand command = new VehicleUpgradeAddCommand { Model = model.VehicleUpgrade };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id, int? p) {
            VehicleUpgradeGetByIdQuery query = new VehicleUpgradeGetByIdQuery { Id = id };
            return View(new VehicleUpgradeViewModel { VehicleUpgrade = _query.Process(query), p = p });
        }

        [HttpPost]
        public ActionResult Edit([CustomizeValidator(RuleSet = VehicleUpgradeValidationRuleSets.Default)] VehicleUpgradeViewModel model) {
            if (ModelState.IsValid) {
                VehicleUpgradeEditCommand command = new VehicleUpgradeEditCommand { Model = model.VehicleUpgrade };
                _commandBus.Submit(command);
                return RedirectToAction("Index", new { p = model.p });
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MarkAsActive(int id, int p) {
            VehicleUpgradeMarkAsCommand command = new VehicleUpgradeMarkAsCommand { Id = id, MarkAs = true };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p });
        }

        [HttpPost]
        public ActionResult MarkAsInactive(int id, int p = 1) {
            VehicleUpgradeMarkAsCommand command = new VehicleUpgradeMarkAsCommand { Id = id, MarkAs = false };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p });
        }
    }
}