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

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator,SEO")]
    public class DynamicPagesController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public DynamicPagesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index(int p = 1) {
            DynamicPagesGetQuery query = new DynamicPagesGetQuery();
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOfDynamicPageModel dynamicPages = _query.Process(query);
            return View(dynamicPages);
        }

        public ActionResult Add() {
            return View(new DynamicPageViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add([CustomizeValidator(RuleSet = DynamicPageValidationRuleSets.Default)] DynamicPageViewModel model) {
            if (ModelState.IsValid) {
                DynamicPageAddCommand command = new DynamicPageAddCommand { Model = model.Page };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {
            DynamicPageGetByIdQuery query = new DynamicPageGetByIdQuery { Id = id, IncludePageContent = true };
            DynamicPageModel d = _query.Process(query);
            return View(new DynamicPageViewModel { Page = d });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([CustomizeValidator(RuleSet = DynamicPageValidationRuleSets.Default)] DynamicPageViewModel model) {
            if (ModelState.IsValid) {
                //model.Page.Id = id;
                DynamicPageEditCommand command = new DynamicPageEditCommand { Model = model.Page };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}