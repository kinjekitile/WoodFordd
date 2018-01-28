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
    public class VehiclesController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public VehiclesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index(int p = 1) {
            VehiclesGetQuery query = new VehiclesGetQuery();
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<VehicleModel> vehicles = _query.Process(query);
            vehicles.Items = vehicles.Items.OrderBy(x => x.VehicleGroup.SortOrder).ThenBy(x => x.SortOrder).ToList();
            return View(vehicles);
        }

        public ActionResult Add() {
            return View(new VehicleViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add([CustomizeValidator(RuleSet = VehicleValidationRuleSets.Default)] VehicleViewModel model) {
            if (ModelState.IsValid) {                

                //if (model.VehicleImage != null) {
                //    MemoryStream target = new MemoryStream();
                //    model.VehicleImage.InputStream.CopyTo(target);
                //    byte[] fileContent = target.ToArray();

                //    model.Vehicle.VehicleImage = new FileUploadModel {
                //        Title = Utilities.GenerateSlug(model.Vehicle.Title),
                //        FileContents = fileContent,
                //        FileExtension = Path.GetExtension(model.VehicleImage.FileName).ToLower(),
                //        FileContext = "Vehicles"
                //    };
                //}

                model.Vehicle.VehicleImage = FileUploadModelFactory.CreateInstance(model, false);
                model.Vehicle.VehicleImage2 = FileUploadModelFactory.CreateInstance(model, true);

                //if (model.VehicleImage2 != null) {
                //    MemoryStream target2 = new MemoryStream();
                //    model.VehicleImage2.InputStream.CopyTo(target2);
                //    byte[] fileContent2 = target2.ToArray();

                //    model.Vehicle.VehicleImage2 = new FileUploadModel {
                //        Title = Utilities.GenerateSlug(model.Vehicle.Title + "-interior"),
                //        FileContents = fileContent2,
                //        FileExtension = Path.GetExtension(model.VehicleImage2.FileName).ToLower(),
                //        FileContext = "Vehicles"
                //    };
                //}

                VehicleAddCommand command = new VehicleAddCommand { Model = model.Vehicle };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {
            VehicleGetByIdQuery query = new VehicleGetByIdQuery { Id = id, includePageContent = true };
            VehicleViewModel v = new VehicleViewModel { Vehicle = _query.Process(query) };

            return View(v);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, [CustomizeValidator(RuleSet = VehicleValidationRuleSets.Default)]VehicleViewModel model) {
            if (ModelState.IsValid) {

                //if (model.VehicleImage != null) {
                //    MemoryStream target = new MemoryStream();
                //    model.VehicleImage.InputStream.CopyTo(target);
                //    byte[] fileContent = target.ToArray();

                //    model.Vehicle.VehicleImage = new FileUploadModel {
                //        Title = Utilities.GenerateSlug(model.Vehicle.Title),
                //        FileContents = fileContent,
                //        FileExtension = Path.GetExtension(model.VehicleImage.FileName).ToLower(),
                //        FileContext = "Vehicles"
                //    };
                //}

                //if (model.VehicleImage2 != null) {
                //    MemoryStream target2 = new MemoryStream();
                //    model.VehicleImage2.InputStream.CopyTo(target2);
                //    byte[] fileContent2 = target2.ToArray();

                //    model.Vehicle.VehicleImage2 = new FileUploadModel {
                //        Title = Utilities.GenerateSlug(model.Vehicle.Title + "-interior"),
                //        FileContents = fileContent2,
                //        FileExtension = Path.GetExtension(model.VehicleImage2.FileName).ToLower(),
                //        FileContext = "Vehicles"
                //    };
                //}

                model.Vehicle.VehicleImage = FileUploadModelFactory.CreateInstance(model, false);
                model.Vehicle.VehicleImage2 = FileUploadModelFactory.CreateInstance(model, true);

                VehicleEditCommand command = new VehicleEditCommand { Model = model.Vehicle };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            } else {
                //reset the image objects
                VehicleGetByIdQuery query = new VehicleGetByIdQuery { Id = id, includePageContent = true };
                VehicleModel v = _query.Process(query);

                model.Vehicle.VehicleImage = v.VehicleImage;
                model.Vehicle.VehicleImage2 = v.VehicleImage2;
            }
            return View(model);
        }
    }
}