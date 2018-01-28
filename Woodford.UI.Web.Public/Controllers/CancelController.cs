using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Commands.Users;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers {
    public class CancelController : Controller {

        private IQueryProcessor _query;
        private ICommandBus _commandBus;
        private readonly IReservationBuilder _reservationBuilder;
        private readonly ISettingService _settings;
        public CancelController(IQueryProcessor query, ICommandBus commandBus, IReservationBuilder reservationBuilder, ISettingService settings) {
            _query = query;
            _commandBus = commandBus;
            _reservationBuilder = reservationBuilder;
            _settings = settings;
        }

        public ActionResult Index() {
            ViewBag.DoesNotExist = false;
            return View(new ReservationViewModel());
        }

        [HttpPost]
        public ActionResult Index([CustomizeValidator(RuleSet = ReservationValidationRuleSets.ModifyLookup)] ReservationViewModel viewModel) {
            ViewBag.DoesNotExist = false;

            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = viewModel.Reservation.Id, PIN = viewModel.Reservation.QuoteReference });

            if (reservation == null) {
                ModelState.AddModelError("", "Number or PIN is invalid");
                return View(viewModel);
            }
            //The below removed as we will be trialing cancelling at any point in booking
            //int hoursLeadTime = _settings.GetValue<int>(Setting.Cancel_Lead_Time_Hours);
            //if (reservation.PickupDate <= DateTime.Now.AddHours(hoursLeadTime)) {
            //    return RedirectToAction("NoLeadTime", new { id = reservation.Id, pin = viewModel.Reservation.QuoteReference });
            //}

            if (ModelState.IsValid) {
                return RedirectToAction("Confirm", new { id = reservation.Id, pin = reservation.QuoteReference });
            }


            return View(viewModel);
        }

        public ActionResult Confirm(int id, string pin) {
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);
            }

            //The below removed as we will be trialing cancelling at any point in booking
            //int hoursLeadTime = _settings.GetValue<int>(Setting.Cancel_Lead_Time_Hours);
            //if (reservation.PickupDate <= DateTime.Now.AddHours(hoursLeadTime)) {
            //    return RedirectToAction("NoLeadTime", new { id = reservation.Id, pin = pin });
            //}


            ReservationViewModel viewModel = new ReservationViewModel();
            viewModel.Reservation = reservation;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Confirm(ReservationViewModel viewModel) {
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = viewModel.Reservation.Id, PIN = viewModel.Reservation.QuoteReference });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + viewModel.Reservation.Id);
            }

            //The below removed as we will be trialing cancelling at any point in booking
            //int hoursLeadTime = _settings.GetValue<int>(Setting.Cancel_Lead_Time_Hours);
            //if (reservation.PickupDate <= DateTime.Now.AddHours(hoursLeadTime)) {
            //    return RedirectToAction("NoLeadTime", new { id = reservation.Id, pin = reservation.QuoteReference });
            //}

            ReservationCancelCommand cancelReservation = new ReservationCancelCommand();
            cancelReservation.ReservationId = reservation.Id;
            cancelReservation.IsCancelled = true;
            //The below added as we will be trialing cancelling at any point in booking
            cancelReservation.IgnoreLeadTime = true;

            _commandBus.Submit(cancelReservation);
            switch (cancelReservation.OutCancelStatus) {
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
                    return RedirectToAction("Complete", new { id = reservation.Id, pin = reservation.QuoteReference });
            }
            
            viewModel.Reservation = reservation;
            return View(viewModel);
        }

        public ActionResult NoLeadTime(int id, string pin) {
            
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);
            }

            ReservationViewModel viewModel = new ReservationViewModel();
            viewModel.Reservation = reservation;
            return View(viewModel);
        }

        public ActionResult Complete(int id, string pin) {
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);
            }

            ReservationViewModel viewModel = new ReservationViewModel();
            viewModel.Reservation = reservation;
            return View(viewModel);
        }
    }
}