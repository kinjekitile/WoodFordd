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
    [Authorize]
    public class ReservationsController : Controller {
        private IQueryProcessor _query;
        private ICommandBus _commandBus;

        public ReservationsController(IQueryProcessor query, ICommandBus commandBus) {
            _query = query;
            _commandBus = commandBus;
        }
        public ActionResult Index() {
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);

            var reservations = _query.Process(new ReservationsGetQuery { Filter = new ReservationFilterModel { UserId = currentUser.Id , ReservationState = ReservationState.Completed }, Pagination = new ListPaginationModel { ItemsPerPage = 50, CurrentPage = 1 } });

            reservations.Items = reservations.Items.Where(x => x.PickupDate >= DateTime.Today).ToList();

            return View(reservations);
        }

        public ActionResult Past() {

            BookingHistoryViewModel viewModel = new BookingHistoryViewModel();
                 
            //Use booking history here, this will show all woodford bookings a user has made and not just their website bookings
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);

            if (currentUser.IsLoyaltyMember) {
                viewModel.ShowLoyaltyPointsEarned = true;
            }

            var bookingHistory = _query.Process(new BookingHistoryGetQuery { Filter = new BookingHistoryFilterModel { UserId = currentUser.Id }, Pagination = null });

            viewModel.Result = bookingHistory;

            return View(viewModel);
        }


        public ActionResult Details(int id) {
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);

            ReservationViewModel viewModel = new ReservationViewModel();

            var reservation = _query.Process(new ReservationGetByIdQuery { Id = id });
            if (reservation.UserId != currentUser.Id) {
                throw new Exception("Invalid Reservation Id");
            }
            viewModel.Reservation = reservation;

            WaiversGetQuery waiversQuery = new WaiversGetQuery { Filter = new WaiverFilterModel { VehicleGroupId = reservation.Vehicle.VehicleGroupId } };
            viewModel.Waivers = _query.Process(waiversQuery);

            return View(viewModel);
        }

        public ActionResult ChangeDates(int id) {
            var reservation = _query.Process(new ReservationGetByIdQuery { Id = id });
            ReservationModifyDateViewModel viewModel = new ReservationModifyDateViewModel();
            viewModel.ModifyPickupDate = reservation.PickupDate;
            viewModel.ModifyPickupTime = reservation.PickupDate.TimeOfDay.Hours;
            viewModel.ModifyDropOffDate = reservation.DropOffDate;
            viewModel.ModifyDropOffTime = reservation.DropOffDate.TimeOfDay.Hours;
            viewModel.ReservationId = reservation.Id;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ChangeDates(ReservationModifyDateViewModel viewModel) {
            if (ModelState.IsValid) {
                UserModifyReservationDatesCommand modifyCommand = new UserModifyReservationDatesCommand();
                modifyCommand.IsDateUpdate = true;
                modifyCommand.ReservationId = viewModel.ReservationId;
                modifyCommand.PickupDate = viewModel.ModifyPickupDate;
                modifyCommand.PickupTime = viewModel.ModifyPickupTime;
                modifyCommand.DropOffDate = viewModel.ModifyDropOffDate;
                modifyCommand.DropOffTime = viewModel.ModifyDropOffTime;

                _commandBus.Submit(modifyCommand);

                if (modifyCommand.Success) {
                    return RedirectToAction("Details", new { id = modifyCommand.ReservationId });
                } else {
                    if (modifyCommand.AlternateRates.Count > 0) {
                        viewModel.AlternateRates = modifyCommand.AlternateRates;
                        return View("SelectRate", viewModel);
                    } 
                    if (modifyCommand.AlternateResults.Count > 0) {
                        viewModel.AlternateResults = modifyCommand.AlternateResults;
                        return View("Results", viewModel);
                    }
                }


            }
            return View(viewModel);
        }



        public ActionResult ChangeVehicle(int id) {
            var reservation = _query.Process(new ReservationGetByIdQuery { Id = id });
            ReservationViewModel viewModel = new ReservationViewModel();
            viewModel.Reservation = reservation;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ChangeVehicle(ReservationModifyDateViewModel viewModel) {
            if (ModelState.IsValid) {
                UserModifyReservationDatesCommand modifyCommand = new UserModifyReservationDatesCommand();
                modifyCommand.IsDateUpdate = true;
                modifyCommand.PickupDate = viewModel.ModifyPickupDate;
                modifyCommand.PickupTime = viewModel.ModifyPickupTime;
                modifyCommand.DropOffDate = viewModel.ModifyDropOffDate;
                modifyCommand.DropOffTime = viewModel.ModifyDropOffTime;

                _commandBus.Submit(modifyCommand);

                if (modifyCommand.Success) {

                } else {
                    if (modifyCommand.AlternateRates.Count > 0) {

                    } else {
                        //Date change is invalid for this booking

                    }
                }


            }
            return View(viewModel);
        }

        [AllowAnonymous]
        public ActionResult Modify() {
            ViewBag.DoesNotExist = false;
            return View(new ReservationViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Modify([CustomizeValidator(RuleSet = ReservationValidationRuleSets.ModifyLookup)] ReservationViewModel viewModel) {
            ViewBag.DoesNotExist = false; 

            if (ModelState.IsValid) {
                var reservation = _query.Process(new ReservationsGetQuery { Filter = new ReservationFilterModel { QuoteReference = viewModel.Reservation.QuoteReference }, Pagination = null }).Items.SingleOrDefault();

                if (reservation == null) {
                    ViewBag.DoesNotExist = true;
                } else {
                    if (Request.IsAuthenticated) {
                        //User is logged on, take them to the details page for the booking
                        return RedirectToAction("Details", new { id = reservation.Id });
                    } else {
                        //Not logged in, check reservation has user id
                        if (reservation.UserId.HasValue) {
                            //For now just show the modify options, might need to ask to login
                        } else {
                            //Show modify options
                        }
                    }

                }
            }
            

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EmailQuote(int id) {




            return View();
        }
    }
}