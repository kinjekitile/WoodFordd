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
    public class CheckoutController : Controller {
        private IQueryProcessor _query;
        private ICommandBus _commandBus;
        private IPaymentProcessor _paymentProcessor;
        private string[] mobileDevices = new string[] { "iphone", "ppc", "windows ce", "blackberry", "opera mini", "mobile", "palm", "portable", "opera mobi" };

        public CheckoutController(IQueryProcessor query, ICommandBus commandBus, IPaymentProcessor paymentProcessor) {
            _query = query;
            _commandBus = commandBus;
            _paymentProcessor = paymentProcessor;

        }

        [CheckLeadTimeFilter]
        [CheckQuoteExpiredFilter]
        [Route("checkout/options/{id?}")]
        public ActionResult Options(int id) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            //Tune off payments
            //ViewBag.IsMobile = true;

            ReservationModel reservation = getReservation(id);
            if (reservation != null) {

                ReservationAddBenefitsCommand addBenefits = new ReservationAddBenefitsCommand { ReservationId = reservation.Id };
                _commandBus.Submit(addBenefits);
                reservation = getReservation(id);
                CheckoutOptionsViewModel model = new CheckoutOptionsViewModel();
                model.Criteria = new SearchCriteriaViewModel { Criteria = getSearchCriteriaModelFromReservation(reservation) };
                model.Criteria.PickupDate = model.Criteria.Criteria.PickupDate;
                model.Criteria.DropOffDate = model.Criteria.Criteria.DropOffDate;
                model.Reservation = reservation;
                model.Vehicle = getVehicle(reservation.VehicleId);
                model.SetExtras(getExtras());

                var currentUser = _query.Process(new UserGetCurrentQuery { });
                if (currentUser != null) {
                    if (currentUser.IsLoyaltyMember) {

                        LoyaltyOverviewModel loyaltyOverview = _query.Process(new LoyaltyOverviewByUserIdQuery { UserId = currentUser.Id });
                        if (loyaltyOverview.PointsRemaining > 0) {
                            model.IsLoyaltyUserWithPoints = true;
                            model.LoyaltyPointsAvailable = loyaltyOverview.PointsRemaining;
                        }

                    }

                }
                return View(model);
            }
            else {
                CheckoutViewModelBase baseModel = new CheckoutViewModelBase { ErrorState = CheckoutReservationErrors.Rate_No_Longer_Available };
                return View("InvalidRateOrVehicle", baseModel);
            }

        }

        [CheckLeadTimeFilter]
        [CheckQuoteExpiredFilter]
        [Route("checkout/options/{id?}")]
        [HttpPost]
        public ActionResult Options(int id, CheckoutOptionsViewModel model, FormCollection fc) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            //Tune off payments
            //ViewBag.IsMobile = true;

            bool voucherCanBeUsed = false;

            if (!string.IsNullOrEmpty(model.Reservation.VoucherNumber)) {
                var voucher = _query.Process(new VouchersGetQuery { Filter = new VoucherFilterModel { VoucherNumber = model.Reservation.VoucherNumber } }).Items.SingleOrDefault();
                if (voucher != null) {
                    if (!voucher.IsMultiUse) {
                        if (voucher.DateRedeemed.HasValue) {
                            ModelState.AddModelError("Reservation.VoucherNumber", "Voucher has been redeemed");
                        }
                        else {
                            voucherCanBeUsed = true;
                        }
                    }
                    else {
                        voucherCanBeUsed = true;
                    }


                }
                else {
                    ModelState.AddModelError("Reservation.VoucherNumber", "Voucher is invalid");

                }
                if (voucherCanBeUsed) {
                    ReservationSetVoucherCommand voucherCommand = new ReservationSetVoucherCommand { ReservationId = id, VoucherId = voucher.Id };
                    _commandBus.Submit(voucherCommand);
                }
            }

            var currentUser = _query.Process(new UserGetCurrentQuery { });
            bool loyaltyPointsAllowed = false;
            decimal? loyaltyPointsSpent = model.Reservation.LoyaltyPointsSpent;

            ReservationModel reservation = getReservation(id);

            if (currentUser != null) {
                if (currentUser.IsLoyaltyMember) {


                    if (currentUser.LoyaltyPointsRemaining > 0) {
                        model.IsLoyaltyUserWithPoints = true;
                        model.LoyaltyPointsAvailable = currentUser.LoyaltyPointsRemaining;
                        if (model.Reservation.LoyaltyPointsSpent > 0) {
                            if (currentUser.LoyaltyPointsRemaining < model.Reservation.LoyaltyPointsSpent) {
                                ModelState.AddModelError("Reservation.LoyaltyPointsSpent", "Not enough loyalty points");

                            }
                            if (model.Reservation.LoyaltyPointsSpent > reservation.BookingPriceWithoutLoyaltyPoints) {
                                ModelState.AddModelError("Reservation.LoyaltyPointsSpent", "More points have been assigned than required.");
                            }
                            else {
                                loyaltyPointsAllowed = true;
                            }
                        }
                    }

                }
            }



            model.Reservation = reservation;

            model.Criteria = new SearchCriteriaViewModel { Criteria = getSearchCriteriaModelFromReservation(reservation) };
            model.Criteria.PickupDate = model.Criteria.Criteria.PickupDate;
            model.Criteria.DropOffDate = model.Criteria.Criteria.DropOffDate;

            model.Reservation = reservation;
            model.Vehicle = getVehicle(reservation.VehicleId);
            model.SetExtras(getExtras());

            if (ModelState.IsValid) {

                if (!loyaltyPointsAllowed) {
                    model.Reservation.LoyaltyPointsSpent = 0;
                }
                else {
                    //Set loyalty points for reservation
                    model.Reservation.LoyaltyPointsSpent = loyaltyPointsSpent;
                    _commandBus.Submit(new ReservationEditCommand { Reservation = model.Reservation });
                }


                var vehicleExtraIds = new List<int>();
                foreach (var extra in model.Extras) {
                    if (fc["chkVehicleExtras_" + extra.Id.ToString()] != null) {
                        if (fc["chkVehicleExtras_" + extra.Id.ToString()] == "true,false" || fc["chkVehicleExtras_" + extra.Id.ToString()] == "true") {
                            vehicleExtraIds.Add(extra.Id);
                        }
                    }
                }
                ReservationAddExtrasCommand command = new ReservationAddExtrasCommand { ReservationId = id, ExtraIds = vehicleExtraIds };
                _commandBus.Submit(command);

                return RedirectToAction("VehicleUpgrade", new { id = id });
            }
            else {



                return View(model);
            }

        }

        [CheckLeadTimeFilter]
        [CheckQuoteExpiredFilter]
        public ActionResult UserDetails(int id) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            //Tune off payments
            //ViewBag.IsMobile = true;

            UserModel u = getCurrentUser();
            if (u != null) {

                ReservationAssignUserCommand assignUserCommand = new ReservationAssignUserCommand { ReservationId = id, UserId = u.Id };
                _commandBus.Submit(assignUserCommand);
                ReservationAddBenefitsCommand addBenefits = new ReservationAddBenefitsCommand { ReservationId = id };
                _commandBus.Submit(addBenefits);
                return RedirectToAction("Options", new { id = id });
            }

            ReservationModel r = getReservation(id);

            ViewBag.QuoteExpired = false;
            if (r.DateCreated < DateTime.Today.AddDays(-7)) {
                ViewBag.QuoteExpired = true;
            }
            ReservationUserLoginOrRegistrationViewModel model = new ReservationUserLoginOrRegistrationViewModel();
            model.ReservationId = id;
            if (r.UserId == null && !string.IsNullOrEmpty(r.FirstName)) {
                model.User = new UserModel();
                model.User.FirstName = r.FirstName;
                model.User.LastName = r.LastName;
                model.User.Email = r.Email;
                model.User.MobileNumber = r.MobileNumber;
                model.User.IdNumber = r.IdNumber;
            }

            ReservationModel reservation = getReservation(id);
            model.Reservation = reservation;
            model.Vehicle = getVehicle(reservation.VehicleId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(int id, [CustomizeValidator(RuleSet = ReservationUserLoginOrRegistrationValidationRuleSets.Default)] ReservationUserLoginOrRegistrationViewModel model) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            if (ModelState.IsValid) {

                bool accountBlocked = false;
                //assign guest details to reservation
                var user = _query.Process(new UserGetByUsernameQuery { UserName = model.LoginDetails.Email });
                if (user != null) {
                    if (user.IsAccountDisabled) {
                        accountBlocked = true;
                        ModelState.AddModelError("LoginDetails.Password", "Account Blocked");

                    }
                }

                if (!accountBlocked) {
                    UserAuthenticateCommand command = new UserAuthenticateCommand { Username = model.LoginDetails.Email, Password = model.LoginDetails.Password };
                    _commandBus.Submit(command);

                    ReservationAddBenefitsCommand addBenefits = new ReservationAddBenefitsCommand { ReservationId = id };
                    _commandBus.Submit(addBenefits);

                    if (command.Success) {

                        UserGetByUsernameQuery query = new UserGetByUsernameQuery { UserName = model.LoginDetails.Email };
                        UserModel currentUser = _query.Process(query);

                        ReservationAssignUserCommand assignUserCommand = new ReservationAssignUserCommand { ReservationId = id, UserId = currentUser.Id };
                        _commandBus.Submit(assignUserCommand);

                        return RedirectToAction("Options", new { id = id });
                    }
                    else {
                        ModelState.AddModelError("LoginDetails.Password", "Email or Password is incorrect");
                    }
                }

            }
            ReservationModel reservation = getReservation(id);
            model.Reservation = reservation;
            model.Vehicle = getVehicle(reservation.VehicleId);
            return View("UserDetails", model);
        }

        [HttpGet]
        public ActionResult EnterDetails(int id) {
            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }


            return RedirectToAction("UserDetails", new { id = id });
        }

        [HttpPost]
        public ActionResult EnterDetails(int id, [CustomizeValidator(RuleSet = ReservationUserLoginOrRegistrationValidationRuleSets.Default)] ReservationUserLoginOrRegistrationViewModel model) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            if (ModelState.IsValid) {

                if (model.IsCreateAccount) {
                    UserRegisterCommand command = new UserRegisterCommand { User = model.User, Password = model.Password };
                    _commandBus.Submit(command);

                    ReservationAddBenefitsCommand addBenefits = new ReservationAddBenefitsCommand { ReservationId = id };
                    _commandBus.Submit(addBenefits);

                    if (command.Success) {

                        UserGetByUsernameQuery query = new UserGetByUsernameQuery { UserName = model.User.Email };
                        UserModel currentUser = _query.Process(query);

                        ReservationAssignUserCommand assignUserCommand = new ReservationAssignUserCommand { ReservationId = id, UserId = currentUser.Id };
                        _commandBus.Submit(assignUserCommand);

                        return RedirectToAction("Options", new { id = id });
                    }
                    else {
                        ModelState.AddModelError("User.Email", "An error occurred");
                    }
                }
                else {
                    bool accountBlocked = false;
                    //assign guest details to reservation
                    var user = _query.Process(new UserGetByUsernameQuery { UserName = model.User.Email });
                    if (user != null) {
                        if (user.IsAccountDisabled) {
                            accountBlocked = true;
                            ModelState.AddModelError("User.Email", "Account Blocked");

                        }
                    }

                    if (!accountBlocked) {
                        ReservationAssignGuestUserDetailsCommand assignGuestUserCommand = new ReservationAssignGuestUserDetailsCommand { ReservationId = id, FirstName = model.User.FirstName, LastName = model.User.LastName, Email = model.User.Email, IdNumber = model.User.IdNumber, MobileNumber = model.User.MobileNumber };
                        _commandBus.Submit(assignGuestUserCommand);

                        return RedirectToAction("Options", new { id = id });
                    }

                }

            }
            ReservationModel reservation = getReservation(id);
            model.Reservation = reservation;
            model.Vehicle = getVehicle(reservation.VehicleId);
            return View("UserDetails", model);
        }

        [CheckLeadTimeFilter]
        [CheckQuoteExpiredFilter]
        public ActionResult VehicleUpgrade(int id) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            var currentUser = _query.Process(new UserGetCurrentQuery { });

            ReservationAddBenefitsCommand addBenefits = new ReservationAddBenefitsCommand { ReservationId = id };
            _commandBus.Submit(addBenefits);

            CheckoutVehicleUpgradeViewModel model = new CheckoutVehicleUpgradeViewModel();
            model.Reservation = getReservation(id);



            var rateCode = _query.Process(new RateCodeGetByIdQuery { Id = model.Reservation.RateCodeId });
            if (rateCode.CanHaveUpgradeApplied) {
                model.SetUpgrades(getVehicleUpgrades(model.Reservation.VehicleId, model.Reservation.PickupBranchId, model.Reservation.PickupDate));
                model.VehicleUpgradeId = (model.Reservation.VehicleUpgradeId ?? 0);
                // model.Reservation = getReservation(id);
            }
            else {
                return RedirectToAction("Confirm", new { id = id });
            }




            if (model.VehicleUpgrades.Count == 0) {
                //if (currentUser.IsLoyaltyMember) {
                //    var benefits = _query.Process(new LoyaltyTierGetBenefitsQuery {  Level = (LoyaltyTierLevel)currentUser.LoyaltyTierId }).Items.Where(x=>x.BenefitType == BenefitType.Upgrades && model.VehicleUpgrades.Select(y=>y.FromVehicleId).Contains x.UpgradeId)
                //}
                return RedirectToAction("Confirm", new { id = id });
            }
            else {
                model.Criteria = new SearchCriteriaViewModel { Criteria = getSearchCriteriaModelFromReservation(model.Reservation) };
                model.Criteria.PickupDate = model.Criteria.Criteria.PickupDate;
                model.Criteria.DropOffDate = model.Criteria.Criteria.DropOffDate;
                model.Vehicle = getVehicle(model.Reservation.VehicleId);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult VehicleUpgrade(int id, CheckoutVehicleUpgradeViewModel model) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }


            //repopulate so we can see if the user's selection is a valid from the list of upgrades (not hacked via inspect element / firebug / similar)
            model.Reservation = getReservation(id);
            model.SetUpgrades(getVehicleUpgrades(model.Reservation.VehicleId, model.Reservation.PickupBranchId, model.Reservation.PickupDate));

            model.Criteria = new SearchCriteriaViewModel { Criteria = getSearchCriteriaModelFromReservation(model.Reservation) };
            model.Criteria.PickupDate = model.Criteria.Criteria.PickupDate;
            model.Criteria.DropOffDate = model.Criteria.Criteria.DropOffDate;
            model.Vehicle = getVehicle(model.Reservation.VehicleId);

            if (ModelState.IsValid) {


                decimal upgradePrice = 0m;
                if (model.VehicleUpgradeId > 0) {
                    var selectedUpgrade = model.VehicleUpgrades.Where(x => x.Id == model.VehicleUpgradeId).SingleOrDefault();
                    upgradePrice = selectedUpgrade.UpgradeAmount;

                    ReservationSetUpgradeCommand command = new ReservationSetUpgradeCommand { ReservationId = id, UpgradeId = model.VehicleUpgradeId, UpgradePrice = upgradePrice };
                    _commandBus.Submit(command);

                }


                return RedirectToAction("Confirm", new { id = id });

            }
            return View(model);
        }

        [CheckLeadTimeFilter]
        [CheckQuoteExpiredFilter]
        public ActionResult Confirm(int id) {

            ViewBag.IsMobile = false;
            ViewBag.ShowReturnToOptions = false;

            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            //Testing
            //ViewBag.IsMobile = true;


            CheckoutConfirmViewModel viewModel = new CheckoutConfirmViewModel();
            viewModel.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                viewModel.IsMobile = true;
            }

            

            var currentUser = _query.Process(new UserGetCurrentQuery());
            if (currentUser != null) {
                if (currentUser.CorporateId.HasValue) {
                    viewModel.IsCorporate = true;
                }
            }
            //if (User.IsInRole("Administrator")) {
            //    ViewBag.IsMobile = false;
            //    viewModel.IsMobile = false;
            //}
            //else {
            //    ViewBag.IsMobile = true;
            //    viewModel.IsMobile = true;
            //}

            var reservation = getReservation(id);

            //Better booking frequency was experienced when user's did not have to pay on checkout. Essa requested that bookings in the next 5 days do not have to pay at checkout
            if (reservation.PickupDate < DateTime.Today.AddDays(5)) {
                ViewBag.IsMobile = true;
                viewModel.IsMobile = true;
            }


            viewModel.Reservation = reservation;

            ReservationAddBenefitsCommand addBenefits = new ReservationAddBenefitsCommand { ReservationId = viewModel.Reservation.Id };
            _commandBus.Submit(addBenefits);

            viewModel.Vehicle = getVehicle(viewModel.Reservation.VehicleId);
            if (viewModel.Reservation.VehicleUpgradeId.HasValue) {
                VehicleUpgradeModel u = getVehicleUpgrade(viewModel.Reservation.VehicleUpgradeId.Value);
                viewModel.UpgradeToVehicle = getVehicle(u.ToVehicle.Id);
                viewModel.Reservation.VehicleUpgrade = viewModel.UpgradeToVehicle;
            }

            viewModel.RentalDepositAmount = Convert.ToDecimal(getSetting(Setting.Rental_Deposit_Amount));
            viewModel.Criteria = new SearchCriteriaViewModel { Criteria = getSearchCriteriaModelFromReservation(viewModel.Reservation) };
            viewModel.Criteria.PickupDate = viewModel.Criteria.Criteria.PickupDate;
            viewModel.Criteria.DropOffDate = viewModel.Criteria.Criteria.DropOffDate;
            viewModel.Vehicle = getVehicle(viewModel.Reservation.VehicleId);
            viewModel.Waivers = getWaivers(viewModel.Vehicle.VehicleGroupId);

            return View(viewModel);
        }

        [CheckLeadTimeFilter]
        [CheckQuoteExpiredFilter]
        [HttpPost]
        public ActionResult Confirm(int id, CheckoutConfirmViewModel viewModel, FormCollection fc) {
            ViewBag.IsMobile = false;
            ViewBag.ShowReturnToOptions = false;

            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }
            //ViewBag.IsMobile = true;
            var reservation = getReservation(id);
            var currentUser = getCurrentUser();



            if (currentUser != null) {
                var loyaltyOverview = _query.Process(new LoyaltyOverviewByUserIdQuery { UserId = currentUser.Id });
                if (reservation.LoyaltyPointsSpent > loyaltyOverview.PointsRemaining) {
                    ModelState.AddModelError("", "Insufficient Advance Points");
                    ViewBag.ShowReturnToOptions = true;
                }
            }

            applyCountdownSpecialToReservation(id);

            if (viewModel.IsCorporate) {
                //Must validate the fields below

                if (string.IsNullOrEmpty(viewModel.Reservation.FirstName)) {
                    ModelState.AddModelError("Reservation_FirstName", " First Name Required");
                }

                if (string.IsNullOrEmpty(viewModel.Reservation.LastName)) {
                    ModelState.AddModelError("Reservation_LastName", "Last Name Required");
                }

                if (string.IsNullOrEmpty(viewModel.Reservation.Email)) {
                    ModelState.AddModelError("Reservation_Email", "Email Required");
                }

                if (string.IsNullOrEmpty(viewModel.Reservation.MobileNumber)) {
                    ModelState.AddModelError("Reservation_MobileNumber", "Mobile Required");
                }

                if (string.IsNullOrEmpty(viewModel.Reservation.IdNumber)) {
                    ModelState.AddModelError("Reservation_IdNumber", "ID Number Required");
                }
            }

            if (ModelState.IsValid) {
                if (fc["confirmNoPayment"] != null) {
                    //if Corporate then we need to set driver details
                    if (viewModel.IsCorporate) {


                        ReservationAssignGuestUserDetailsCommand command = new ReservationAssignGuestUserDetailsCommand();
                        command.ReservationId = viewModel.Reservation.Id;
                        command.FirstName = viewModel.Reservation.FirstName;
                        command.LastName = viewModel.Reservation.LastName;
                        command.Email = viewModel.Reservation.Email;
                        command.IdNumber = viewModel.Reservation.IdNumber;


                        _commandBus.Submit(command);
                    }

                    //Check if mobile or corporate or both
                    InvoiceModel inv = markReservationAsPaid(id, isMobile: viewModel.IsMobile, isCorporate: viewModel.IsCorporate, amountPaid: 0, transaction: null, notransaction: true);
                    return RedirectToAction("Success", new { id = id });
                }

                if (fc["confirmFull"] != null) {
                    return RedirectToAction("Payment", new { id = id, type = "full" });

                }
                if (fc["confirmDeposit"] != null) {
                    return RedirectToAction("Payment", new { id = id, type = "deposit" });
                }
            }

            viewModel.Reservation = getReservation(id);

            ReservationAddBenefitsCommand addBenefits = new ReservationAddBenefitsCommand { ReservationId = viewModel.Reservation.Id };
            _commandBus.Submit(addBenefits);

            viewModel.Vehicle = getVehicle(viewModel.Reservation.VehicleId);
            if (viewModel.Reservation.VehicleUpgradeId.HasValue) {
                VehicleUpgradeModel u = getVehicleUpgrade(viewModel.Reservation.VehicleUpgradeId.Value);
                viewModel.UpgradeToVehicle = getVehicle(u.ToVehicle.Id);
            }



            viewModel.RentalDepositAmount = Convert.ToDecimal(getSetting(Setting.Rental_Deposit_Amount));
            viewModel.Criteria = new SearchCriteriaViewModel { Criteria = getSearchCriteriaModelFromReservation(viewModel.Reservation) };
            viewModel.Criteria.PickupDate = viewModel.Criteria.Criteria.PickupDate;
            viewModel.Criteria.DropOffDate = viewModel.Criteria.Criteria.DropOffDate;
            viewModel.Vehicle = getVehicle(viewModel.Reservation.VehicleId);
            viewModel.Waivers = getWaivers(viewModel.Vehicle.VehicleGroupId);

            return View(viewModel);
        }

        [CheckLeadTimeFilter]
        [CheckQuoteExpiredFilter]
        public ActionResult Payment(int id, int error = 0) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            ViewBag.InvoiceAlreadyPaid = false;
            ViewBag.show3Dsecure = false;

            var model = new CheckoutPaymentViewModel();

            model.Reservation = getReservation(id);
            model.Reservation.Invoice = getInvoiceForReservation(model.Reservation.Id);
            if (model.Reservation.Invoice != null) {
                if (model.Reservation.Invoice.IsCompleted) {
                    return RedirectToAction("AlreadyPaid", new { id = id });
                }
            }


            ReservationAddBenefitsCommand addBenefits = new ReservationAddBenefitsCommand { ReservationId = model.Reservation.Id };
            _commandBus.Submit(addBenefits);

            InvoiceModel invoice = getInvoiceForReservation(id);
            if (invoice != null) {
                if (invoice.IsCompleted)
                    ViewBag.InvoiceAlreadyPaid = true;
            }

            model.Criteria = new SearchCriteriaViewModel { Criteria = getSearchCriteriaModelFromReservation(model.Reservation) };
            model.Criteria.PickupDate = model.Criteria.Criteria.PickupDate;
            model.Criteria.DropOffDate = model.Criteria.Criteria.DropOffDate;

            //model.Vehicle = getVehicle(model.Reservation.VehicleId.Value);
            //model.RatePrice = getRatePriceModel(model.SearchCriteria, model.Reservation.VehicleRateId.Value, model.Reservation.VehicleExtras, model.Reservation.PinnacleDiscountPercentage ?? 0, model.Reservation.PinnacleNumberVerified, model.Reservation.VoucherDiscount ?? 0, 0);


            ViewBag.RequestType = Request["type"];
            decimal bookingTotal = model.Reservation.BookingPrice;

            decimal depositAmount = Convert.ToDecimal(getSetting(Setting.Rental_Deposit_Amount));


            if (Request["type"] == "full") {
                model.Amount = string.Format("{0:c}", bookingTotal);
                model.IsDepositPayment = false;
            }
            else {

                model.Amount = string.Format("{0:c}", depositAmount);
                model.IsDepositPayment = true;
            }


            if (error == 1) {
                ViewBag.ShowError = true;
            }
            else {
                ViewBag.ShowError = false;
            }

            if (model.Reservation.HasPaymentError) {
                ViewBag.ShowError = true;
            }

            return View(model);
        }




        [HttpPost]
        public ActionResult Payment([CustomizeValidator(RuleSet = CheckoutValidationRuleSets.Default)] CheckoutPaymentViewModel model, FormCollection fc) {

            if (model.CardType == 1) {
                //Amex card, mark as paid with 0, then send to success as we cannot process Amex online, only at branch
                InvoiceModel inv = markReservationAsPaid(model.Reservation.Id, isMobile: true, isCorporate: false, amountPaid: 0, transaction: null, notransaction: true);
                return RedirectToAction("Success", new { id = model.Reservation.Id });
            }

            ViewBag.ShowError = false;

            ISettingService _settings = MvcApplication.Container.GetInstance<ISettingService>();
            int leadTime = _settings.GetValue<int>(Setting.Booking_Lead_Time_Hours);

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            ViewBag.InvoiceAlreadyPaid = false;
            ViewBag.show3Dsecure = false;

            ViewBag.RequestType = Request["type"];
            int id = model.Reservation.Id;

            InvoiceModel invoice = getInvoiceForReservation(id);
            if (invoice != null) {
                if (invoice.IsCompleted)
                    ViewBag.InvoiceAlreadyPaid = true;
            }

            model.Reservation = getReservation(id);

            if (model.Reservation.PickupDate.AddHours(leadTime * -1) < DateTime.Now) {
                return RedirectToAction("QuoteExpiredNoLeadTime");
            }

            PaymentRequestModel requestModel = new PaymentRequestModel();

            decimal bookingTotal = model.Reservation.BookingPrice;
            if (!model.IsDepositPayment) {
                requestModel.Amount = bookingTotal;
                model.Amount = string.Format("{0:c}", bookingTotal);

            }
            else {
                decimal depositAmount = Convert.ToDecimal(getSetting(Setting.Rental_Deposit_Amount));
                requestModel.Amount = depositAmount;
                model.Amount = string.Format("{0:c}", depositAmount);
            }

            if (ModelState.IsValid) {

                //PaymentRequestModel requestModel = new PaymentRequestModel();

                //ViewBag.RequestType = Request["type"];
                //decimal bookingTotal = model.Reservation.BookingPrice;
                //if (Request["type"] == "full") {
                //    requestModel.Amount = bookingTotal;
                //} else {
                //    decimal depositAmount = Convert.ToDecimal(getSetting(Setting.Rental_Deposit_Amount));
                //    requestModel.Amount = depositAmount;
                //}
                requestModel.CardDetails = new PaymentCardModel {
                    CardNumber = model.CardNumber.Replace(" ", ""),
                    CardType = (PaymentCardType)model.CardType,
                    NameOnCard = model.CardHolderName,
                    CardExpiryMonth = model.ExpiryMonth,
                    CardExpiryYear = model.ExpiryYear,
                    CVV = model.CVV
                };
                requestModel.ReservationId = id;


                bool is3DPostback = (fc["Is3DPostback"] == "1");
                bool use3DSecure = getSetting(Setting.Payment_Gateway_Use_3D_Secure).ToLower() == "true";

                //create invoice at beginning of transaction
                InvoiceUpsertCommand upsertInvoiceCommand = new InvoiceUpsertCommand {
                    Invoice = new InvoiceModel {
                        AmountPaid = requestModel.Amount,
                        IsCompleted = false,
                        IsMobileCheckout = false,
                        ReservationId = model.Reservation.Id
                    }
                };
                _commandBus.Submit(upsertInvoiceCommand);

                invoice = upsertInvoiceCommand.Invoice;

                if (invoice != null) {
                    if (invoice.IsCompleted) {
                        ModelState.AddModelError("", "Reservation already paid for or confirmed");
                    }
                }


                //if (is3DPostback) {
                PaymentProcessCommand payCommand = new PaymentProcessCommand { PayRequest = requestModel, Invoice = invoice };
                _commandBus.Submit(payCommand);


                if (payCommand.PayResponse.State == PaymentResponseState.Success) {
                    return RedirectToAction("Success", new { id = model.Reservation.Id });
                }
                if (payCommand.PayResponse.State == PaymentResponseState.Required3DSecure) {
                    //show 3d secure form
                    ViewBag.show3Dsecure = true;
                    ViewBag.acsUrl = payCommand.PayResponse.Secure3DCheckUrl;
                    //ViewBag.parEq = HttpUtility.UrlEncode(lookupCommand.LookupResponse.ParEqMsg);
                    //ViewBag.transactionIndex = lookupCommand.LookupResponse.TransactionIndex.ToString();
                    ViewBag.FrameParams = payCommand.PayResponse.Secure3DParameters;

                    return View(model);
                }

                if (!payCommand.PayResponse.Processed) {
                    ModelState.AddModelError("", payCommand.PayResponse.ErrorMessage);
                }
                else {
                    return RedirectToAction("Success", new { id = model.Reservation.Id });
                }
                //}
                //else {
                //    if (use3DSecure) {
                //        Payment3DSecureLookupCommand lookupCommand = new Payment3DSecureLookupCommand { PayRequest = requestModel };
                //        _commandBus.Submit(lookupCommand);
                //        if (lookupCommand.LookupResponse.Enrolled) {
                //            //show 3d secure form
                //            ViewBag.show3Dsecure = true;
                //            ViewBag.acsUrl = lookupCommand.LookupResponse.ACSUrl;
                //            ViewBag.parEq = HttpUtility.UrlEncode(lookupCommand.LookupResponse.ParEqMsg);
                //            ViewBag.transactionIndex = lookupCommand.LookupResponse.TransactionIndex.ToString();
                //            return View(model);
                //        }
                //        else {
                //            PaymentProcessCommand payCommand = new PaymentProcessCommand { PayRequest = requestModel, Invoice = invoice, TransactionId = lookupCommand.LookupResponse.TransactionIndex.ToString() };
                //            _commandBus.Submit(payCommand);
                //            if (!payCommand.PayResponse.Processed) {
                //                ModelState.AddModelError("", payCommand.PayResponse.ErrorMessage);
                //            }
                //            else {
                //                return RedirectToAction("Success", new { id = model.Reservation.Id });
                //            }

                //        }
                //    }
                //}


                //requestModel.Is3DSecurePostback = (fc["Is3DPostback"] == "1");

                //PaymentPayCommand command = new PaymentPayCommand { PayRequest = requestModel };
                //_commandBus.Submit(command);
                //PaymentResponseModel payResponse = command.PayResponse;

                //switch (payResponse.State) {
                //    case PaymentResponseState.Success:
                //        return RedirectToAction("Success", new { id = id });


                //    case PaymentResponseState.Error:
                //        ModelState.AddModelError("", payResponse.ErrorMessage);
                //        break;

                //    case PaymentResponseState.Required3DSecure:
                //        //show 3d secure form
                //        ViewBag.show3Dsecure = true;
                //        ViewBag.acsUrl = command.Pay3DSecureResponse.ACSUrl;
                //        ViewBag.parEq = HttpUtility.UrlEncode(command.Pay3DSecureResponse.ParEqMsg);
                //        ViewBag.transactionIndex = command.Pay3DSecureResponse.TransactionIndex.ToString();
                //        return View(model);

                //}
            }

            return View(model);
        }

        public ActionResult Success(int id) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }


            CheckoutSuccessViewModel model = new CheckoutSuccessViewModel();
            model.Reservation = getReservation(id);

            if (model.Reservation.HasPaymentError) {
                return RedirectToAction("Failed", new { id = id });
            }

            model.Invoice = getInvoiceForReservation(id);
            model.Vehicle = getVehicle(model.Reservation.VehicleId);
            model.Waivers = getWaivers(model.Vehicle.VehicleGroupId);

            if (model.Reservation.VehicleUpgradeId.HasValue) {
                VehicleUpgradeModel u = getVehicleUpgrade(model.Reservation.VehicleUpgradeId.Value);
                model.UpgradeToVehicle = getVehicle(u.ToVehicle.Id);
                model.Reservation.VehicleUpgrade = model.UpgradeToVehicle;
            }

            model.Criteria = new SearchCriteriaViewModel { Criteria = getSearchCriteriaModelFromReservation(model.Reservation) };
            model.Criteria.PickupDate = model.Criteria.Criteria.PickupDate;
            model.Criteria.DropOffDate = model.Criteria.Criteria.DropOffDate;
            model.Vehicle = getVehicle(model.Reservation.VehicleId);

            return View(model);
        }

        public ActionResult Failed(int id) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }


            CheckoutSuccessViewModel model = new CheckoutSuccessViewModel();
            model.Reservation = getReservation(id);
            model.Invoice = getInvoiceForReservation(id);
            model.Vehicle = getVehicle(model.Reservation.VehicleId);
            model.Waivers = getWaivers(model.Vehicle.VehicleGroupId);

            if (model.Reservation.VehicleUpgradeId.HasValue) {
                VehicleUpgradeModel u = getVehicleUpgrade(model.Reservation.VehicleUpgradeId.Value);
                model.UpgradeToVehicle = getVehicle(u.ToVehicle.Id);
                model.Reservation.VehicleUpgrade = model.UpgradeToVehicle;
            }

            model.Criteria = new SearchCriteriaViewModel { Criteria = getSearchCriteriaModelFromReservation(model.Reservation) };
            model.Criteria.PickupDate = model.Criteria.Criteria.PickupDate;
            model.Criteria.DropOffDate = model.Criteria.Criteria.DropOffDate;
            model.Vehicle = getVehicle(model.Reservation.VehicleId);

            return View(model);
        }

        public ActionResult PostTo3DSecure(string acs, string parEqMsg, string tindex) {
            ViewBag.ACSUrl = acs;
            ViewBag.ParEqMsg = parEqMsg;
            ViewBag.ACSCallbackUrl = getSetting(Setting.Payment_3D_Secure_Callback_URL);
            ViewBag.TransactionIndex = tindex;
            return View();
        }

        [HttpPost]
        public ContentResult Notify(int id, FormCollection fc) {
            var reservation = _query.Process(new ReservationGetByIdQuery { Id = Convert.ToInt32(id) });

            IReservationRepository _resRepo = MvcApplication.Container.GetInstance<IReservationRepository>();

            string status = fc["TRANSACTION_STATUS"];
            ViewBag.Status = status;
            if (status == "1") {
                //Success
                ReservationMarkAsPaidCommand markAsPaid = new ReservationMarkAsPaidCommand();
                markAsPaid.AmountPaid = reservation.Invoice.AmountPaid;
                var currentUser = getCurrentUser();
                if (currentUser != null) {
                    if (currentUser.CorporateId.HasValue) {
                        markAsPaid.IsCorporate = true;
                    }
                }
                markAsPaid.ReservationId = reservation.Id;
                markAsPaid.PaymentTransaction = new PaymentTransactionModel();
                markAsPaid.PaymentTransaction.TransactionId = fc["PAY_REQUEST_ID"];

                _commandBus.Submit(markAsPaid);

                reservation.HasPaymentError = false;
                reservation.PaymentErrorMessage = "";

                _resRepo.AddPaymentError(reservation);


            }
            else {
                //Set Error on Invoice

                reservation.HasPaymentError = true;
                if (status == "2") {
                    reservation.PaymentErrorCode = "2";
                    reservation.PaymentErrorMessage = "There was an issue with your card";
                }
                if (status == "0") {
                    reservation.PaymentErrorCode = "0";
                    reservation.PaymentErrorMessage = "There was an issue with our payment provider";
                }
                _resRepo.AddPaymentError(reservation);
            }
            //PAY_REQUEST_ID
            //TRANSACTION_STATUS
            //CHECKSUM
            return Content("OK");
        }


        public ActionResult Checkout3DCallback(string id) {


            PayGate3D model = new PayGate3D();
            model.ResId = id;
            return View(model);

        }

        [HttpGet]
        public ActionResult SendQuoteContactDetails(int id) {
            ReservationQuoteContactDetailsModel model = new ReservationQuoteContactDetailsModel();
            model.ReservationId = id;

            return View(model);
        }

        [HttpPost]
        public ActionResult SendQuoteContactDetails(int id, ReservationQuoteContactDetailsModel model) {

            if (ModelState.IsValid) {
                ReservationAssignGuestUserDetailsCommand assignGuestUserCommand = new ReservationAssignGuestUserDetailsCommand { ReservationId = id, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, IdNumber = "", MobileNumber = "" };
                _commandBus.Submit(assignGuestUserCommand);

                NotifyReservationQuoteCommand command = new NotifyReservationQuoteCommand();
                command.ReservationId = id;
                command.CheckoutStep = 2;
                _commandBus.Submit(command);

                return RedirectToAction("SendQuoteComplete", new { id = id });
            }

            return View(model);
        }

        public ActionResult SendQuoteComplete(int id) {
            ReservationModel model = _query.Process(new ReservationGetByIdQuery { Id = id });
            return View(model);
        }
        public ActionResult SendQuote(int id) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }

            NotifyReservationQuoteCommand command = new NotifyReservationQuoteCommand();
            command.ReservationId = id;
            _commandBus.Submit(command);
            ReservationModel model = _query.Process(new ReservationGetByIdQuery { Id = id });
            return RedirectToAction("SendQuoteComplete", new { id = id });
        }

        public ActionResult Print(int id) {

            ViewBag.IsMobile = false;


            if (mobileDevices.Any(x => Request.UserAgent.ToLower().Contains(x))) {
                ViewBag.IsMobile = true;
            }


            ReservationInvoiceNotificationModel viewModel = new ReservationInvoiceNotificationModel();
            viewModel.Reservation = _query.Process(new ReservationGetByIdQuery { Id = id });
            //viewModel.Reservation = _query.Process(new ReservationsGetQuery { Filter = new ReservationFilterModel { Id = id }, Pagination = null }).Items.First();
            viewModel.Invoice = viewModel.Reservation.Invoice;
            if (viewModel.Invoice == null) {
                viewModel.Invoice = new InvoiceModel();
                viewModel.Reservation.Invoice = new InvoiceModel();
            }
            return View(viewModel);
        }

        public ActionResult QuoteExpired() {
            ViewBag.PanelMinimised = false;
            return View();
        }

        public ActionResult QuoteExpiredNoLeadTime() {
            ViewBag.PanelMinimised = false;
            return View();
        }
        #region helper methods

        private InvoiceModel markReservationAsPaid(int reservationId, bool isMobile, bool isCorporate, decimal amountPaid, PaymentTransactionModel transaction, bool notransaction = false) {
            ReservationMarkAsPaidCommand command = new ReservationMarkAsPaidCommand { NoTransaction = notransaction, ReservationId = reservationId, IsMobileDevice = isMobile, IsCorporate = isCorporate, AmountPaid = amountPaid, PaymentTransaction = transaction };
            _commandBus.Submit(command);
            return command.InvoiceOut;
        }

        private UserModel getCurrentUser() {
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            return _query.Process(query);
        }

        private ReservationModel getReservation(int id) {
            ReservationGetByIdQuery query = new ReservationGetByIdQuery { Id = id };
            return _query.Process(query);
        }

        private VehicleModel getVehicle(int id) {
            VehicleGetByIdQuery query = new VehicleGetByIdQuery { Id = id, includePageContent = false };
            return _query.Process(query);
        }

        private List<VehicleExtrasModel> getExtras() {
            VehicleExtrasGetQuery query = new VehicleExtrasGetQuery();
            return _query.Process(query);
        }

        private SearchCriteriaModel getSearchCriteriaModelFromReservation(ReservationModel r) {
            SearchCriteriaModel res = new SearchCriteriaModel();
            res.PickupDate = r.PickupDate.Date;
            res.PickupTime = r.PickupDate.Hour;
            res.PickupTimeFull = DateTime.Today.AddHours(res.PickupTime).ToString("hh:00 tt");

            res.DropOffDate = r.DropOffDate.Date;
            res.DropOffTime = r.DropOffDate.Hour;
            res.DropOffTimeFull = DateTime.Today.AddHours(res.DropOffTime).ToString("hh:00 tt");

            res.PickUpLocationId = r.PickupBranchId;
            res.DropOffLocationId = r.DropOffBranchId;

            return res;
        }

        private List<VehicleUpgradeModel> getVehicleUpgrades(int vehicleId, int branchId, DateTime pickupDate) {
            VehicleUpgradesGetQuery query = new VehicleUpgradesGetQuery { Filter = new VehicleUpgradeFilterModel { FromVehicleId = vehicleId, BranchId = branchId, IsActive = true, PickupDate = pickupDate }, Pagination = null };
            ListOf<VehicleUpgradeModel> upgrades = _query.Process(query);
            return upgrades.Items;
        }

        private VehicleUpgradeModel getVehicleUpgrade(int vehicleUpgradeId) {
            VehicleUpgradeGetByIdQuery query = new VehicleUpgradeGetByIdQuery { Id = vehicleUpgradeId };
            return _query.Process(query);
        }

        private string getSetting(Setting s) {
            SettingGetByNameQuery query = new SettingGetByNameQuery { SettingName = s };
            SettingModel setting = _query.Process(query);
            return setting.Value;
        }

        private InvoiceModel getInvoiceForReservation(int reservationId) {
            InvoiceGetByReservationIdQuery query = new InvoiceGetByReservationIdQuery { ReservationId = reservationId };
            return _query.Process(query);
        }

        private List<WaiverModel> getWaivers(int vehicleGroupId) {
            WaiversGetQuery query = new WaiversGetQuery { Filter = new WaiverFilterModel { VehicleGroupId = vehicleGroupId } };
            return _query.Process(query);
        }

        private void applyCountdownSpecialToReservation(int reservationId) {

            if (Session["CountDownSpecialType"] != null) {
                CountdownSpecialType specialType = (CountdownSpecialType)(Convert.ToInt32(Session["CountDownSpecialType"]));
                DateTime expires = Convert.ToDateTime(Session["CountDownSpecialExpire"]);
                string textReward = "";
                int vehicleUpgradeId = 0;
                decimal amount = 0;
                int countdownId = Convert.ToInt32(Session["CountDownSpecialId"]);

                switch (specialType) {
                    case CountdownSpecialType.TextOnInvoice:
                        textReward = Session["CountDownSpecialReward"].ToString();
                        break;

                    case CountdownSpecialType.VehicleUpgrade:
                        vehicleUpgradeId = Convert.ToInt32(Session["CountDownSpecialReward"]);
                        amount = Convert.ToDecimal(Session["CountDownSpecialAmount"]);
                        break;
                }
                if (expires > DateTime.Now) {
                    ReservationSetCountdownSpecialCommand command = new ReservationSetCountdownSpecialCommand {
                        CountDownSpecialId = countdownId,
                        ReservationId = reservationId,
                        SpecialType = specialType,
                        TextReward = textReward,
                        VehicleUpgradeId = vehicleUpgradeId,
                        Amount = amount
                    };
                    _commandBus.Submit(command);
                }
            }

        }

        #endregion
    }
}