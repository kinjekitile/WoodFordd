using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    public class UserLoginViewModel {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserSetDisabledViewModel {
        public UserModel User { get; set; }
    }

    [Validator(typeof(UserChangeEmailValidator))]
    public class UserChangeEmailViewModel {
        public UserModel User { get; set; }
    }

    public class UserSetPasswordViewModel {
        public UserModel User { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }

    [Validator(typeof(UserRegistrationValidator))]
    public class UserViewModel {
        public UserModel User { get; set; }
        public int LoyaltyBookingsPerPeriod { get; set; }
        public decimal LoyaltyPointsEarnedPerPeriod { get; set; }
        public string LoyaltyTier { get; set; }
        public int BookingsRequiredForNextLoyaltyTier { get; set; }
        public string UserPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public LoyaltyOverviewModel LoyaltyOverview { get; set; }
        public string AdminPassword { get; set; }
    }

    [Validator(typeof(UserSearchValidator))]
    public class UserSearchViewModel {
        public UserFilterModel Filter { get; set; }

        public ListPaginationModel Pagination { get; set; }
        public ReportModel Report { get; set; }
        public List<UserModel> Items { get; set; }
    }

   
}
