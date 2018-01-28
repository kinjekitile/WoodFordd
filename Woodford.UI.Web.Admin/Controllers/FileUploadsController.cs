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
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator")]
    public class FileUploadsController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public FileUploadsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index(int p = 1) {
            FilesGetQuery query = new FilesGetQuery();
            query.Filter = new FileUploadFilterModel { IsStandalone = true };
            query.Pagination = new ListPaginationModel { ItemsPerPage = 20, CurrentPage = p };
            ListOf<FileUploadModel> files = _query.Process(query);
            return View(files);
        }

        public ActionResult Add() {
            return View(new FileUploadViewModel());
        }

        [HttpPost]
        public ActionResult Add ([CustomizeValidator(RuleSet = FileUploadValidationRuleSets.Default)] FileUploadViewModel model) {
            if (ModelState.IsValid) {

                FileUploadModel f = FileUploadModelFactory.CreateInstance(model);
                FileUploadAddCommand command = new FileUploadAddCommand { Model = f };
                _commandBus.Submit(command);
                return RedirectToAction("Index");

            }
            return View(model);
        }

        public ActionResult Edit(int id) {

            FileUploadGetByIdQuery query = new FileUploadGetByIdQuery { Id = id };
            FileUploadModel f = _query.Process(query);
            return View(new FileUploadViewModel { File = f });

        }

        [HttpPost]
        public ActionResult Edit(int id, [CustomizeValidator(RuleSet = FileUploadValidationRuleSets.Default)] FileUploadViewModel model) {
            if (ModelState.IsValid) {

                FileUploadModel f = FileUploadModelFactory.CreateInstance(model);
                f.Id = model.File.Id;
                FileUploadEditCommand command = new FileUploadEditCommand { Model = f };
                _commandBus.Submit(command);
                return RedirectToAction("Index");

            }
            return View(model);
        }
    }
}