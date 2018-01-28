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
    public class VehicleExtrasController : Controller {

        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public VehicleExtrasController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index() {
            var extras = _query.Process(new VehicleExtrasGetQuery());
            return View(extras);
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            var extra = _query.Process(new VehicleExtraGetByIdQuery { Id = id });
            return View(new VehicleExtrasViewModel { Extra = extra });
        }

        [HttpPost]
        public ActionResult Edit(VehicleExtrasViewModel viewModel) {
            if (ModelState.IsValid) {
                _commandBus.Submit(new VehicleExtraEditCommand { Model = viewModel.Extra });
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
    }
}