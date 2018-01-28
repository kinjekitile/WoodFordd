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
using Woodford.Core.ApplicationServices.Queries.Reviews;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.Code;
using Woodford.UI.Web.Admin.Code.Helpers;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator")]
    public class ReviewInvitesController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public ReviewInvitesController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        public ActionResult Index(int p = 1) {

            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

            ReviewFilterModel filter = new ReviewFilterModel();


            ReviewGetQuery query = new ReviewGetQuery();
            query.Filter = filter;
            query.Pagination = pagination;

            var items = _query.Process(query);

            ReviewSearchViewModels viewModel = new ReviewSearchViewModels();
            viewModel.Filter = filter;
            viewModel.Pagination = pagination;
            viewModel.Items = items.Items;

            return View(viewModel);
        }

        public ActionResult Filter(ReviewSearchViewModels viewModel, int p = 1) {
            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

            ReviewGetQuery query = new ReviewGetQuery();

            query.Pagination = pagination;
            query.Filter = viewModel.Filter;

            var result = _query.Process(query);

            viewModel.Pagination = pagination;
            viewModel.Items = result.Items.OrderByDescending(x => x.Id).ToList();

            //if (ModelState.IsValid) {

            //    query.Pagination = pagination;
            //    query.Filter = viewModel.Filter;

            //    var result = _query.Process(query);

            //    viewModel.Pagination = pagination;
            //    viewModel.Items = result.Items.OrderByDescending(x => x.Id).ToList();

            //} else {
            //    query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

            //}

            viewModel.Pagination.UrlGetParameters = query.Filter.ToQueryString();
            viewModel.Pagination.UrlAction = "/ReviewInvites/Filter";
            return View("Index", viewModel);
        }
    }
}