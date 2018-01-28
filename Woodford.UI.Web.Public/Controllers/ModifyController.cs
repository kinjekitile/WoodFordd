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
    public class ModifyController : Controller {
        private IQueryProcessor _query;
        private ICommandBus _commandBus;
        private readonly IReservationBuilder _reservationBuilder;
        public ModifyController(IQueryProcessor query, ICommandBus commandBus, IReservationBuilder reservationBuilder) {
            _query = query;
            _commandBus = commandBus;
            _reservationBuilder = reservationBuilder;
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

            if (reservation.IsCancelled) {
                ModelState.AddModelError("", "Reservation has been cancelled");
            }

            if (reservation.PickupDate.Date <= DateTime.Today) {
                return RedirectToAction("ReservationStarted", new { id = reservation.Id });
            }

            if (ModelState.IsValid) {
                return RedirectToAction("Modify", new { id = reservation.Id, pin = reservation.QuoteReference });
            }


            return View(viewModel);
        }

        public ActionResult Modify(int id, string pin) {
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);

            }

            if (reservation.IsCancelled) {
                ModelState.AddModelError("", "Reservation has been cancelled");
            }

            if (reservation.PickupDate.Date <= DateTime.Today) {
                return RedirectToAction("ReservationStarted", new { id = reservation.Id });
            }

            SearchResultsViewModel viewModel = new SearchResultsViewModel();
            viewModel.Reservation = reservation;

            var aiportBranches = _query.Process(new BranchesGetQuery { Filter = new BranchFilterModel { IsAirport = true, IsArchived = false }, Pagination = null }).Items;


            SearchCriteriaModel criteria = new SearchCriteriaModel();
            criteria.DropOffDate = reservation.DropOffDate;
            criteria.PickupDate = reservation.PickupDate;
            criteria.DropOffTime = reservation.DropOffDate.Hour;
            criteria.PickupTime = reservation.PickupDate.Hour;
            criteria.PickupTimeFull = criteria.PickupDate.ToString("hh:mm tt");
            criteria.DropOffTimeFull = criteria.DropOffDate.ToString("hh:mm tt");
            criteria.PickUpLocationId = reservation.PickupBranchId;
            criteria.DropOffLocationId = reservation.DropOffBranchId;
            

            SearchCriteriaViewModel criteriaViewModel = new SearchCriteriaViewModel();
            criteriaViewModel.Criteria = criteria;
            criteriaViewModel.PickupDate = criteria.PickupDate;
            criteriaViewModel.DropOffDate = criteria.DropOffDate;
            criteriaViewModel.AirportLocationIds = string.Join(",", aiportBranches.Select(x => x.Id).ToArray());
            viewModel.Criteria = criteriaViewModel;


            BookingSearchQuery query = new BookingSearchQuery { Criteria = criteria };
            
            viewModel.Results = _query.Process(query);
            //viewModel.Results.Items = viewModel.Results.Items.OrderBy(x => x.Vehicle.VehicleGroup.SortOrder).ThenBy(x => x.Vehicle.SortOrder).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Modify(int id, string pin, SearchResultsViewModel viewModel) {
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);

            }

            if (reservation.IsCancelled) {
                ModelState.AddModelError("", "Reservation has been cancelled");
            }

            if (reservation.PickupDate.Date <= DateTime.Today) {
                return RedirectToAction("ReservationStarted", new { id = reservation.Id });
            }
            viewModel.Reservation = reservation;
            viewModel.Criteria.Criteria.PickupDate = viewModel.Criteria.PickupDate;
            viewModel.Criteria.Criteria.DropOffDate = viewModel.Criteria.DropOffDate;

            
            BookingSearchQuery query = new BookingSearchQuery { Criteria = viewModel.Criteria.Criteria };

            viewModel.Results = _query.Process(query);
            //viewModel.Results.Items = viewModel.Results.Items.OrderBy(x => x.Vehicle.VehicleGroup.SortOrder).ThenBy(x => x.Vehicle.SortOrder).ToList();


            return View("Modify", viewModel);
        }

        public ActionResult Details(int id, string pin) {


            //UserGetCurrentQuery query = new UserGetCurrentQuery();
            //UserModel currentUser = _query.Process(query);
            

            ReservationViewModel viewModel = new ReservationViewModel();

            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);

            }

            
            //if (reservation.UserId != currentUser.Id) {
            //    throw new Exception("Invalid Reservation Id");
            //}
            viewModel.Reservation = reservation;

            WaiversGetQuery waiversQuery = new WaiversGetQuery { Filter = new WaiverFilterModel { VehicleGroupId = reservation.Vehicle.VehicleGroupId } };
            viewModel.Waivers = _query.Process(waiversQuery);

            return View(viewModel);
        }

        public ActionResult ChangeDates(int id, string pin) {
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);

            }

            ReservationModifyDateViewModel viewModel = new ReservationModifyDateViewModel();
            viewModel.Reservation = reservation;
            viewModel.ModifyPickupDate = reservation.PickupDate;
            viewModel.ModifyPickupTime = reservation.PickupDate.TimeOfDay.Hours;
            viewModel.ModifyDropOffDate = reservation.DropOffDate;
            viewModel.ModifyDropOffTime = reservation.DropOffDate.TimeOfDay.Hours;
            viewModel.ReservationId = reservation.Id;
            viewModel.ReservationPin = pin;


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ChangeDates(ReservationModifyDateViewModel viewModel) {
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = viewModel.ReservationId, PIN = viewModel.ReservationPin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + viewModel.ReservationId);

            }

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
                    return RedirectToAction("Details", new { id = modifyCommand.ReservationId, pin = viewModel.ReservationPin });
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

        public ActionResult ChangeVehicle(int id, string pin) {
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);

            }
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Confirm(int id, string pin, int vehicleId, int pickupBranchId, int dropOffBranchId, DateTime pickupDate, int pickupTime, DateTime dropOffDate, int dropOffTime, FormCollection fc) {

            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);

            }

            if (reservation.IsCancelled) {
                throw new Exception("Reservation Cancelled");
            }

            var rateFCValue = fc["radRates_" + vehicleId];
            int vehicleRateId = Convert.ToInt32(rateFCValue.Replace("radRates_", ""));
            int reservationId = Convert.ToInt32(fc["reservationId"]);

            string pickupTimeFull = pickupDate.AddHours(pickupTime).ToString("hh:mm tt");
            string dropoffTimeFull = dropOffDate.AddHours(dropOffTime).ToString("hh:mm tt");
            
            var modifiedReservation = _query.Process(new ReservationGetByIdQuery { Id = reservationId });
            modifiedReservation.RateId = vehicleRateId;
            modifiedReservation.VehicleId = vehicleId;
            SearchCriteriaModel searchCriteria = new SearchCriteriaModel {
                DropOffDate = dropOffDate,
                DropOffTime = dropOffTime,
                DropOffLocationId = dropOffBranchId,
                PickupDate = pickupDate,
                PickupTime = pickupTime,
                PickUpLocationId = pickupBranchId,
                VehicleId = vehicleId,
                PickupTimeFull = pickupTimeFull,
                DropOffTimeFull = dropoffTimeFull
        };

            ReservationModifyViewModel viewModel = new ReservationModifyViewModel();
            viewModel.Criteria = searchCriteria;
            viewModel.Reservation = reservation;
       
            viewModel.Modified = _reservationBuilder.InitializeModifiedFromCriteria(modifiedReservation, searchCriteria, vehicleId, vehicleRateId);

            return View("Confirm", viewModel);
        }


        [HttpPost]
        public ActionResult Complete(int id, string pin, ReservationModifyViewModel viewModel) {

            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);

            }

            if (reservation.IsCancelled) {
                throw new Exception("Reservation Cancelled");
            }

            //string pickupTimeFull = viewModel.Criteria.PickupDate.AddHours(viewModel.Criteria.PickupTime).ToString("hh:mm tt");
            //string dropoffTimeFull = viewModel.Criteria.DropOffDate.AddHours(viewModel.Criteria.DropOffTime).ToString("hh:mm tt");

            var modifiedReservation = _query.Process(new ReservationGetByIdQuery { Id = id });
            modifiedReservation.RateId = viewModel.Modified.RateId;
            modifiedReservation.VehicleId = viewModel.Modified.VehicleId;
            

            SearchCriteriaModel searchCriteria = new SearchCriteriaModel {
                DropOffDate = viewModel.Modified.DropOffDate.Date,
                DropOffTime = viewModel.Modified.DropOffDate.Hour,
                DropOffLocationId = viewModel.Modified.DropOffBranchId,
                PickupDate = viewModel.Modified.PickupDate.Date,
                PickupTime = viewModel.Modified.PickupDate.Hour,
                PickUpLocationId = viewModel.Modified.PickupBranchId,
                VehicleId = viewModel.Modified.VehicleId,
                PickupTimeFull = viewModel.Modified.PickupDate.ToString("hh:mm tt"),
                DropOffTimeFull = viewModel.Modified.DropOffDate.ToString("hh:mm tt")
            };

            
            viewModel.Criteria = searchCriteria;
            viewModel.Reservation = reservation;

            viewModel.Modified = _reservationBuilder.InitializeModifiedFromCriteria(modifiedReservation, searchCriteria, viewModel.Modified.VehicleId, viewModel.Modified.RateId);

            _commandBus.Submit(new ReservationModifyCommand { Reservation = viewModel.Modified });

            return RedirectToAction("Complete", new { id = reservation.Id, pin = reservation.QuoteReference });
        }

        [HttpGet]
        public ActionResult Complete(int id, string pin) {
            var reservation = _query.Process(new ReservationGetByIdAndPINQuery { Id = id, PIN = pin });
            if (reservation == null) {
                throw new Exception("Invalid Pin Attempt: " + id);

            }
            ReservationViewModel viewModel = new ReservationViewModel();
            viewModel.Reservation = reservation;
            return View(viewModel);
        }

        public ActionResult ReservationStarted(int id) {
            return View(id);
        }

        public ActionResult ReservationCancelled(int id) {
            return View(id);
        }

        public ActionResult Option() {
            return View();
        }
    }
}