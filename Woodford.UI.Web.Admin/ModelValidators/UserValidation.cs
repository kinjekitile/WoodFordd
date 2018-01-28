using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.ModelValidators {

    public static class UserUpdateValidationRuleSets {
        public const string Default = "default";

    }

    public static class UserValidationRuleSets {
        public const string Search = "search";
        public const string Report = "report";
    }


    public class UserSearchValidator : AbstractValidator<UserSearchViewModel> {
        public UserSearchValidator() {

            RuleSet(UserValidationRuleSets.Report, () => {
                RuleFor(x => x.Report.Title)
                    .NotEmpty().WithMessage("Title is required");

                When(x => x.Report.UseCurrentDateAsStartDate, () => {

                    RuleFor(x => x.Report.DateUnitsToAdd)
                    .NotEmpty().WithMessage("Date Units is required");
                    RuleFor(x => x.Report.DateUnitType)
                    .NotEmpty().WithMessage("Unit Type is required");

                });
            });


            RuleSet(UserValidationRuleSets.Search, () => {
                When(x => x.Filter.DateFilterType != Core.DomainModel.Enums.UserDateFilterTypes.None, () => {
                    RuleFor(x => x.Filter.DateCreatedStart)
                    .NotEmpty().WithMessage("Required");
                    RuleFor(x => x.Filter.DateCreatedEnd)
                    .NotEmpty().WithMessage("Required");
                });

            });
        }
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

    //public static class UserLoginValidationRuleSets {
    //    public const string Default = "default";
    //}

    //public class UserLoginValidator : AbstractValidator<UserLoginViewModel> {
    //    public UserLoginValidator() {
    //        RuleSet(UserLoginValidationRuleSets.Default, () => {
    //            RuleFor(x => x.Email)
    //                .NotEmpty().WithMessage("Email is required");
    //            RuleFor(x => x.Password)
    //                .NotEmpty().WithMessage("Password is required");
    //        });
    //    }
    //}

    public static class UserRegistrationValidationRuleSets {
        public const string Default = "default";
        public const string ChangeEmail = "changeemail";
    }

    public class UserChangeEmailValidator : AbstractValidator<UserChangeEmailViewModel> {
        private IUserService _userService;

        public UserChangeEmailValidator() {
            _userService = MvcApplication.Container.GetInstance<IUserService>();

            RuleSet(UserRegistrationValidationRuleSets.ChangeEmail, () => {
                RuleFor(x => x.User.Email)
                    .NotEmpty().WithMessage("Email/Username is required");
                RuleFor(x => x.User.Email)
                    .EmailAddress().WithMessage("Must be a valid email address")
                    .Must(email => !UsernameExits(email))
                    .When(x => x.User.Email != null)
                    .WithMessage("Username already exists");
            });

        }

        private bool UsernameExits(string username) {
            return _userService.UserExists(username);
        }
    }

    public class UserRegistrationValidator : AbstractValidator<UserViewModel> {
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
                RuleFor(x => x.User.FirstName)
                   .NotEmpty().WithMessage("First Name is required");
                RuleFor(x => x.User.LastName)
                    .NotEmpty().WithMessage("Last Name is required");
                RuleFor(x => x.User.MobileNumber)
                    .NotEmpty().WithMessage("Mobile Number is required");
                //RuleFor(x => x.UserPassword)
                //    .NotEmpty().WithMessage("Password is required");
                //RuleFor(x => x.ConfirmPassword)
                //    .NotEmpty().WithMessage("Confirm password is required");
                //RuleFor(x => x.UserPassword).Equal(x => x.ConfirmPassword).WithMessage("Passwords must match");
            });
        }

        private bool UsernameExits(string username) {
            return _userService.UserExists(username);
        }
    }


}
