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
    [Authorize(Roles = "Administrator")]
    public class ReportsController : Controller {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public ReportsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }


        public ActionResult Index(int p = 1) {

            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

            ReportFilterModel filter = new ReportFilterModel { };


            ReportsGetQuery query = new ReportsGetQuery();
            query.Pagination = null;
            query.Filter = filter;

            ListOf<ReportModel> result = _query.Process(query);


            ReportSearchViewModel viewModel = new ReportSearchViewModel();
            viewModel.Filter = filter;
            viewModel.Pagination = null;
            viewModel.Items = result.Items.ToList();
            foreach (var item in viewModel.Items) {
                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today.AddDays(1);

                if (item.UseCurrentDateAsStartDate) {
                    if (item.DateUnitType.HasValue) {

                        if (item.ReportType == ReportType.Reservation) {
                            switch (item.DateUnitType.Value) {
                                case ReportDateUnitType.Day:
                                    endDate = startDate.AddDays(item.DateUnitsToAdd.Value);
                                    break;
                                case ReportDateUnitType.Month:
                                    endDate = startDate.AddMonths(item.DateUnitsToAdd.Value);
                                    break;
                                case ReportDateUnitType.Year:
                                    endDate = startDate.AddYears(item.DateUnitsToAdd.Value);
                                    break;
                            }
                        }

                        if (item.ReportType == ReportType.User) {
                            if (item.DateUnitsToAdd.Value > 0) {
                                item.DateUnitsToAdd = item.DateUnitsToAdd.Value * -1;
                            }
                            switch (item.DateUnitType.Value) {
                                case ReportDateUnitType.Day:
                                    endDate = startDate.AddDays(item.DateUnitsToAdd.Value);
                                    break;
                                case ReportDateUnitType.Month:
                                    endDate = startDate.AddMonths(item.DateUnitsToAdd.Value);
                                    break;
                                case ReportDateUnitType.Year:
                                    endDate = startDate.AddYears(item.DateUnitsToAdd.Value);
                                    break;
                            }
                        }

                    }
                    
                }

                if (item.ReportType == ReportType.Reservation) {
                    
                    item.ReservationFilter.DateSearchStart = startDate;
                    item.ReservationFilter.DateSearchEnd = endDate;
                    if (item.ReservationFilter.DateFilterType == ReservationDateFilterTypes.None) {
                        item.ReservationFilter.DateFilterType = ReservationDateFilterTypes.PickupDate;
                    }
                    item.ReportFilterUrl = item.ReservationFilter.ToQueryString();
                }
                if (item.ReportType == ReportType.User) {

                    item.UserFilter.DateCreatedStart = endDate;
                    item.UserFilter.DateCreatedEnd = startDate;

                    //if (item.UserFilter.DateFilterType == UserDateFilterTypes.None) {
                    //    item.UserFilter.DateFilterType = UserDateFilterTypes.None;
                    //}
                    item.ReportFilterUrl = item.UserFilter.ToQueryString();
                }
            }
            //viewModel.Pagination.UrlGetParameters = filter.ToQueryString();

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Delete(int id) {


            ReportModel model = _query.Process(new ReportGetByIdQuery { Id = id });


            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(ReportModel model) {


            _commandBus.Submit(new ReportDeleteCommand { Id = model.Id });


            return RedirectToAction("Index");
        }

        //public FileResult ExportAsExcel(ReservationFilterModel viewModel) {

        //    if (viewModel.PickupBranchId == 0) {
        //        viewModel.PickupBranchId = null;
        //    }

        //    if (viewModel.RateCodeId == 0) {
        //        viewModel.RateCodeId = null;
        //    }

        //    if (viewModel.CorporateId == 0) {
        //        viewModel.CorporateId = null;
        //    }

        //    ReservationsGetQuery query = new ReservationsGetQuery();

        //    query.Pagination = null;
        //    query.Filter = viewModel;


        //    ListOf<ReservationModel> result = _query.Process(query);

        //    //viewModel.Pagination = null;
        //    //viewModel.Items = result.Items.OrderByDescending(x => x.Id).ToList();
        //    byte[] fileBytes;

        //    using (ExcelPackage package = new ExcelPackage()) {
        //        var ws = package.Workbook.Worksheets.Add("report");

        //        //Add report data

        //        ws.Cells[1, 1].Value = "Id";
        //        ws.Cells[1, 2].Value = "User";
        //        ws.Cells[1, 3].Value = "Booking Date";
        //        ws.Cells[1, 4].Value = "Collection Date";
        //        ws.Cells[1, 5].Value = "Return Branch";
        //        ws.Cells[1, 6].Value = "Vehicle";
        //        ws.Cells[1, 7].Value = "Rate Code";
        //        ws.Cells[1, 8].Value = "Total";
        //        ws.Cells[1, 9].Value = "Amount Paid";

        //        //Shared Styles
        //        var dateStyle = package.Workbook.Styles.CreateNamedStyle("Date");
        //        dateStyle.Style.Numberformat.Format = "dd-MM-yyyy";

        //        var currencyStyle = package.Workbook.Styles.CreateNamedStyle("Currency");
        //        currencyStyle.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";


        //        int rowCount = 2;
        //        foreach (var item in result.Items) {
        //            //Add fields

        //            ws.Cells[rowCount, 1].Value = item.Id;
        //            if (item.UserId.HasValue) {
        //                ws.Cells[rowCount, 2].Value = item.User.FirstName + " " + item.User.LastName;
        //            } else {
        //                ws.Cells[rowCount, 2].Value = item.FirstName + " " + item.LastName;
        //            }

        //            ws.Cells[rowCount, 3].Value = item.DateCreated;
        //            ws.Cells[rowCount, 3].StyleName = "Date";

        //            ws.Cells[rowCount, 4].Value = item.PickupDate; //"Collection Date";
        //            ws.Cells[rowCount, 4].StyleName = "Currency";

        //            ws.Cells[rowCount, 5].Value = item.DropOffBranch.Title; //"Return Branch";
        //            ws.Cells[rowCount, 6].Value = item.Vehicle.Title; //"Vehicle";
        //            ws.Cells[rowCount, 7].Value = item.RateCodeTitle; //"Rate Code";
        //            ws.Cells[rowCount, 8].Value = item.BookingPrice; //"Total";
        //            ws.Cells[rowCount, 8].StyleName = "Currency";
        //            if (item.Invoice != null) {
        //                ws.Cells[rowCount, 9].Value = item.Invoice.AmountPaid; //"Amount Paid"; 
        //                ws.Cells[rowCount, 9].StyleName = "Currency";
        //            }



        //            rowCount++;
        //        }

        //        fileBytes = package.GetAsByteArray();
        //    }



        //    string fileName = "myfile.xlsx";


        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        //}



        //public ActionResult Filter([CustomizeValidator(RuleSet = ReservationValidationRuleSets.Search)] ReportSearchViewModel viewModel, int p = 1) {


        //    ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

        //    if (viewModel.Filter.PickupBranchId == 0) {
        //        viewModel.Filter.PickupBranchId = null;
        //    }

        //    if (viewModel.Filter.RateCodeId == 0) {
        //        viewModel.Filter.RateCodeId = null;
        //    }

        //    if (viewModel.Filter.CorporateId == 0) {
        //        viewModel.Filter.CorporateId = null;
        //    }

        //    ReservationsGetQuery query = new ReservationsGetQuery();

        //    if (ModelState.IsValid) {

        //        query.Pagination = pagination;
        //        query.Filter = viewModel.Filter;


        //        ListOf<ReservationModel> result = _query.Process(query);

        //        viewModel.Pagination = pagination;
        //        viewModel.Items = result.Items.OrderByDescending(x => x.Id).ToList();

        //    } else {


        //        query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

        //        query.Filter = new ReservationFilterModel { };
        //        query.Filter.ReservationState = ReservationState.Completed;
        //        ListOf<ReservationModel> result = _query.Process(query);
        //    }

        //    viewModel.Pagination.UrlGetParameters = query.Filter.ToQueryString();
        //    viewModel.Pagination.UrlAction = "/Reservations/Filter";

        //    return View("Index", viewModel);
        //}

        //public ActionResult Details(int id) {
        //    ReservationViewModel viewModel = new ReservationViewModel();
        //    viewModel.Reservation = _query.Process(new ReservationGetByIdQuery { Id = id });

        //    return View(viewModel);
        //}

        //public ActionResult Print(int id) {

        //    ReservationInvoiceNotificationModel viewModel = new ReservationInvoiceNotificationModel();
        //    viewModel.Reservation = _query.Process(new ReservationGetByIdQuery { Id = id });
        //    viewModel.Invoice = viewModel.Reservation.Invoice;
        //    if (viewModel.Invoice == null) {
        //        viewModel.Invoice = new InvoiceModel();
        //        viewModel.Reservation.Invoice = new InvoiceModel();
        //    }
        //    return View(viewModel);
        //}

        //[HttpGet]
        //public ActionResult Edit(int id) {
        //    ReservationViewModel viewModel = new ReservationViewModel();
        //    viewModel.Reservation = _query.Process(new ReservationGetByIdQuery { Id = id });
        //    return View(viewModel);
        //}

        //[HttpPost]
        //public ActionResult Edit(ReservationViewModel viewModel) {
        //    if (ModelState.IsValid) {

        //    }
        //    return View(viewModel);
        //}

    }
}