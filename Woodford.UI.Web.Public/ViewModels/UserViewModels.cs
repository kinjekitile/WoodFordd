using FluentValidation.Attributes;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Public.ModelValidators;

namespace Woodford.UI.Web.Public.ViewModels {
    [Validator(typeof(UserLoginValidator))]
    public class UserLoginViewModel {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [Validator(typeof(UserRegistrationValidator))]
    public class UserRegistrationViewModel {
        public UserModel User { get; set; }
        public string UserPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    [Validator(typeof(UserChangeEmailValidator))]
    public class ChangeEmailAddressViewModel {
        public UserModel User { get; set; }        
        public string NewEmail { get; set; }
        
    }

    [Validator(typeof(UserForgotPasswordValidator))]    
    public class ForgotPasswordViewModel {
        public string Email { get; set; }

    }

    
    public class ResetPasswordViewModel {
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }        
    }

    [Validator(typeof(UserChangeEmailValidator))]
    public class ChangePasswordViewModel {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }        
    }    

    [Validator(typeof(ReservationUserLoginOrRegistrationValidator))]
    public class ReservationUserLoginOrRegistrationViewModel {
        public int ReservationId { get; set; }
        public ReservationModel Reservation { get; set; }

        public VehicleModel Vehicle { get; set; }
        public UserLoginViewModel LoginDetails { get; set; }

        public UserModel User { get; set; }
        public bool IsLoginUser { get; set; }
        public bool IsCreateAccount { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }

    [Validator(typeof(ReservationGuestInfoValidator))]
    public class GuestInfoViewModel {
        public int ReservationId { get; set; }
        public UserModel User { get; set; }        
    }

    [Validator(typeof(SubscribeValidator))]
    public class SubscribeViewModel {
        public bool Success { get; set; }
        public string SubscribeEmail { get; set; }
        public SubscribeViewModel() {
            Success = false;
        }
    }
}
