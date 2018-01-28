using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ModelValidators;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class BookingClaimsController : Controller
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _query;

        public BookingClaimsController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        // GET: BookingClaims
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id) {
            BookingClaimViewModel viewModel = new BookingClaimViewModel();
            viewModel.Claim = _query.Process(new ClaimBookingGetByIdQuery { Id = id });
            viewModel.User = _query.Process(new UserGetByIdQuery { Id = viewModel.Claim.UserId });

            return View(viewModel);
        }


        [HttpGet]
        public ActionResult AddBookingHistory(int? id = null, int? userId = null) {

            if (!id.HasValue) {
                if (userId.HasValue) {
                    AddBookingHistoryViewModel newModel = new AddBookingHistoryViewModel();
                    newModel.BookingHistory = new BookingHistoryModel();
                    newModel.BookingHistory.UserId = userId.Value;
                    return View(newModel);

                } else {
                    throw new Exception("no user Id");
                }

                return View(new AddBookingHistoryViewModel());
            }
            var bookingClaim = _query.Process(new ClaimBookingGetByIdQuery { Id = id.Value });
            AddBookingHistoryViewModel viewModel = new AddBookingHistoryViewModel();
            viewModel.BookingHistory = new BookingHistoryModel(bookingClaim);
            viewModel.ClaimBookingId = id.Value;

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult AddBookingHistory([CustomizeValidator(RuleSet = BookingHistoryValidationRuleSets.Default)] AddBookingHistoryViewModel viewModel) {

            if (ModelState.IsValid) {

                var historyItemExists = _query.Process(new BookingHistoryByExternalIdQuery { ExternalId = viewModel.BookingHistory.ExternalId });

                if (historyItemExists != null) {
                    ModelState.AddModelError("BookingHistory.ExternalId", "This booking has already been claimed");
                } else {
                    BookingHistoryAddCommand addBookingHistory = new BookingHistoryAddCommand();
                    addBookingHistory.BookingHistory = viewModel.BookingHistory;

                    _commandBus.Submit(addBookingHistory);

                    return RedirectToAction("BookingHistory", "Users", new { id = addBookingHistory.BookingHistory.UserId });
                }

                
            }

            
            return View(viewModel);
        }

    }
}