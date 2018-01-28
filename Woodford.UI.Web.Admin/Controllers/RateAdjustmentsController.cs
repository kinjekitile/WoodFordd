using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands.RateAdjustments;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RateAdjustmentsController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public RateAdjustmentsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdjustmentsByBranch() {
            RateAdjustmentGetQuery query = new RateAdjustmentGetQuery { Filter = new RateAdjustmentFilterModel { AdjustmentType = RateAdjustmentType.ByBranch, ShowPastAdjustments = false } };

            RateAdjustmentListViewModel viewModel = new RateAdjustmentListViewModel();
            viewModel.Items = _query.Process(query).Items;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AdjustmentsByBranch(RateAdjustmentListViewModel viewModel) {
            RateAdjustmentGetQuery query = new RateAdjustmentGetQuery { Filter = new RateAdjustmentFilterModel { AdjustmentType = RateAdjustmentType.ByBranch, ShowPastAdjustments = viewModel.ShowPastAdjustments } };

            viewModel.Items = _query.Process(query).Items;
            return View(viewModel);
        }

        public ActionResult AdjustmentsByBranchAdd() {
            return View(new RateAdjustmentViewModel());
        }
        [HttpPost]
        public ActionResult AdjustmentsByBranchAdd([CustomizeValidator(RuleSet = RateAdjustmentValidatorRuleSets.Default)] RateAdjustmentViewModel model) {
            if (ModelState.IsValid) {
                model.RateAdjustment.AdjustmentType = RateAdjustmentType.ByBranch;

                RateAdjustmentAddCommand cmd = new RateAdjustmentAddCommand { Model = model.RateAdjustment };
                _commandBus.Submit(cmd);

                return RedirectToAction("AdjustmentsByBranch");
            }
            return View(model);
        }

        public ActionResult AdjustmentsByBranchEdit(int id) {
            RateAdjustmentGetByIdQuery query = new RateAdjustmentGetByIdQuery { Id = id };
            RateAdjustmentModel r = _query.Process(query);
            return View(new RateAdjustmentViewModel { RateAdjustment = r });
        }

        [HttpPost]
        public ActionResult AdjustmentsByBranchEdit(int id, [CustomizeValidator(RuleSet = RateAdjustmentValidatorRuleSets.Default)] RateAdjustmentViewModel model) {
            if (ModelState.IsValid) {
                model.RateAdjustment.AdjustmentType = RateAdjustmentType.ByBranch;
                RateAdjustmentUpdateCommand cmd = new RateAdjustmentUpdateCommand { Model = model.RateAdjustment };
                _commandBus.Submit(cmd);
                return RedirectToAction("AdjustmentsByBranch");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult AdjustmentsByVehicleGroup() {
            RateAdjustmentGetQuery query = new RateAdjustmentGetQuery { Filter = new RateAdjustmentFilterModel { AdjustmentType = RateAdjustmentType.ByVehicleGroup, ShowPastAdjustments = false } };

            RateAdjustmentListViewModel viewModel = new RateAdjustmentListViewModel();
            viewModel.Items = _query.Process(query).Items;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AdjustmentsByVehicleGroup(RateAdjustmentListViewModel viewModel) {
            RateAdjustmentGetQuery query = new RateAdjustmentGetQuery { Filter = new RateAdjustmentFilterModel { AdjustmentType = RateAdjustmentType.ByVehicleGroup, ShowPastAdjustments = viewModel.ShowPastAdjustments } };

            
            viewModel.Items = _query.Process(query).Items;
            return View(viewModel);
        }

        public ActionResult AdjustmentsByVehicleGroupAdd() {
            return View(new RateAdjustmentViewModel());
        }
        [HttpPost]
        public ActionResult AdjustmentsByVehicleGroupAdd([CustomizeValidator(RuleSet = RateAdjustmentValidatorRuleSets.ByVehicleGroup)] RateAdjustmentViewModel model) {
            if (ModelState.IsValid) {
                model.RateAdjustment.AdjustmentType = RateAdjustmentType.ByVehicleGroup;
                RateAdjustmentAddCommand cmd = new RateAdjustmentAddCommand { Model = model.RateAdjustment };
                _commandBus.Submit(cmd);
                
                return RedirectToAction("AdjustmentsByVehicleGroup");
            }
            return View(model);
        }

        public ActionResult AdjustmentsByVehicleGroupEdit(int id) {
            RateAdjustmentGetByIdQuery query = new RateAdjustmentGetByIdQuery { Id = id };
            RateAdjustmentModel r = _query.Process(query);
            return View(new RateAdjustmentViewModel { RateAdjustment = r });
        }

        [HttpPost]
        public ActionResult AdjustmentsByVehicleGroupEdit(int id, [CustomizeValidator(RuleSet = RateAdjustmentValidatorRuleSets.ByVehicleGroup)] RateAdjustmentViewModel model) {
            if (ModelState.IsValid) {
                model.RateAdjustment.AdjustmentType = RateAdjustmentType.ByBranch;
                RateAdjustmentUpdateCommand cmd = new RateAdjustmentUpdateCommand { Model = model.RateAdjustment };
                _commandBus.Submit(cmd);
                return RedirectToAction("AdjustmentsByVehicleGroup");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult AdjustmentsByRateCode() {
            RateAdjustmentGetQuery query = new RateAdjustmentGetQuery { Filter = new RateAdjustmentFilterModel { AdjustmentType = RateAdjustmentType.ByRateCode, ShowPastAdjustments = false } };

            RateAdjustmentListViewModel viewModel = new RateAdjustmentListViewModel();
            viewModel.Items = _query.Process(query).Items;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AdjustmentsByRateCode(RateAdjustmentListViewModel viewModel) {
            RateAdjustmentGetQuery query = new RateAdjustmentGetQuery { Filter = new RateAdjustmentFilterModel { AdjustmentType = RateAdjustmentType.ByRateCode, ShowPastAdjustments = viewModel.ShowPastAdjustments } };

            
            viewModel.Items = _query.Process(query).Items;
            return View(viewModel);
        }

        public ActionResult AdjustmentsByRateCodeAdd() {
            return View(new RateAdjustmentViewModel());
        }
        [HttpPost]
        public ActionResult AdjustmentsByRateCodeAdd([CustomizeValidator(RuleSet = RateAdjustmentValidatorRuleSets.ByVehicleGroup)] RateAdjustmentViewModel model) {
            if (ModelState.IsValid) {
                model.RateAdjustment.AdjustmentType = RateAdjustmentType.ByRateCode;
                RateAdjustmentAddCommand cmd = new RateAdjustmentAddCommand { Model = model.RateAdjustment };
                _commandBus.Submit(cmd);

                return RedirectToAction("AdjustmentsByRateCode");
            }
            return View(model);
        }

        public ActionResult AdjustmentsByRateCodeEdit(int id) {
            RateAdjustmentGetByIdQuery query = new RateAdjustmentGetByIdQuery { Id = id };
            RateAdjustmentModel r = _query.Process(query);
            return View(new RateAdjustmentViewModel { RateAdjustment = r });
        }

        [HttpPost]
        public ActionResult AdjustmentsByRateCodeEdit(int id, [CustomizeValidator(RuleSet = RateAdjustmentValidatorRuleSets.ByVehicleGroup)] RateAdjustmentViewModel model) {
            if (ModelState.IsValid) {
                model.RateAdjustment.AdjustmentType = RateAdjustmentType.ByRateCode;
                RateAdjustmentUpdateCommand cmd = new RateAdjustmentUpdateCommand { Model = model.RateAdjustment };
                _commandBus.Submit(cmd);
                return RedirectToAction("AdjustmentsByRateCode");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult AdjustmentsByDemand() {
            RateAdjustmentGetQuery query = new RateAdjustmentGetQuery { Filter = new RateAdjustmentFilterModel { AdjustmentType = RateAdjustmentType.ByDemand, ShowPastAdjustments = false } };

            RateAdjustmentListViewModel viewModel = new RateAdjustmentListViewModel();
            viewModel.Items = _query.Process(query).Items;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AdjustmentsByDemand(RateAdjustmentListViewModel viewModel) {
            RateAdjustmentGetQuery query = new RateAdjustmentGetQuery { Filter = new RateAdjustmentFilterModel { AdjustmentType = RateAdjustmentType.ByDemand, ShowPastAdjustments = viewModel.ShowPastAdjustments } };

            
            viewModel.Items = _query.Process(query).Items;
            return View(viewModel);
        }

        public ActionResult AdjustmentsByDemandAdd() {
            return View(new RateAdjustmentViewModel());
        }
        [HttpPost]
        public ActionResult AdjustmentsByDemandAdd([CustomizeValidator(RuleSet = RateAdjustmentValidatorRuleSets.ByVehicleGroup)] RateAdjustmentViewModel model) {
            if (ModelState.IsValid) {
                model.RateAdjustment.AdjustmentType = RateAdjustmentType.ByDemand;
                RateAdjustmentAddCommand cmd = new RateAdjustmentAddCommand { Model = model.RateAdjustment };
                _commandBus.Submit(cmd);

                return RedirectToAction("AdjustmentsByDemand");
            }
            return View(model);
        }

        public ActionResult AdjustmentsByDemandEdit(int id) {
            RateAdjustmentGetByIdQuery query = new RateAdjustmentGetByIdQuery { Id = id };
            RateAdjustmentModel r = _query.Process(query);
            return View(new RateAdjustmentViewModel { RateAdjustment = r });
        }

        [HttpPost]
        public ActionResult AdjustmentsByDemandEdit(int id, [CustomizeValidator(RuleSet = RateAdjustmentValidatorRuleSets.ByVehicleGroup)] RateAdjustmentViewModel model) {
            if (ModelState.IsValid) {
                model.RateAdjustment.AdjustmentType = RateAdjustmentType.ByDemand;
                RateAdjustmentUpdateCommand cmd = new RateAdjustmentUpdateCommand { Model = model.RateAdjustment };
                _commandBus.Submit(cmd);
                return RedirectToAction("AdjustmentsByDemand");
            }
            return View(model);
        }

      
    }
}