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
    public class BranchesController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public BranchesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index(int p = 1) {
            BranchesGetQuery query = new BranchesGetQuery();
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<BranchModel> branches = _query.Process(query);
            return View(branches);
        }

        public ActionResult Add() {
            return View(new BranchViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add([CustomizeValidator(RuleSet = BranchValidationRuleSets.Default)] BranchViewModel model) {
            if (ModelState.IsValid) {

                model.Branch.BranchImage = FileUploadModelFactory.CreateInstance(model);
               
                BranchAddCommand command = new BranchAddCommand { Model = model.Branch };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {

            BranchGetByIdQuery query = new BranchGetByIdQuery { Id = id, IncludePageContent = true };
            BranchModel model = _query.Process(query);

            return View(new BranchViewModel { Branch = model });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([CustomizeValidator(RuleSet = BranchValidationRuleSets.Default)] BranchViewModel model) {
            if (ModelState.IsValid) {
                
                model.Branch.BranchImage = FileUploadModelFactory.CreateInstance(model);

                BranchEditCommand command = new BranchEditCommand { Model = model.Branch };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            } else {
                BranchGetByIdQuery query = new BranchGetByIdQuery { Id = model.Branch.Id, IncludePageContent = true };
                BranchModel b = _query.Process(query);

                model.Branch.BranchImage = b.BranchImage;
            }

            return View(model);
        }
    }
}