using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.UI.Web.Public.ViewModels;
namespace Woodford.UI.Web.Public.ModelValidators {

    public static class ReservationValidationRuleSets {
        public const string ModifyLookup = "modifylookup";
    }
    public class ReservationValidator : AbstractValidator<ReservationViewModel> {
        public ReservationValidator() {
            RuleSet(ReservationValidationRuleSets.ModifyLookup, () => {
                RuleFor(x => x.Reservation.Id)
                .NotEmpty().WithMessage("Required");
                RuleFor(x => x.Reservation.QuoteReference)
                .NotEmpty().WithMessage("Required");
            });
        }
    }
}