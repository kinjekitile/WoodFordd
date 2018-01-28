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

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator,SEO")]
    public class VehicleManufacturersController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public VehicleManufacturersController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }


        public ActionResult Index(int p = 1) {
            VehicleManufacturerGetQuery query = new VehicleManufacturerGetQuery();
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<VehicleManufacturerModel> vehicleManufacturers = _query.Process(query);
            vehicleManufacturers.Items = vehicleManufacturers.Items.OrderBy(x => x.SortOrder).ToList();
            return View(vehicleManufacturers);
        }

        public ActionResult Add() {
            return View(new VehicleManufacturerViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add([CustomizeValidator(RuleSet = VehicleManufacturerValidationRuleSets.Default)] VehicleManufacturerViewModel model) {
            if (ModelState.IsValid) {

                model.Manufacturer.ManufacturerImage= FileUploadModelFactory.CreateInstance(model);

                VehicleManufacturerAddCommand command = new VehicleManufacturerAddCommand { Manufacturer = model.Manufacturer };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {
            VehicleManufacturerGetByIdQuery query = new VehicleManufacturerGetByIdQuery { Id = id, includePageContent = true };
            VehicleManufacturerViewModel vg = new VehicleManufacturerViewModel { Manufacturer = _query.Process(query) };

            return View(vg);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, [CustomizeValidator(RuleSet = VehicleManufacturerValidationRuleSets.Default)]  VehicleManufacturerViewModel model) {
            if (ModelState.IsValid) {

                model.Manufacturer.ManufacturerImage= FileUploadModelFactory.CreateInstance(model);

                VehicleManufacturerEditCommand command = new VehicleManufacturerEditCommand { Manufacturer = model.Manufacturer };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            else {
                VehicleManufacturerGetByIdQuery query = new VehicleManufacturerGetByIdQuery { Id = id, includePageContent = true };
                VehicleManufacturerModel vg = _query.Process(query);
                
            }
            return View(model);
        }
    }
}