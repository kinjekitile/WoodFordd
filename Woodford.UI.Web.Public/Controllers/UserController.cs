using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Woodford.Core.ApplicationServices.Commands;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.Code.Helpers;
using Woodford.UI.Web.Public.ModelValidators;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Controllers {
    [Authorize]
    public class UserController : Controller {
        private ICommandBus _commandBus;
        private IQueryProcessor _query;

        public UserController(ICommandBus commandBus, IQueryProcessor query) {
            _commandBus = commandBus;
            _query = query;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([CustomizeValidator(RuleSet = UserLoginValidationRuleSets.Default)]  UserLoginViewModel model, string returnUrl) {
            if (ModelState.IsValid) {
                bool accountBlocked = false;
                //assign guest details to reservation
                var user = _query.Process(new UserGetByUsernameQuery { UserName = model.Email });
                if (user != null) {
                    if (user.IsAccountDisabled) {
                        accountBlocked = true;
                        ModelState.AddModelError("Email", "Account Blocked");

                    }
                }

                if (!accountBlocked) {
                    UserAuthenticateCommand command = new UserAuthenticateCommand { Username = model.Email, Password = model.Password };
                    _commandBus.Submit(command);
                    if (command.Success) {
                        return RedirectToLocal(returnUrl);
                    }
                } 
                
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register([CustomizeValidator(RuleSet = UserRegistrationValidationRuleSets.Default)] UserRegistrationViewModel model) {
            if (ModelState.IsValid) {
                //All new users are added to Advance by default
                model.User.IsLoyaltyMember = true;
                model.User.LoyaltyTierId = (int)LoyaltyTierLevel.Green;

                UserRegisterCommand command = new UserRegisterCommand { User = model.User, Password = model.UserPassword };
                _commandBus.Submit(command);
                if (command.Success) {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Profile() {
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);

            return View(currentUser);
        }

        [HttpPost]
        public ActionResult Profile([CustomizeValidator(RuleSet = UserUpdateValidationRuleSets.Default)] UserModel model) {
            if (ModelState.IsValid) {
                UserUpdateProfileCommand command = new UserUpdateProfileCommand { User = model };
                _commandBus.Submit(command);
                model = command.User;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult LogOff() {
            UserLogOffCommand command = new UserLogOffCommand();
            _commandBus.Submit(command);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangeEmailAddress() {
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);
            return View(new ChangeEmailAddressViewModel { User = currentUser });
        }

        [HttpPost]
        public ActionResult ChangeEmailAddress([CustomizeValidator(RuleSet = UserChangeEmailValidationRuleSets.Default)] ChangeEmailAddressViewModel model) {

            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);
            model.User = currentUser;

            if (ModelState.IsValid) {
                UserChangeUsernameCommand command = new UserChangeUsernameCommand { User = currentUser, NewUserName = model.NewEmail };
                _commandBus.Submit(command);
                UserLogOffCommand logOffCommand = new UserLogOffCommand();
                _commandBus.Submit(logOffCommand);
                return RedirectToAction("EmailAddressChanged");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult EmailAddressChanged() {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword() {
            return View(new ForgotPasswordViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword([CustomizeValidator(RuleSet = UserForgotPasswordValidationRuleSets.Default)] ForgotPasswordViewModel model) {

            if (ModelState.IsValid) {
                try {
                    NotifyUserPasswordResetTokenCommand command = new NotifyUserPasswordResetTokenCommand { Email = model.Email };
                    _commandBus.Submit(command);
                } catch (Exception) {
                    //if user can't be found, carry on as normal to prevent email guessing
                    //throw;
                }
                
                return View("ForgotPasswordSent", model);
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string token) {
            return View(new ResetPasswordViewModel { Token = token });
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model) {
            if (ModelState.IsValid) {                
                UserResetPasswordFromTokenCommand command = new UserResetPasswordFromTokenCommand { Password = model.Password, ResetToken = model.Token };
                _commandBus.Submit(command);
                return View("PasswordReset", model);
            }
            return View(model);
        }

        public ActionResult ChangePassword() {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model) {
            if (ModelState.IsValid) {                
                UserChangePasswordCommand command = new UserChangePasswordCommand { OldPassword = model.Password, NewPassword = model.NewPassword };
                _commandBus.Submit(command);
                return View("PasswordChanged", model);
            }
            return View(model);
        }

        public ActionResult BookingHistory() {
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);
            var bookingHistory = _query.Process(new BookingHistoryGetQuery { Filter = new BookingHistoryFilterModel { UserId = currentUser.Id } });
            return View(bookingHistory);
        }

        public ActionResult Reservations() {
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);
            var reservations = _query.Process(new ReservationsGetQuery { Filter = new ReservationFilterModel { UserId = currentUser.Id, IsCompletedInvoice = true } });

            return View(reservations);
        }

        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Advance() {
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);
            
            

            AdvanceViewModel viewModel = new AdvanceViewModel();
            viewModel.User = currentUser;
            var loyaltyTier = (LoyaltyTierLevel)currentUser.LoyaltyTierId;
           

            ViewBag.LoyaltyTier = loyaltyTier.GetDescription();
            switch (loyaltyTier) {
                case LoyaltyTierLevel.Green:
                    var greenTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Green });
                    viewModel.BookingsRequiredForCurrentLoyaltyTier = greenTier.BookingThresholdPerPeriod;
                    viewModel.BookingsRequiredForNextLoyaltyTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Silver }).BookingThresholdPerPeriod;
                    viewModel.LoyaltyTierPointsPercentage = greenTier.PointsEarnedPerRandSpent;
                    break;
                case LoyaltyTierLevel.Silver:
                    var silverTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Silver });
                    viewModel.BookingsRequiredForCurrentLoyaltyTier = silverTier.BookingThresholdPerPeriod;
                    viewModel.BookingsRequiredForNextLoyaltyTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Gold }).BookingThresholdPerPeriod;
                    viewModel.LoyaltyTierPointsPercentage = silverTier.PointsEarnedPerRandSpent;
                    break;

                case LoyaltyTierLevel.Gold:
                    var goldTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Gold });
                    viewModel.BookingsRequiredForCurrentLoyaltyTier = goldTier.BookingThresholdPerPeriod;
                    viewModel.BookingsRequiredForNextLoyaltyTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Gold }).BookingThresholdPerPeriod;
                    viewModel.LoyaltyTierPointsPercentage = goldTier.PointsEarnedPerRandSpent;
                    viewModel.IsFinalTier = true;
                    break;
            }


            LoyaltyOverviewModel loyaltyOverview = _query.Process(new LoyaltyOverviewByUserIdQuery { UserId = currentUser.Id });
            viewModel.LoyaltyOverview = loyaltyOverview;
            viewModel.LoyaltyBookingsPerPeriod = loyaltyOverview.BookingPerLoyaltyPeriod;

            
            return View(viewModel);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult AdvanceTest(int id) {
            UserGetByIdQuery query = new UserGetByIdQuery();
            query.Id = id;
            UserModel currentUser = _query.Process(query);



            AdvanceViewModel viewModel = new AdvanceViewModel();
            viewModel.User = currentUser;
            var loyaltyTier = (LoyaltyTierLevel)currentUser.LoyaltyTierId;


            ViewBag.LoyaltyTier = loyaltyTier.GetDescription();
            switch (loyaltyTier) {
                case LoyaltyTierLevel.Green:
                    var greenTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Green });
                    viewModel.BookingsRequiredForCurrentLoyaltyTier = greenTier.BookingThresholdPerPeriod;
                    viewModel.BookingsRequiredForNextLoyaltyTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Silver }).BookingThresholdPerPeriod;
                    viewModel.LoyaltyTierPointsPercentage = greenTier.PointsEarnedPerRandSpent;
                    break;
                case LoyaltyTierLevel.Silver:
                    var silverTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Silver });
                    viewModel.BookingsRequiredForCurrentLoyaltyTier = silverTier.BookingThresholdPerPeriod;
                    viewModel.BookingsRequiredForNextLoyaltyTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Gold }).BookingThresholdPerPeriod;
                    viewModel.LoyaltyTierPointsPercentage = silverTier.PointsEarnedPerRandSpent;
                    break;

                case LoyaltyTierLevel.Gold:
                    var goldTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Gold });
                    viewModel.BookingsRequiredForCurrentLoyaltyTier = goldTier.BookingThresholdPerPeriod;
                    viewModel.BookingsRequiredForNextLoyaltyTier = _query.Process(new LoyaltyTierGetByLevelQuery { Level = LoyaltyTierLevel.Gold }).BookingThresholdPerPeriod;
                    viewModel.LoyaltyTierPointsPercentage = goldTier.PointsEarnedPerRandSpent;
                    viewModel.IsFinalTier = true;
                    break;
            }


            LoyaltyOverviewModel loyaltyOverview = _query.Process(new LoyaltyOverviewByUserIdQuery { UserId = currentUser.Id });
            viewModel.LoyaltyOverview = loyaltyOverview;
            viewModel.LoyaltyBookingsPerPeriod = loyaltyOverview.BookingPerLoyaltyPeriod;


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Enroll() {

            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);

            UserEnrollLoyaltyCommand command = new UserEnrollLoyaltyCommand();
            command.Id = currentUser.Id;
            _commandBus.Submit(command);
            return RedirectToAction("Advance");
        }

        [HttpGet]
        public ActionResult ClaimBooking() {
            BookingClaimModel viewModel = new BookingClaimModel();
            viewModel.BookingPickupDate = DateTime.Today;
            viewModel.BookingDropoffDate = DateTime.Today;
            
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ClaimBooking(BookingClaimModel viewModel) {
            UserGetCurrentQuery query = new UserGetCurrentQuery();
            UserModel currentUser = _query.Process(query);
            viewModel.UserId = currentUser.Id;
            ClaimBookingAddCommand addBookingClaim = new ClaimBookingAddCommand();
            addBookingClaim.BookingClaim = viewModel;
            _commandBus.Submit(addBookingClaim);
            
            return RedirectToAction("ClaimSent");
        }

        public ActionResult ClaimSent() {
            return View();
        }
    }
}