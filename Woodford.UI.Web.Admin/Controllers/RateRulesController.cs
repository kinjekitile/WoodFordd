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
    public class RateRulesController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public RateRulesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        public ActionResult Index(int p = 1) {
            RateRuleGetQuery query = new RateRuleGetQuery { Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 } };
            return View(_query.Process(query));
        }

        public ActionResult Add() {            
            return View(new RateRuleViewModel());
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = RateRuleValidationRuleSets.Default)] RateRuleViewModel model) {
            if (ModelState.IsValid) {
                RateRuleAddCommand command = new RateRuleAddCommand { Model = model.Rule };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {
            RateRuleGetByIdQuery query = new RateRuleGetByIdQuery { Id = id };
            RateRuleModel r = _query.Process(query);
            return View(new RateRuleViewModel { Rule = r });
        }

        [HttpPost]
        public ActionResult Edit(int id, [CustomizeValidator(RuleSet = RateRuleValidationRuleSets.Default)] RateRuleViewModel model) {
            if (ModelState.IsValid) {
                RateRuleEditCommand command = new RateRuleEditCommand { Model = model.Rule };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}