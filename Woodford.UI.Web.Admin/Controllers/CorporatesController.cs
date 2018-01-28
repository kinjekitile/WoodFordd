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
    public class CorporatesController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public CorporatesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        // GET: Corporates
        public ActionResult Index(int p = 1) {
            CorporatesGetQuery query = new CorporatesGetQuery();
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<CorporateModel> corps = _query.Process(query);
            return View(corps);
        }

        [HttpGet]
        public ActionResult Add() {
            return View(new CorporateViewModel());
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = CorporateValidationRuleSets.Default)] CorporateViewModel viewModel) {

            if (ModelState.IsValid) {
                CorporateAddCommand command = new CorporateAddCommand { Model = viewModel.Corporate };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            CorporateGetByIdQuery query = new CorporateGetByIdQuery { Id = id };
            CorporateModel model = _query.Process(query);

            return View(new CorporateViewModel { Corporate = model });
        }

        [HttpPost]
        public ActionResult Edit([CustomizeValidator(RuleSet = CorporateValidationRuleSets.Default)] CorporateViewModel viewModel) {
            if (ModelState.IsValid) {
                CorporateEditCommand command = new CorporateEditCommand { Model = viewModel.Corporate };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }


        [HttpGet]
        public ActionResult Detail(int id) {
            CorporateGetByIdQuery query = new CorporateGetByIdQuery { Id = id };
            CorporateModel corporate = _query.Process(query);
            CorporateViewModel viewModel = new CorporateViewModel { Corporate = corporate };
            viewModel.Users = _query.Process(new UsersGetQuery { Filter = new UserFilterModel { CorporateId = corporate.Id } });
            viewModel.CorporateRateCodes = _query.Process(new CorporateGetByIdQuery { Id = corporate.Id }).RateCodes.ToList();


            return View(viewModel);
        }

    }
}