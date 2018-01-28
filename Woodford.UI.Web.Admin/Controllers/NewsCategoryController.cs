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
    [Authorize(Roles = "Administrator,SEO")]
    public class NewsCategoryController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public NewsCategoryController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        public ActionResult Index(int p = 1) {
            NewsCategoryGetQuery query = new NewsCategoryGetQuery();
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<NewsCategoryModel> cats = _query.Process(query);
            return View(cats);
        }

        public ActionResult Add() {
            return View(new NewsCategoryViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add([CustomizeValidator(RuleSet = NewsCategoryValidationRuleSets.Default)] NewsCategoryViewModel model) {
            if (ModelState.IsValid) {
                
                NewsCategoryAddCommand command = new NewsCategoryAddCommand { Model = model.Category };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {

            NewsCategoryGetByIdQuery query = new NewsCategoryGetByIdQuery { Id = id, IncludeArticles = false };
            NewsCategoryModel model = _query.Process(query);

            return View(new NewsCategoryViewModel { Category = model });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([CustomizeValidator(RuleSet = BranchValidationRuleSets.Default)] NewsCategoryViewModel model) {
            if (ModelState.IsValid) {
                
                NewsCategoryEditCommand command = new NewsCategoryEditCommand { Model = model.Category };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult MarkAsArchived(int id, int p) {
            NewsCategoryMarkAsCommand command = new NewsCategoryMarkAsCommand { Id = id, MarkAsArchived = true };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p });
        }

        [HttpPost]
        public ActionResult MarkAsActive(int id, int p = 1) {
            NewsCategoryMarkAsCommand command = new NewsCategoryMarkAsCommand { Id = id, MarkAsArchived = false };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p });
        }
    }
}