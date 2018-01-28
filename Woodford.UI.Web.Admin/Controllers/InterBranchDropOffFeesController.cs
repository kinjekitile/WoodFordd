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

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class InterBranchDropOffFeesController : Controller
    {

        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public InterBranchDropOffFeesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        
        public ActionResult Index(int p = 1)
        {
            InterBranchDropOffFeesGetQuery query = new InterBranchDropOffFeesGetQuery { Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 } };
            ListOf<InterBranchDropOffFeeModel> fees = _query.Process(query);
            return View(fees);
        }

        public ActionResult Add() {
            return View(new InterBranchDropOffFeeViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add([CustomizeValidator(RuleSet = InterBranchDropOffFeeValidationRuleSets.Default)] InterBranchDropOffFeeViewModel model) {
            if (ModelState.IsValid) {
                
                InterBranchDropOffFeeAddCommand command = new InterBranchDropOffFeeAddCommand { Model = model.DropOff };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {
            InterBranchDropOffFeeGetByIdQuery query = new InterBranchDropOffFeeGetByIdQuery { Id = id };
            InterBranchDropOffFeeModel model = _query.Process(query);
            return View(new InterBranchDropOffFeeViewModel { DropOff = model });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([CustomizeValidator(RuleSet = InterBranchDropOffFeeValidationRuleSets.Default)] InterBranchDropOffFeeViewModel model) {
            if (ModelState.IsValid) {

                InterBranchDropOffFeeEditCommand command = new InterBranchDropOffFeeEditCommand { Model = model.DropOff };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MarkAsActive(int id, int p) {
            InterBranchDropOffFeeMarkAsCommand command = new InterBranchDropOffFeeMarkAsCommand { Id = id, MarkAs = true };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p });
        }

        [HttpPost]
        public ActionResult MarkAsInactive(int id, int p = 1) {
            InterBranchDropOffFeeMarkAsCommand command = new InterBranchDropOffFeeMarkAsCommand { Id = id, MarkAs = false };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p });
        }
    }
}