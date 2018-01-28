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
    [Authorize(Roles = "Administrator,SEO,Branch,Operations")]
    public class HomeController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public HomeController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        [HttpGet]
        public ActionResult Index() {
            DashboardViewModel viewModel = new DashboardViewModel();

            DashboardGetQuery query = new DashboardGetQuery();
            query.StartDate = new DateTime(2016, 06, 01);
            query.EndDate = DateTime.Today;
            //viewModel.Dashboard = _query.Process(query);
            viewModel.Dashboard = new DashboardModel();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(DashboardViewModel viewModel) {

            return View(viewModel);
        }
    }
}