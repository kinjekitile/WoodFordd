using FluentValidation.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
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
using Woodford.UI.Web.Admin.Code.Helpers;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers {
    [Authorize(Roles = "Administrator,Operations")]
    public class ReservationsController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;
        private const int recordsPerPage = 50;

        public ReservationsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }


        public ActionResult Index(int p = 1) {

            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = recordsPerPage };

            ReservationFilterModel filter = new ReservationFilterModel { };
            filter.IsCompletedInvoice = true;
            filter.IsQuote = false;

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

        public FileResult ExportAsExcel(ReservationFilterModel viewModel) {

            if (viewModel.PickupBranchId == 0) {
                viewModel.PickupBranchId = null;
            }

            if (viewModel.RateCodeId == 0) {
                viewModel.RateCodeId = null;
            }

            if (viewModel.CorporateId == 0) {
                viewModel.CorporateId = null;
            }

            ReservationsGetQuery query = new ReservationsGetQuery();

            query.Pagination = null;
            query.Filter = viewModel;


            ListOf<ReservationModel> result = _query.Process(query);

            //viewModel.Pagination = null;
            //viewModel.Items = result.Items.OrderByDescending(x => x.Id).ToList();
            byte[] fileBytes;

            using (ExcelPackage package = new ExcelPackage()) {
                var ws = package.Workbook.Worksheets.Add("report");

                //Add report data

                ws.Cells[1, 1].Value = "Id";
                ws.Cells[1, 2].Value = "User";
                ws.Cells[1, 3].Value = "Booking Date";
                ws.Cells[1, 4].Value = "Collection Date";
                ws.Cells[1, 5].Value = "Return Branch";
                ws.Cells[1, 6].Value = "Vehicle";
                ws.Cells[1, 7].Value = "Rate Code";
                ws.Cells[1, 8].Value = "Total";
                ws.Cells[1, 9].Value = "Points Spent";
                ws.Cells[1, 10].Value = "Amount Paid";

                //Shared Styles
                var dateStyle = package.Workbook.Styles.CreateNamedStyle("Date");
                dateStyle.Style.Numberformat.Format = "dd-MM-yyyy";

                var currencyStyle = package.Workbook.Styles.CreateNamedStyle("Currency");
                currencyStyle.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";


                int rowCount = 2;
                foreach (var item in result.Items) {
                    //Add fields

                    ws.Cells[rowCount, 1].Value = item.Id;
                    if (item.UserId.HasValue) {
                        ws.Cells[rowCount, 2].Value = item.User.FirstName + " " + item.User.LastName;
                    }
                    else {
                        ws.Cells[rowCount, 2].Value = item.FirstName + " " + item.LastName;
                    }

                    ws.Cells[rowCount, 3].Value = item.DateCreated;
                    ws.Cells[rowCount, 3].StyleName = "Date";

                    ws.Cells[rowCount, 4].Value = item.PickupDate; //"Collection Date";
                    ws.Cells[rowCount, 4].StyleName = "Currency";

                    ws.Cells[rowCount, 5].Value = item.DropOffBranch.Title; //"Return Branch";
                    ws.Cells[rowCount, 6].Value = item.Vehicle.Title; //"Vehicle";
                    ws.Cells[rowCount, 7].Value = item.RateCodeTitle; //"Rate Code";
                    ws.Cells[rowCount, 8].Value = item.BookingPrice; //"Total";
                    ws.Cells[rowCount, 8].StyleName = "Currency";
                    if (item.LoyaltyPointsSpent.HasValue) {
                        ws.Cells[rowCount, 9].Value = item.LoyaltyPointsSpent.Value; //"Loyalty Points Spent"; 
                        ws.Cells[rowCount, 9].StyleName = "Currency";
                    }
                    if (item.Invoice != null) {
                        ws.Cells[rowCount, 10].Value = item.Invoice.AmountPaid; //"Amount Paid"; 
                        ws.Cells[rowCount, 10].StyleName = "Currency";
                    }



                    rowCount++;
                }


                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                fileBytes = package.GetAsByteArray();
            }



            string fileName = "Reservations Report - " + DateTime.Now.ToString("dd MMM yyyy HH:mm") + ".xlsx";


            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }


        public ActionResult SaveReport([CustomizeValidator(RuleSet = ReservationValidationRuleSets.Report)] ReservationSearchViewModel viewModel) {

            if (viewModel.Filter.PickupBranchId == 0) {
                viewModel.Filter.PickupBranchId = null;
            }

            if (viewModel.Filter.RateCodeId == 0) {
                viewModel.Filter.RateCodeId = null;
            }

            if (viewModel.Filter.CorporateId == 0) {
                viewModel.Filter.CorporateId = null;
            }

            if (ModelState.IsValid) {
                ReportAddCommand addReport = new ReportAddCommand();
                addReport.Model = viewModel.Report;
                addReport.Model.ReportType = ReportType.Reservation;
                addReport.Model.ReservationFilter = viewModel.Filter;
                addReport.Model.ReportFilter = viewModel.Filter.ToQueryString();


                _commandBus.Submit(addReport);
                return RedirectToAction("Index", "Reports");


            }
            else {


                ListPaginationModel pagination = new ListPaginationModel { CurrentPage = 1, ItemsPerPage = 20 };
                ReservationsGetQuery query = new ReservationsGetQuery();

                query.Pagination = pagination;
                query.Filter = viewModel.Filter;


                ListOf<ReservationModel> result = _query.Process(query);

                viewModel.Pagination = pagination;
                viewModel.Items = result.Items.OrderByDescending(x => x.Id).ToList();

                return View("Index", viewModel);
            }


        }

        public ActionResult Filter([CustomizeValidator(RuleSet = ReservationValidationRuleSets.Search)] ReservationSearchViewModel viewModel, int p = 1) {


            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = recordsPerPage };

            if (viewModel.Filter.PickupBranchId == 0) {
                viewModel.Filter.PickupBranchId = null;
            }

            if (viewModel.Filter.RateCodeId == 0) {
                viewModel.Filter.RateCodeId = null;
            }

            if (viewModel.Filter.CorporateId == 0) {
                viewModel.Filter.CorporateId = null;
            }

            ReservationsGetQuery query = new ReservationsGetQuery();

            if (ModelState.IsValid) {

                query.Pagination = pagination;
                query.Filter = viewModel.Filter;


                ListOf<ReservationModel> result = _query.Process(query);

                viewModel.Pagination = pagination;
                viewModel.Items = result.Items.OrderByDescending(x => x.Id).ToList();

            }
            else {


                query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

                query.Filter = new ReservationFilterModel { };
                query.Filter.ReservationState = ReservationState.Completed;
                ListOf<ReservationModel> result = _query.Process(query);
            }

            viewModel.Pagination.UrlGetParameters = query.Filter.ToQueryString();
            viewModel.Pagination.UrlAction = "/Reservations/Filter";
            viewModel.Report = new ReportModel();
            viewModel.Report.UseCurrentDateAsStartDate = true;
            viewModel.Report.DateUnitsToAdd = 1;
            return View("Index", viewModel);
        }

        [HttpPost]
        public ActionResult GoToReservation(ReservationSearchViewModel viewModel) {
            int id = viewModel.Filter.Id.Value;
            return RedirectToAction("Details", new { id = id });
        }
        public ActionResult Details(int id) {
            ReservationViewModel viewModel = new ReservationViewModel();
            //viewModel.Reservation = _query.Process(new ReservationsGetQuery { Filter = new ReservationFilterModel { Id = id }, Pagination = null }).Items.First();

            viewModel.Reservation = _query.Process(new ReservationGetByIdQuery { Id = id });

            return View(viewModel);
        }

        public ActionResult Print(int id) {

            ReservationInvoiceNotificationModel viewModel = new ReservationInvoiceNotificationModel();
            //viewModel.Reservation = _query.Process(new ReservationsGetQuery { Filter = new ReservationFilterModel { Id = id }, Pagination = null }).Items.First();
            viewModel.Reservation = _query.Process(new ReservationGetByIdQuery { Id = id });
            viewModel.Invoice = viewModel.Reservation.Invoice;
            if (viewModel.Invoice == null) {
                viewModel.Invoice = new InvoiceModel();
                viewModel.Reservation.Invoice = new InvoiceModel();
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            ReservationViewModel viewModel = new ReservationViewModel();
            viewModel.Reservation = _query.Process(new ReservationGetByIdQuery { Id = id });
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(ReservationViewModel viewModel) {
            if (ModelState.IsValid) {

            }
            return View(viewModel);
        }

        public ActionResult AddedToExternalSystem(int id, bool addedToExternalSystem) {
            _commandBus.Submit(new ReservationSetAsAddedToExternalSystemCommand { ReservationId = id, HasBeenAddedToExternalSystem = addedToExternalSystem });

            return Json(new { status = "success" });
        }


        public ActionResult SearchUsers() {
            throw new NotImplementedException();
        }

        public ActionResult SearchUsersFilter(ReservationSearchUsersViewModel viewModel, int p = 1) {

            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = recordsPerPage };

            if (viewModel.Filter.BranchId == 0) {
                viewModel.Filter.BranchId = null;
            }

            if (viewModel.Filter.VehicleGroupId == 0) {
                viewModel.Filter.VehicleGroupId = null;
            }



            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Cancel(int id) {
            var model = _query.Process(new ReservationGetByIdQuery { Id = id });
            return View(model);
        }

        [HttpPost]
        public ActionResult Cancel(ReservationModel model) {

            ReservationCancelCommand cancel = new ReservationCancelCommand();
            cancel.ReservationId = model.Id;
            cancel.IsCancelled = model.IsCancelled;

            _commandBus.Submit(cancel);

            switch (cancel.OutCancelStatus) {
                case ReservationCancelResponseState.Failed:
                    ModelState.AddModelError("", "Reservation could not be cancelled");
                    break;
                case ReservationCancelResponseState.NoLeadTime:
                    ModelState.AddModelError("", "Reservation could not be cancelled: Not enough lead time to cancel.");
                    break;
                case ReservationCancelResponseState.AlreadyCancelled:
                    ModelState.AddModelError("", "Reservation has already been cancelled.");
                    break;
                case ReservationCancelResponseState.Success:
                    return RedirectToAction("Details", new { id = model.Id });
            }
            model = _query.Process(new ReservationGetByIdQuery { Id = model.Id });
            return View(model);
        }

        [HttpGet]
        public ActionResult Archive(int id) {
            var model = _query.Process(new ReservationGetByIdQuery { Id = id });
            return View(model);
        }

        [HttpPost]
        public ActionResult Archive(ReservationModel model) {
            _commandBus.Submit(new ReservationArchiveCommand { ReservationId = model.Id, IsArchived = model.IsCancelled });
            return RedirectToAction("Details", new { id = model.Id });
        }
    }
}