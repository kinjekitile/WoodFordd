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
    public class CampaignsController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public CampaignsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        public ActionResult Index(int p = 1) {
            CampaignGetQuery query = new CampaignGetQuery();
            query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };
            ListOf<CampaignModel> campaigns = _query.Process(query);
            return View(campaigns);
        }

        public ActionResult Add() {
            return View(new CampaignViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add([CustomizeValidator(RuleSet = CampaignValidatorRuleSets.Default)] CampaignViewModel model) {
            if (ModelState.IsValid) {
                model.Campaign.CampaignImage= FileUploadModelFactory.CreateInstance(model);

                model.Campaign.SearchResultIconImage = FileUploadModelFactory.CreateInstanceSearchResult(model);

                CampaignAddCommand command = new CampaignAddCommand { Model = model.Campaign };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id) {
            CampaignGetByIdQuery query = new CampaignGetByIdQuery { Id = id, includePageContent = true };
            CampaignViewModel c = new CampaignViewModel { Campaign = _query.Process(query) };

            return View(c);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([CustomizeValidator(RuleSet = CampaignValidatorRuleSets.Default)] CampaignViewModel model) {
            if (ModelState.IsValid) {

                model.Campaign.CampaignImage = FileUploadModelFactory.CreateInstance(model);
                model.Campaign.SearchResultIconImage = FileUploadModelFactory.CreateInstanceSearchResult(model);

                CampaignEditCommand command = new CampaignEditCommand { Model = model.Campaign };
                _commandBus.Submit(command);
                return RedirectToAction("Index");
            } else {
                CampaignGetByIdQuery query = new CampaignGetByIdQuery { Id = model.Campaign.Id, includePageContent = true };
                CampaignModel c = _query.Process(query);
                model.Campaign.CampaignImage = c.CampaignImage;
                model.Campaign.SearchResultIconImage = c.SearchResultIconImage;
            }
            return View(model);
        }
    }
}