using FluentValidation.Mvc;
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
using Woodford.UI.Web.Public.Code.Filters;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers {
    public class SearchController : Controller {

        private IQueryProcessor _query;
        private ICommandBus _commandBus;

        public SearchController(IQueryProcessor query, ICommandBus commandBus) {
            _query = query;
            _commandBus = commandBus;
        }

        [HttpGet]
        public ActionResult Index() {
            SearchResultsModel model = new SearchResultsModel();
            model.Criteria = new SearchCriteriaModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Results([CustomizeValidator(RuleSet = SearchValidationRuleSets.Default)] SearchCriteriaViewModel viewModel) {

            if (!viewModel.IsReturnDifferentLocation) {
                viewModel.Criteria.DropOffLocationId = viewModel.Criteria.PickUpLocationId;
            }

            SearchResultsViewModel model = new SearchResultsViewModel();

            viewModel.Criteria.PickupDate = viewModel.PickupDate;
            viewModel.Criteria.DropOffDate = viewModel.DropOffDate;
            model.Criteria = viewModel;

            model.Results = new SearchResultsModel();
            model.Results.Criteria = viewModel.Criteria;

            removeCountDownSessionVars();

          
            
            if (ModelState.IsValid) {
                ViewBag.ShowError = false;
                CountdownSpecialModel countdownSpecial = null;
                if (checkCountDownSpecialCookieExists()) {
                    removeCountDownSpecialCookie();
                    countdownSpecial = getApplicableCountdownSpecial();
                } else {
                    addCountDownSpecialCookie();
                }                

                model.CountdownSpecial = countdownSpecial;
                if (countdownSpecial != null) {
                    model.CountDownSpecialExpiry = DateTime.Now.AddMinutes(getCountdownSpecialMinutes());
                    Session["CountDownSpecialExpire"] = model.CountDownSpecialExpiry;
                }

                BookingSearchQuery query = new BookingSearchQuery { Criteria = viewModel.Criteria };
                model.Results = _query.Process(query);
             
            } else {
                ViewBag.ShowError = true;
            }

            return View(model);
        }

        [CheckLeadTimeFilter]
        [HttpPost]
        public ActionResult Book(int vehicleId, int pickupBranchId, int dropOffBranchId, DateTime pickupDate, int pickupTime, DateTime dropOffDate, int dropOffTime, string pickupTimeFull, string dropoffTimeFull, FormCollection fc) {
            var rateFCValue = fc["radRates_" + vehicleId];
            int vehicleRateId = Convert.ToInt32(rateFCValue.Replace("radRates_", ""));

            string countdownSpecialIdString = fc["countDownSpecialId"];
            string countDownSpecialRate = fc["countDownSpecialRate"];
            setCountdownSpecialSession(countdownSpecialIdString, countDownSpecialRate, vehicleRateId, vehicleId);

            SearchCriteriaModel searchCriteria = new SearchCriteriaModel {
                DropOffDate = dropOffDate,
                DropOffTime = dropOffTime,
                DropOffLocationId = dropOffBranchId,
                PickupDate = pickupDate,
                PickupTime = pickupTime,
                PickUpLocationId = pickupBranchId,
                PickupTimeFull = pickupTimeFull,
                DropOffTimeFull = dropoffTimeFull
            };

            ReservationAddCommand command = new ReservationAddCommand { Criteria = searchCriteria, RateId = vehicleRateId, VehicleId = vehicleId };
            _commandBus.Submit(command);

            ReservationAddBenefitsCommand addBenefits = new ReservationAddBenefitsCommand { ReservationId = command.ReservationOut.Id };
            _commandBus.Submit(addBenefits);

            int reservationId = command.ReservationOut.Id;

            ReservationAddBranchSurchargesCommand addSurcharges = new ReservationAddBranchSurchargesCommand { ReservationId = reservationId };
            _commandBus.Submit(addSurcharges);

            return RedirectToAction("UserDetails", "Checkout", new { id = reservationId });
        }

        public CountdownSpecialModel getApplicableCountdownSpecial() {
            CountdownSpecialsGetQuery query = new CountdownSpecialsGetQuery { Filter = new CountdownSpecialFilterModel { IsActive = true }, Pagination = null };
            ListOf<CountdownSpecialModel> specials = _query.Process(query);
            if (specials.Items.Count() == 0) {
                return null;
            } else {
                var result = specials.Items.OrderByDescending(x => x.Id).FirstOrDefault();
                if (result != null) {
                    if (result.SpecialType == CountdownSpecialType.VehicleUpgrade) {
                        var upgrade = _query.Process(new VehicleUpgradeGetByIdQuery { Id = result.VehicleUpgradeId.Value });
                        result.VehicleUpgradeFromId = upgrade.FromVehicleId;
                        var vehicle = _query.Process(new VehicleGetByIdQuery { Id = upgrade.ToVehicleId });
                        if (result.VehicleUpgradeAmountOverride.HasValue) {
                            if (result.VehicleUpgradeAmountOverride.Value > 0) {
                                result.OfferText = "an upgrade to " + vehicle.Title + " for " + string.Format("{0:c}", result.VehicleUpgradeAmountOverride.Value);
                            } else {
                                result.OfferText = "a free upgrade to: " + vehicle.Title;
                            }
                            
                        } else {
                            result.OfferText = "a free upgrade to: " + vehicle.Title;
                        }
                        
                    }
                }
                return result;
            }
            
        }

        public ActionResult SearchExpired() {
            ViewBag.PanelMinimised = false;
            return View();
        }

        private bool checkCountDownSpecialCookieExists() {
            HttpCookie countDownSpecialCookie = Request.Cookies["woodfordShowCountDownSpecial"];
            return (countDownSpecialCookie != null);
        }

        private void addCountDownSpecialCookie() {
            HttpCookie cookie = new HttpCookie("woodfordShowCountDownSpecial");
            cookie.Value = "1";
            int days = getCountdownSpecialCookieDays();
            cookie.Expires = DateTime.Now.AddDays(days);
            Response.Cookies.Add(cookie);
        }

        private void removeCountDownSpecialCookie() {
            HttpCookie cookie = new HttpCookie("woodfordShowCountDownSpecial");
            cookie.Value = "1";
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
        }

        private void removeCountDownSessionVars() {
            Session.Remove("CountDownSpecialType");
            //Session.Remove("CountDownSpecialRate");
            Session.Remove("CountDownSpecialReward");
            Session.Remove("CountDownSpecialAmount");
            //Session.Remove("CountDownSpecialExpire");
        }

        private int getCountdownSpecialCookieDays() {
            SettingGetByNameQuery query = new SettingGetByNameQuery { SettingName = Setting.Countdown_Special_Cookie_Days };
            SettingModel s = _query.Process(query);
            return Convert.ToInt32(s.Value);
        }

        private int getCountdownSpecialMinutes() {
            SettingGetByNameQuery query = new SettingGetByNameQuery { SettingName = Setting.Countdown_Special_Minutes };
            SettingModel s = _query.Process(query);
            return Convert.ToInt32(s.Value);
        }

        private void setCountdownSpecialSession(string countdownSpecialIdString, string countdownSpecialRateIdString, int selectedRateId, int selectedVehicleId) {
            
            if (!string.IsNullOrEmpty(countdownSpecialIdString)) {
                int countdownSpecialId = Convert.ToInt32(countdownSpecialIdString);

                //check if countdown special is available
                CountdownSpecialModel countdownSpecial = getApplicableCountdownSpecial();

                if (countdownSpecial == null) {
                    removeCountDownSessionVars();
                } else {
                    if (countdownSpecial.Id != countdownSpecialId) {
                        removeCountDownSessionVars();
                    } else {

                        Session["CountDownSpecialType"] = Convert.ToInt32(countdownSpecial.SpecialType);
                        switch (countdownSpecial.SpecialType) {
                            case CountdownSpecialType.TextOnInvoice:
                                
                                if (!string.IsNullOrEmpty(countdownSpecialRateIdString)) {
                                    if (Convert.ToInt32(countdownSpecialRateIdString) == selectedRateId) {
                                        Session["CountDownSpecialReward"] = countdownSpecial.OfferText;
                                        Session["CountDownSpecialId"] = countdownSpecial.Id;
                                    } else {
                                        removeCountDownSessionVars();
                                    }
                                } else {
                                    removeCountDownSessionVars();
                                }

                                
                                break;

                            case CountdownSpecialType.VehicleUpgrade:
                                Session["CountDownSpecialReward"] = countdownSpecial.VehicleUpgradeId.ToString();
                                Session["CountDownSpecialAmount"] = (countdownSpecial.VehicleUpgradeAmountOverride ?? 0m);
                                Session["CountDownSpecialId"] = countdownSpecial.Id;
                                var vehicleUpgrade = _query.Process(new VehicleUpgradeGetByIdQuery { Id = countdownSpecial.VehicleUpgradeId.Value });
                                var vehicle = _query.Process(new VehicleGetByIdQuery { Id = vehicleUpgrade.ToVehicleId });
                                Session["CountDownSpecialVehicleUpgradeTitle"] = vehicle.Title;
                                break;
                        }
                        
                    }
                }


            }            
        }
    }
}
