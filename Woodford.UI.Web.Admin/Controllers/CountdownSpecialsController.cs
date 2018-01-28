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

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CountdownSpecialsController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public CountdownSpecialsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }


        public ActionResult Index(int p = 1) {
            CountdownSpecialsGetQuery query = new CountdownSpecialsGetQuery { Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 } };
            return View(_query.Process(query));
        }

        [HttpPost]
        public ActionResult MarkAsActive(int id, int p) {
            CountdownSpecialMarkAsCommand command = new CountdownSpecialMarkAsCommand { Id = id, MarkAs = true };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p });
        }

        [HttpPost]
        public ActionResult MarkAsInactive(int id, int p = 1) {
            CountdownSpecialMarkAsCommand command = new CountdownSpecialMarkAsCommand { Id = id, MarkAs = false };
            _commandBus.Submit(command);
            return RedirectToAction("Index", new { p = p });
        }

        public ActionResult Add() {
            return View(new CountdownSpecialViewModel());
        }
        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = CountdownSpecialValidationRuleSets.Default)] CountdownSpecialViewModel model) {
            if (ModelState.IsValid) {
                CountdownSpecialAddCommand command = new CountdownSpecialAddCommand { Model = model.CountdownSpecial };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id, int? p) {
            CountdownSpecialGetByIdQuery query = new CountdownSpecialGetByIdQuery { Id = id };
            return View(new CountdownSpecialViewModel { CountdownSpecial = _query.Process(query), p = p });
        }

        [HttpPost]
        public ActionResult Edit([CustomizeValidator(RuleSet = CountdownSpecialValidationRuleSets.Default)] CountdownSpecialViewModel model) {
            if (ModelState.IsValid) {
                CountdownSpecialEditCommand command = new CountdownSpecialEditCommand { Model = model.CountdownSpecial };
                _commandBus.Submit(command);
                return RedirectToAction("Index", new { p = model.p });
            }
            return View(model);
        }
    }
}