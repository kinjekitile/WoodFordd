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
    public class CorporateRateCodesController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public CorporateRateCodesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }
        // GET: CorporateRateCodes
        public ActionResult Index() {
            return View(new CorporateRateCodesViewModel());
        }

        [HttpPost]
        public ActionResult Index(CorporateRateCodesViewModel viewModel, FormCollection fc) {

            if (ModelState.IsValid) {
                viewModel.AllRateCodesAvailableToCorporates = _query.Process(new RateCodeGetQuery { Filter = new RateCodeFilterModel { AvailableToCorporate = true } }).Items;


                List<int> addedRateCodes = new List<int>();
                List<int> removedRateCodes = new List<int>();

                foreach (var rateCode in viewModel.AllRateCodesAvailableToCorporates) {
                    string corporateRateCodeId = "" + rateCode.Id;
                    string checkboxName = "rateCode_" + rateCode.Id;


                    if (fc[checkboxName].Contains("true")) {
                        addedRateCodes.Add(rateCode.Id);
                    } else {
                        removedRateCodes.Add(rateCode.Id);
                    }
                }

                CorporateUpdateRateCodesCommand command = new CorporateUpdateRateCodesCommand { CorporateId = viewModel.CorporateId, AddedRateCodeIds = addedRateCodes, RemovedRateCodeIds = removedRateCodes };
                _commandBus.Submit(command);

                viewModel.Updated = true;
                viewModel.Filtered = true;

                viewModel.AllRateCodesAvailableToCorporates = _query.Process(new RateCodeGetQuery { Filter = new RateCodeFilterModel { AvailableToCorporate = true } }).Items;
                viewModel.CorporateRateCodes = _query.Process(new CorporateGetByIdQuery { Id = viewModel.CorporateId }).RateCodes.ToList();
            }
            

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Filter(CorporateRateCodesViewModel viewModel) {

            if (ModelState.IsValid) {
                viewModel.AllRateCodesAvailableToCorporates = _query.Process(new RateCodeGetQuery { Filter = new RateCodeFilterModel { AvailableToCorporate = true } }).Items;
                viewModel.CorporateRateCodes = _query.Process(new CorporateGetByIdQuery { Id = viewModel.CorporateId }).RateCodes.ToList();
                viewModel.Filtered = true;
            }

            return View("Index", viewModel);
        }
    }
}