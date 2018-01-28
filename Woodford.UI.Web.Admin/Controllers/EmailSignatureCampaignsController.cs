
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainServices;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator,SEO")]
    public class EmailSignatureCampaignsController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public EmailSignatureCampaignsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }



        public ActionResult Index(int p = 1) {
            EmailSignatureCampaignFilterModel filter = new EmailSignatureCampaignFilterModel();
            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

            ListOf<EmailSignatureCampaignModel> model = _query.Process(new EmailSignatureCampaignGetQuery { Filter = filter, Pagination = pagination });


            return View(model);
        }


        [HttpGet]
        public ActionResult Add() {
            EmailSignatureCampaignViewModel viewModel = new EmailSignatureCampaignViewModel();
            EmailSignatureCampaignModel model = new EmailSignatureCampaignModel();
            viewModel.Signature = model;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = EmailSignatureCampaignValidationRuleSets.Default)] EmailSignatureCampaignViewModel model) {
            //[HttpPost]
            //public ActionResult Add(EmailSignatureCampaignViewModel model) {

            if (ModelState.IsValid) {


                model.Signature.MainContentFile = FileUploadModelFactory.CreateInstance(model.MainContentImage, model.Signature.CampaignName + "MainContent", "Campaign");
                model.Signature.MainContentNarrowFile = FileUploadModelFactory.CreateInstance(model.MainContentNarrowImage, model.Signature.CampaignName + "MainContent", "Campaign");
                model.Signature.FooterContentFile = FileUploadModelFactory.CreateInstance(model.FooterContentImage, model.Signature.CampaignName + "FooterContent", "Campaign");
                model.Signature.FooterContentNarrowFile = FileUploadModelFactory.CreateInstance(model.FooterContentNarrowImage, model.Signature.CampaignName + "FooterContent", "Campaign");
                model.Signature.SidePanelContentFile = FileUploadModelFactory.CreateInstance(model.SideContentImage, model.Signature.CampaignName + "SideContent", "Campaign");
                model.Signature.UnderlineContentFile = FileUploadModelFactory.CreateInstance(model.UnderlineContentImage, model.Signature.CampaignName + "SideContent", "Campaign");
                _commandBus.Submit(new EmailSignatureCampaignAddCommand { Model = model.Signature });


                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Edit(int id) {
            var model = _query.Process(new EmailSignatureCampaignGetByIdQuery { Id = id });
            EmailSignatureCampaignViewModel viewModel = new EmailSignatureCampaignViewModel();
            viewModel.Signature = model;
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Edit([CustomizeValidator(RuleSet = EmailSignatureCampaignValidationRuleSets.Default)] EmailSignatureCampaignViewModel viewModel) {


            if (ModelState.IsValid) {

                viewModel.Signature.MainContentFile = FileUploadModelFactory.CreateInstance(viewModel.MainContentImage, viewModel.Signature.CampaignName + "MainContent", "Campaign");
                viewModel.Signature.MainContentNarrowFile = FileUploadModelFactory.CreateInstance(viewModel.MainContentNarrowImage, viewModel.Signature.CampaignName + "MainContent", "Campaign");
                viewModel.Signature.FooterContentFile = FileUploadModelFactory.CreateInstance(viewModel.FooterContentImage, viewModel.Signature.CampaignName + "FooterContent", "Campaign");
                viewModel.Signature.FooterContentNarrowFile = FileUploadModelFactory.CreateInstance(viewModel.FooterContentNarrowImage, viewModel.Signature.CampaignName + "FooterContent", "Campaign");
                viewModel.Signature.SidePanelContentFile = FileUploadModelFactory.CreateInstance(viewModel.SideContentImage, viewModel.Signature.CampaignName + "SideContent", "Campaign");
                viewModel.Signature.UnderlineContentFile = FileUploadModelFactory.CreateInstance(viewModel.UnderlineContentImage, viewModel.Signature.CampaignName + "SideContent", "Campaign");

                _commandBus.Submit(new EmailSignatureCampaignEditCommand { Model = viewModel.Signature });
                return RedirectToAction("Index");
            } else {
                var model = _query.Process(new EmailSignatureCampaignGetByIdQuery { Id = viewModel.Signature.Id });
                viewModel.Signature.MainContentFile = model.MainContentFile;
                viewModel.Signature.MainContentNarrowFile = model.MainContentNarrowFile;
                viewModel.Signature.SidePanelContentFile = model.SidePanelContentFile;
                viewModel.Signature.FooterContentFile = model.FooterContentFile;
                viewModel.Signature.FooterContentNarrowFile = model.FooterContentNarrowFile;
                viewModel.Signature.UnderlineContentFile = model.UnderlineContentFile;
            }

            return View(viewModel);
        }
    }
}