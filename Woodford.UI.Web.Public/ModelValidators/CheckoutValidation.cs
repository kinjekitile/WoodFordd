using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.ModelValidators {
    public static class CheckoutValidationRuleSets {
        public const string Default = "default";
        public const string Options = "options";
    }
    public class CheckoutPaymentValidator : AbstractValidator<CheckoutPaymentViewModel> {
        public CheckoutPaymentValidator() {
            RuleSet(CheckoutValidationRuleSets.Default, () => {
                RuleFor(x => x.CardNumber)
                    .NotEmpty().WithMessage("Card Number is required");
                RuleFor(x => x.CardHolderName)
                    .NotEmpty().WithMessage("Name is required");
                RuleFor(x => x.CVV)
                    .NotEmpty().WithMessage("CVV is required");
                RuleFor(x => x)
                .Must(x => IsValidExpiry(x))
                .WithMessage("Card Expired");

            });
        }

        private bool IsValidExpiry(CheckoutPaymentViewModel model) {
            DateTime check = new DateTime(model.ExpiryYear, model.ExpiryMonth, 1);
            DateTime valid = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (check < valid) {
                return false;
            }

            return true;
        }
    }
    
    

    //public class CheckoutOptionsValidator : AbstractValidator<CheckoutOptionsViewModel> {
    //    public CheckoutOptionsValidator() {
    //        RuleSet(CheckoutValidationRuleSets.Options, () => {
    //            RuleFor(x => x.Reservation.VoucherNumber)
    //                .NotEmpty().WithMessage("Card Number is required");

    //        });
    //    }
    //}
    
    //private bool    
}
