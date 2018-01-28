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
    [Authorize(Roles = "Administrator,SEO")]
    public class UrlRedirectsController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public UrlRedirectsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        [HttpGet]
        public ActionResult Index(int p = 1) {
            UrlRedirectSearchModel viewModel = new UrlRedirectSearchModel();

            UrlRedirectGetQuery query = new UrlRedirectGetQuery();
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<UrlRedirectModel> redirects = _query.Process(query);
            viewModel.Result = redirects;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(UrlRedirectSearchModel viewModel, int p = 1) {

            UrlRedirectGetQuery query = new UrlRedirectGetQuery { Filter = viewModel.Filter, Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 } };
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<UrlRedirectModel> redirects = _query.Process(query);
            viewModel.Result = redirects;
            return View(viewModel);
        }

        public ActionResult Add() {
            return View(new UrlRedirectViewModel());
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = UrlRedirectValidationRuleSets.Default)] UrlRedirectViewModel viewModel) {
            if (ModelState.IsValid) {
                UrlRedirectAddCommand command = new UrlRedirectAddCommand { Model = viewModel.Redirect };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        } 

        public ActionResult Edit(int id) {
            UrlRedirectGetByIdQuery query = new UrlRedirectGetByIdQuery { Id = id };
            UrlRedirectModel model = _query.Process(query);
            return View(new UrlRedirectViewModel { Redirect = model });
        }

        [HttpPost]
        public ActionResult Edit([CustomizeValidator(RuleSet = UrlRedirectValidationRuleSets.Default)] UrlRedirectViewModel viewModel) {
            if (ModelState.IsValid) {
                UrlRedirectEditCommand command = new UrlRedirectEditCommand { Model = viewModel.Redirect };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public ActionResult Delete(int id) {
            UrlRedirectGetByIdQuery query = new UrlRedirectGetByIdQuery { Id = id };
            UrlRedirectModel model = _query.Process(query);
            return View(new UrlRedirectViewModel { Redirect = model });
        }

        [HttpPost]
        public ActionResult Delete([CustomizeValidator(RuleSet = UrlRedirectValidationRuleSets.Default)] UrlRedirectViewModel viewModel) {


            if (ModelState.IsValid) {
                UrlRedirectDeleteCommand deleteCommand = new UrlRedirectDeleteCommand { Id = viewModel.Redirect.Id };
                _commandBus.Submit(deleteCommand);
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
    }
}