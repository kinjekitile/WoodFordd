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
    public class RateCodesController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public RateCodesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        public ActionResult Index(int p = 1) {
            RateCodeGetQuery query = new RateCodeGetQuery { Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 } };
            return View(_query.Process(query));
        }

        public ActionResult Add() {
            return View(new RateCodeViewModel());
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = RateCodeValidationRuleSets.Default)] RateCodeViewModel model) {
            if (ModelState.IsValid) {
                RateCodeAddCommand command = new RateCodeAddCommand { Model = model.Code };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {
            RateCodeGetByIdQuery query = new RateCodeGetByIdQuery { Id = id };
            RateCodeModel r = _query.Process(query);
            return View(new RateCodeViewModel { Code = r });
        }

        [HttpPost]
        public ActionResult Edit(int id, [CustomizeValidator(RuleSet = RateRuleValidationRuleSets.Default)] RateCodeViewModel model) {
            if (ModelState.IsValid) {
                RateCodeEditCommand command = new RateCodeEditCommand { Model = model.Code };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}