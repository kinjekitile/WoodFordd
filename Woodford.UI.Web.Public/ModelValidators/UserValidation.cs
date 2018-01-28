using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.ModelValidators {

    public static class UserUpdateValidationRuleSets {
        public const string Default = "default";
    }

    public class UserUpdateValidator : AbstractValidator<UserModel> {
        public UserUpdateValidator() {
            RuleSet(UserUpdateValidationRuleSets.Default, () => {
                RuleFor(x => x.FirstName)
                    .NotEmpty().WithMessage("First Name is required");
                RuleFor(x => x.LastName)
                    .NotEmpty().WithMessage("Last Name is required");
                RuleFor(x => x.MobileNumber)
                    .NotEmpty().WithMessage("Mobile Number is required");
            });
        }
    }

    public static class UserLoginValidationRuleSets {
        public const string Default = "default";
    }

    public class UserLoginValidator : AbstractValidator<UserLoginViewModel> {
        public UserLoginValidator() {
            RuleSet(UserLoginValidationRuleSets.Default, () => {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is required");
                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required");
            });
        }
    }

    public static class UserRegistrationValidationRuleSets {
        public const string Default = "default";
    }

    public class UserRegistrationValidator : AbstractValidator<UserRegistrationViewModel> {
        private IUserService _userService;

        public UserRegistrationValidator() {

            _userService = MvcApplication.Container.GetInstance<IUserService>();

            RuleSet(UserRegistrationValidationRuleSets.Default, () => {
                RuleFor(x => x.User.Email)
                    .NotEmpty().WithMessage("Email/Username is required");
                RuleFor(x => x.User.Email)
                    .EmailAddress().WithMessage("Must be a valid email address")
                    .Must(email => !UsernameExits(email))
                    .When(x => x.User.Email != null)
                    .WithMessage("Username already exists");
                RuleFor(x => x.UserPassword)
                    .NotEmpty().WithMessage("Password is required");
                RuleFor(x => x.ConfirmPassword)
                    .NotEmpty().WithMessage("Confirm password is required");
                RuleFor(x => x.UserPassword).Equal(x => x.ConfirmPassword).WithMessage("Passwords must match");
                RuleFor(x => x.User.FirstName)
                   .NotEmpty().WithMessage("First Name is required");
                RuleFor(x => x.User.LastName)
                    .NotEmpty().WithMessage("Last Name is required");
            });
        }

        private bool UsernameExits(string username) {
            return _userService.UserExists(username);
        }
    }

    public static class UserChangeEmailValidationRuleSets {
        public const string Default = "default";
    }

    public class UserChangeEmailValidator : AbstractValidator<ChangeEmailAddressViewModel> {
        private IUserService _userService;

        public UserChangeEmailValidator() {

            _userService = MvcApplication.Container.GetInstance<IUserService>();

            RuleSet(UserChangeEmailValidationRuleSets.Default, () => {
                RuleFor(x => x.NewEmail)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("Email/Username is required")
                    .NotEmpty().WithMessage("Email/Username is required")
                    .EmailAddress().WithMessage("Must be a valid email address");

                When(x => !string.IsNullOrEmpty(x.NewEmail), () => {
                    RuleFor(x => x.NewEmail)
                        .Must(email => !UsernameExits(email))
                        .WithMessage("Username already exists");
                });
            });
        }

        private bool UsernameExits(string username) {
            if (!string.IsNullOrEmpty(username))
                return _userService.UserExists(username);
            else
                return false;
        }
    }

    public static class UserChangePasswordValidationRuleSets {
        public const string Default = "default";
    }

    public class UserChangePasswordValidator : AbstractValidator<ChangePasswordViewModel> {

        public UserChangePasswordValidator() {
            RuleSet(UserChangePasswordValidationRuleSets.Default, () => {
                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is Required");
                RuleFor(x => x.NewPassword)
                    .NotEmpty().WithMessage("New Password is Required");

                RuleFor(x => x.ConfirmNewPassword)
                    .NotEmpty().WithMessage("Confirm New Password is Required");
                RuleFor(x => x.NewPassword).Equal(x => x.ConfirmNewPassword).WithMessage("Passwords must match");
            });
        }
    }

    public static class UserForgotPasswordValidationRuleSets {
        public const string Default = "default";
    }

    public class UserForgotPasswordValidator : AbstractValidator<ForgotPasswordViewModel> {

        public UserForgotPasswordValidator() {
            RuleSet(UserForgotPasswordValidationRuleSets.Default, () => {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email is Required")
                    .EmailAddress().WithMessage("Must be a valid email address");
            });
        }
    }

    public static class ReservationUserLoginOrRegistrationValidationRuleSets {
        public const string Default = "default";
    }

    public class ReservationUserLoginOrRegistrationValidator : AbstractValidator<ReservationUserLoginOrRegistrationViewModel> {
        private IUserService _userService;
        public ReservationUserLoginOrRegistrationValidator() {

            _userService = MvcApplication.Container.GetInstance<IUserService>();

            RuleSet(ReservationUserLoginOrRegistrationValidationRuleSets.Default, () => {
                When(x => x.IsLoginUser, () => {

                    RuleFor(x => x.LoginDetails.Email)
                        .NotEmpty().WithMessage("Email is Required")
                        .EmailAddress().WithMessage("Must be a valid email address");

                    RuleFor(x => x.LoginDetails.Password)
                        .NotEmpty().WithMessage("Password is required");
                });

                When(x => !x.IsLoginUser, () => {
                    RuleFor(x => x.User.FirstName)
                       .NotEmpty().WithMessage("First Name is Required");
                    RuleFor(x => x.User.LastName)
                       .NotEmpty().WithMessage("Last Name is Required");
                    RuleFor(x => x.User.Email)
                      .NotEmpty().WithMessage("Email is Required")
                      .EmailAddress().WithMessage("Must be a valid email address");
                    RuleFor(x => x.User.IdNumber)
                      .NotEmpty().WithMessage("Id Number is Required");


                    When(x => x.IsCreateAccount, () => {
                        RuleFor(x => x.User.Email)
                            .NotEmpty()
                            .Must(email => !UsernameExits(email))
                            .WithMessage("Username already exists");
                        RuleFor(x => x.Password)
                            .NotEmpty().WithMessage("Password is required");
                        RuleFor(x => x.ConfirmPassword)
                            .NotEmpty().WithMessage("Confirm password is required");
                        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords must match");
                    });

                });
            });
        }

        private bool UsernameExits(string username) {
            return _userService.UserExists(username);
        }
    }

    public static class ReservationGuestInfoValidationRuleSets {
        public const string Default = "default";
    }

    public class ReservationGuestInfoValidator : AbstractValidator<GuestInfoViewModel> {
        public ReservationGuestInfoValidator() {

            RuleSet(ReservationGuestInfoValidationRuleSets.Default, () => {
                RuleFor(x => x.User.FirstName)
                   .NotEmpty().WithMessage("First Name is Required");
                RuleFor(x => x.User.LastName)
                   .NotEmpty().WithMessage("Last Name is Required");
                RuleFor(x => x.User.Email)
                  .NotEmpty().WithMessage("Email is Required")
                    .EmailAddress().WithMessage("Must be a valid email address");
                RuleFor(x => x.User.IdNumber)
                  .NotEmpty().WithMessage("Id Number is Required");
                RuleFor(x => x.User.MobileNumber)
                  .NotEmpty().WithMessage("Mobile Number is Required");
            });
        }
    }

    public static class SubscribeValidationRuleSets {
        public const string Default = "default";
    }

    public class SubscribeValidator : AbstractValidator<SubscribeViewModel> {

        public SubscribeValidator() {

            RuleSet(SubscribeValidationRuleSets.Default, () => {
                RuleFor(x => x.SubscribeEmail)
                    .NotEmpty().WithMessage("Email is required")
                    .EmailAddress().WithMessage("Must be a valid email address");
            });
        }
    }
}
