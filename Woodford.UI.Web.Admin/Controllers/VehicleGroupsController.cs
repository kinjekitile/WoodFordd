using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator,SEO")]
    public class VehicleGroupsController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public VehicleGroupsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        public ActionResult Index(int p = 1)
        {
            VehicleGroupsGetQuery query = new VehicleGroupsGetQuery();
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<VehicleGroupModel> vehicleGroups = _query.Process(query);
            vehicleGroups.Items = vehicleGroups.Items.OrderBy(x => x.SortOrder).ToList();
            return View(vehicleGroups);
        }

        public ActionResult Add() {
            return View(new VehicleGroupViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add([CustomizeValidator(RuleSet = VehicleGroupValidationRuleSets.Default)] VehicleGroupViewModel model) {
            if (ModelState.IsValid) {                
                model.VehicleGroup.VehicleGroupImage = FileUploadModelFactory.CreateInstance(model);

                VehicleGroupAddCommand command = new VehicleGroupAddCommand { Model = model.VehicleGroup};
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {
            VehicleGroupGetByIdQuery query = new VehicleGroupGetByIdQuery { Id = id, includePageContent = true };
            VehicleGroupViewModel vg = new VehicleGroupViewModel { VehicleGroup = _query.Process(query) };

            return View(vg);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, [CustomizeValidator(RuleSet = VehicleGroupValidationRuleSets.Default)] VehicleGroupViewModel model) {
            if (ModelState.IsValid) {                

                model.VehicleGroup.VehicleGroupImage = FileUploadModelFactory.CreateInstance(model);
                
                VehicleGroupEditCommand command = new VehicleGroupEditCommand { Model = model.VehicleGroup };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            } else {
                VehicleGroupGetByIdQuery query = new VehicleGroupGetByIdQuery { Id = id, includePageContent = true };
                VehicleGroupModel vg = _query.Process(query);
                model.VehicleGroup.VehicleGroupImage = vg.VehicleGroupImage;
            }
            return View(model);
        }
    }
}