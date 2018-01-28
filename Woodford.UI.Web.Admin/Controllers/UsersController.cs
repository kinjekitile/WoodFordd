using FluentValidation.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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

    public class UsersController : Controller {
        private ICommandBus _commandBus;
        private IQueryProcessor _query;

        public UsersController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        [Authorize(Roles = "Administrator,Branch,Operations")]
        public ActionResult Index(int p = 1) {

            ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

            UserFilterModel filter = new UserFilterModel();

            UsersGetQuery query = new UsersGetQuery();
            query.Pagination = pagination;
            query.Filter = filter;

            ListOf<UserModel> result = _query.Process(query);

            UserSearchViewModel viewModel = new UserSearchViewModel();
            viewModel.Filter = filter;
            viewModel.Pagination = pagination;
            viewModel.Items = result.Items;

            viewModel.Pagination.UrlGetParameters = query.Filter.ToQueryString();
            viewModel.Pagination.UrlAction = "/Users/Filter";

            return View(viewModel);

        }

        [Authorize(Roles = "Administrator,Branch,Operations")]
        public FileResult ExportAsExcel(UserFilterModel viewModel) {
            if (viewModel.CorporateId == 0) {
                viewModel.CorporateId = null;
            }

            UsersGetQuery query = new UsersGetQuery();
            query.Pagination = null;
            query.Filter = viewModel;

            ListOf<UserModel> result = _query.Process(query);

            byte[] fileBytes;

            using (ExcelPackage package = new ExcelPackage()) {
                var ws = package.Workbook.Worksheets.Add("report");

                //Add report data

                ws.Cells[1, 1].Value = "Email";
                ws.Cells[1, 2].Value = "Name";
                ws.Cells[1, 3].Value = "Cell";
                ws.Cells[1, 4].Value = "Tier";
                ws.Cells[1, 5].Value = "Loyalty Points Earned";
                ws.Cells[1, 6].Value = "Loyalty Points Remaining";
                ws.Cells[1, 7].Value = "Loyalty Points Spent";
                ws.Cells[1, 8].Value = "Loyalty Number";


                //Shared Styles
                var dateStyle = package.Workbook.Styles.CreateNamedStyle("Date");
                dateStyle.Style.Numberformat.Format = "dd-MM-yyyy";

                var currencyStyle = package.Workbook.Styles.CreateNamedStyle("Currency");
                currencyStyle.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";


                int rowCount = 2;
                foreach (var item in result.Items) {
                    //Add fields

                    ws.Cells[rowCount, 1].Value = item.Email;
                    ws.Cells[rowCount, 2].Value = item.FirstName + " " + item.LastName;
                    ws.Cells[rowCount, 3].Value = item.MobileNumber;
                    ws.Cells[rowCount, 4].Value = item.LoyaltyTier.GetDescription();
                    if (item.IsLoyaltyMember && item.LoyaltyPointsEarned.HasValue) {
                        ws.Cells[rowCount, 5].Value = decimal.Round(item.LoyaltyPointsEarned.Value, 2);
                    }
                    else {
                        ws.Cells[rowCount, 5].Value = 0m;
                    }
                    if (item.IsLoyaltyMember) {
                        ws.Cells[rowCount, 6].Value = decimal.Round(item.LoyaltyPointsRemaining, 2);
                    }
                    else {
                        ws.Cells[rowCount, 6].Value = 0m;
                    }
                    if (item.IsLoyaltyMember && item.LoyaltyPointsSpent.HasValue) {
                        ws.Cells[rowCount, 7].Value = decimal.Round(item.LoyaltyPointsSpent.Value, 2);
                    }
                    else {
                        ws.Cells[rowCount, 7].Value = 0m;
                    }
                    ws.Cells[rowCount, 8].Value = item.LoyaltyNumberFull;
                    rowCount++;
                }

                decimal totalPoints = 0m;
                decimal totalSpent = 0m;
                decimal totalRemain = 0m;

                totalPoints = result.Items.Where(x => x.LoyaltyPointsEarned.HasValue).Select(x => x.LoyaltyPointsEarned.Value).DefaultIfEmpty().Sum();
                totalSpent = result.Items.Where(x => x.LoyaltyPointsSpent.HasValue).Select(x => x.LoyaltyPointsSpent.Value).DefaultIfEmpty().Sum();
                totalRemain = totalPoints - totalSpent;

                ws.Cells[rowCount, 5].Value = totalPoints;
                ws.Cells[rowCount, 6].Value = totalRemain;
                ws.Cells[rowCount, 7].Value = totalSpent;


                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                fileBytes = package.GetAsByteArray();
            }



            string fileName = "Users Report - " + DateTime.Now.ToString("dd MMM yyyy HH:mm") + ".xlsx";


            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        public ActionResult SaveReport([CustomizeValidator(RuleSet = UserValidationRuleSets.Report)] UserSearchViewModel viewModel) {



            if (ModelState.IsValid) {
                ReportAddCommand addReport = new ReportAddCommand();
                addReport.Model = viewModel.Report;
                addReport.Model.ReportType = ReportType.User;
                addReport.Model.UserFilter = viewModel.Filter;
                addReport.Model.ReportFilter = viewModel.Filter.ToQueryString();


                _commandBus.Submit(addReport);
                return RedirectToAction("Index", "Reports");


            }
            else {


                ListPaginationModel pagination = new ListPaginationModel { CurrentPage = 1, ItemsPerPage = 20 };
                UsersGetQuery query = new UsersGetQuery();

                query.Pagination = pagination;
                query.Filter = viewModel.Filter;


                var result = _query.Process(query);

                foreach (var item in result.Items) {
                    if (item.IsLoyaltyMember) {
                        var overview = _query.Process(new LoyaltyOverviewByUserIdQuery { UserId = item.Id });
                        item.LoyaltyOverview = overview;
                    }
                }

                viewModel.Pagination = pagination;
                viewModel.Items = result.Items.OrderByDescending(x => x.Id).ToList();

                return View("Index", viewModel);
            }


        }

        [Authorize(Roles = "Administrator,Branch,Operations")]
        public ActionResult Filter([CustomizeValidator(RuleSet = UserValidationRuleSets.Search)] UserSearchViewModel viewModel, int p = 1) {

            //Essa asked for no pagination after search
            //ListPaginationModel pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

            ListPaginationModel pagination = null;


            if (viewModel.Filter.CorporateId == 0) {
                viewModel.Filter.CorporateId = null;
            }
            if (ModelState.IsValid) {
                UsersGetQuery query = new UsersGetQuery();
                query.Pagination = pagination;
                query.Filter = viewModel.Filter;

                var result = _query.Process(query);


                viewModel.Pagination = pagination;
                viewModel.Items = result.Items;
            }
            else {

                UsersGetQuery query = new UsersGetQuery();
                query.Pagination = new ListPaginationModel { CurrentPage = p, ItemsPerPage = 20 };

                query.Filter = new UserFilterModel { };

                var result = _query.Process(query);
                viewModel.Pagination = pagination;
                viewModel.Items = result.Items;
            }

            if (viewModel.Pagination == null) {
                viewModel.Pagination = new ListPaginationModel();
            }

            viewModel.Pagination.UrlGetParameters = viewModel.Filter.ToQueryString();

            viewModel.Pagination.UrlAction = "/Users/Filter";

            return View("Index", viewModel);
        }

        [Authorize(Roles = "Administrator,Branch,Operations")]
        public ActionResult Details(int id) {
            UserGetByIdQuery query = new UserGetByIdQuery { Id = id };

            var user = _query.Process(query);
            var loyaltyTier = (LoyaltyTierLevel)user.LoyaltyTierId;
            UserViewModel viewModel = new UserViewModel();

            ViewBag.LoyaltyTier = loyaltyTier.GetDescription();
            switch (loyaltyTier) {
                case LoyaltyTierLevel.Green:
                    viewModel.BookingsRequiredForNextLoyaltyTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Silver }).BookingThresholdPerPeriod;
                    break;
                case LoyaltyTierLevel.Silver:
                    viewModel.BookingsRequiredForNextLoyaltyTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Gold }).BookingThresholdPerPeriod;
                    break;

                case LoyaltyTierLevel.Gold:
                    viewModel.BookingsRequiredForNextLoyaltyTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Gold }).BookingThresholdPerPeriod;
                    break;
            }


            viewModel.User = user;
            viewModel.LoyaltyOverview = _query.Process(new LoyaltyOverviewByUserIdQuery { UserId = user.Id });


            return View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult SetLoyaltyTier(int id) {
            UserGetByIdQuery query = new UserGetByIdQuery { Id = id };
            var user = _query.Process(query);
            return View(user);
        }
        
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult SetLoyaltyTier(UserModel model) {
            if (ModelState.IsValid) {
                UserSetLoyaltyCommand command = new UserSetLoyaltyCommand { UserId = model.Id, LoyaltyTierId = model.LoyaltyTierId };
                _commandBus.Submit(command);
                return RedirectToAction("Details", new { id = model.Id });
            }
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult FixLoyaltyTier(int id) {
            UserGetByIdQuery query = new UserGetByIdQuery { Id = id };
            var user = _query.Process(query);
            return View(user);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult FixLoyaltyTier(UserModel model) {
            if (ModelState.IsValid) {
                UserSetLoyaltyTierFixedCommand command = new UserSetLoyaltyTierFixedCommand { UserId = model.Id, IsFixedTier = model.IsLoyaltyTierLocked };
                _commandBus.Submit(command);
                return RedirectToAction("Details", new { id = model.Id });
            }
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult SetCorporate(int id) {
            UserGetByIdQuery query = new UserGetByIdQuery { Id = id };
            var user = _query.Process(query);
            return View(user);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult SetCorporate(UserModel model) {
            if (model.CorporateId == 0) {
                model.CorporateId = null;
            }
            if (ModelState.IsValid) {
                UserSetCorporateCommand command = new UserSetCorporateCommand { UserId = model.Id, CorporateId = model.CorporateId };
                _commandBus.Submit(command);
                return RedirectToAction("Details", new { id = model.Id });
            }
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult SetRoles(int id) {
            UserGetByIdQuery query = new UserGetByIdQuery { Id = id };
            var user = _query.Process(query);
            return View(user);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult SetRoles(UserModel model, FormCollection fc) {

            if (ModelState.IsValid) {
                var user = _query.Process(new UserGetByIdQuery { Id = model.Id });


                List<UserRoles> userRoles = new List<UserRoles>();
                foreach (var role in Enum.GetValues(typeof(UserRoles)).Cast<UserRoles>()) {
                    if (fc["role" + role.ToString()] != null) {
                        userRoles.Add(role);
                    }
                }


                UserAssignRolesCommand command = new UserAssignRolesCommand { User = user, Roles = userRoles };
                _commandBus.Submit(command);
                return RedirectToAction("Details", new { id = model.Id });
            }
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult BookingHistory(int id) {


            var bookingHistory = _query.Process(new BookingHistoryGetQuery { Filter = new BookingHistoryFilterModel { UserId = id } });
            return View(bookingHistory);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult DeleteHistoryRecord(int id) {
            var history = _query.Process(new BookingHistoryByIdQuery { Id = id });

            return View(history);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult DeleteHistoryRecord(BookingHistoryModel model) {
            if (ModelState.IsValid) {
                var history = _query.Process(new BookingHistoryByIdQuery { Id = model.Id });
                _commandBus.Submit(new BookingHistoryDeleteCommand { BookingHistoryId = model.Id });
                return RedirectToAction("BookingHistory", new { id = history.UserId });
            }
            return View(model);
        }

        public ActionResult Reservations(int id) {


            var reservations = _query.Process(new ReservationsGetQuery { Filter = new ReservationFilterModel { UserId = id, IsCompletedInvoice = true } });
            return View(reservations);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult Add() {
            UserViewModel viewModel = new UserViewModel();
            viewModel.User = new UserModel();
            return View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Add([CustomizeValidator(RuleSet = UserRegistrationValidationRuleSets.Default)] UserViewModel viewModel, FormCollection fc) {

            if (ModelState.IsValid) {

                UserRegisterCommand regCommand = new UserRegisterCommand();
                if (!string.IsNullOrEmpty(viewModel.AdminPassword)) {
                    //Admin has provided a password for the account
                    regCommand.Password = viewModel.AdminPassword;
                }else {
                    regCommand.Password = Utilities.GeneratePassword(6);
                }
                
                regCommand.User = viewModel.User;
                regCommand.IsAdminCreatedAccount = true;
                _commandBus.Submit(regCommand);

                List<UserRoles> userRoles = new List<UserRoles>();
                foreach (var role in Enum.GetValues(typeof(UserRoles)).Cast<UserRoles>()) {
                    if (fc["role" + role.ToString()] != null) {
                        userRoles.Add(role);
                    }
                }

                var user = regCommand.User;


                UserAssignRolesCommand command = new UserAssignRolesCommand { User = user, Roles = userRoles };
                _commandBus.Submit(command);


                return RedirectToAction("Details", new { id = user.Id });
            }
            return View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult ChangeEmail(int id) {
            UserChangeEmailViewModel viewModel = new UserChangeEmailViewModel();
            viewModel.User = _query.Process(new UserGetByIdQuery { Id = id });
            return View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult ChangeEmail([CustomizeValidator(RuleSet = UserRegistrationValidationRuleSets.ChangeEmail)] UserChangeEmailViewModel viewModel) {

            if (ModelState.IsValid) {

                UserSetEmailCommand command = new UserSetEmailCommand();
                command.UserId = viewModel.User.Id;
                command.Email = viewModel.User.Email;

                _commandBus.Submit(command);


                return RedirectToAction("Details", new { id = viewModel.User.Id });
            }
            return View(viewModel);
        }


        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult SetDisabled(int id) {
            UserSetDisabledViewModel viewModel = new UserSetDisabledViewModel();
            viewModel.User = _query.Process(new UserGetByIdQuery { Id = id });
            return View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult SetDisabled(UserSetDisabledViewModel viewModel) {

            if (ModelState.IsValid) {

                UserSetAccountDisabledCommand command = new UserSetAccountDisabledCommand();
                command.UserId = viewModel.User.Id;
                command.Disabled = viewModel.User.IsAccountDisabled;

                _commandBus.Submit(command);


                return RedirectToAction("Details", new { id = viewModel.User.Id });
            }
            return View(viewModel);
        }




        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult ChangePassword(int id) {
            UserSetPasswordViewModel viewModel = new UserSetPasswordViewModel();
            viewModel.User = _query.Process(new UserGetByIdQuery { Id = id });
            return View(viewModel);
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult ChangePassword(UserSetPasswordViewModel viewModel) {


            if (ModelState.IsValid) {
                UserSetPasswordCommand command = new UserSetPasswordCommand();

                command.UserId = viewModel.User.Id;
                command.NewPassword = viewModel.NewPassword;

                _commandBus.Submit(command);

                return RedirectToAction("Details", new { id = viewModel.User.Id });

            }

            return View(viewModel);
        }

        //UserChangeEmailValidator
    }
}