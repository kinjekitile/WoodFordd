using System.Linq;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.Code.Helpers;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    public class asklngpidsController : Controller
    {

        private ICommandBus _commandBus;
        private IQueryProcessor _query;

        public asklngpidsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        [HttpPost]
        public ActionResult SetAsExported(int id, bool exported) {
            ReservationSetAsAddedToExternalSystemCommand comm = new ReservationSetAsAddedToExternalSystemCommand();
            comm.ReservationId = id;
            comm.HasBeenAddedToExternalSystem = exported;
            _commandBus.Submit(comm);
            
            return Content("true");
        }

        [HttpGet]
        // GET: asklngpids
        public ActionResult Index(int p = 1)
        {
            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 5 };

            ReservationFilterModel filter = new ReservationFilterModel { };
            filter.IsCompletedInvoice = true;
            filter.IsQuote = false;
            filter.ExportedToExternalSystem = false;

            ReservationsGetQuery query = new ReservationsGetQuery();
            query.Pagination = pagination;
            query.Filter = filter;

            ListOf<ReservationModel> result = _query.Process(query);


            ReservationSearchViewModel viewModel = new ReservationSearchViewModel();
            viewModel.Filter = filter;
            viewModel.Pagination = pagination;
            viewModel.Items = result.Items.ToList();
            viewModel.Pagination.UrlGetParameters = filter.ToQueryString();
            viewModel.Report = new ReportModel();
            viewModel.Report.UseCurrentDateAsStartDate = true;
            viewModel.Report.DateUnitsToAdd = 1;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Details(int id) {
            var res = _query.Process(new ReservationGetByIdQuery { Id = id });
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}