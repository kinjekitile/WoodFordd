using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.ModelValidators {
    public static class BookingHistoryValidationRuleSets {
        public const string Default = "default";
    }
    public class BookingHistoryValidator : AbstractValidator<AddBookingHistoryViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;
        public BookingHistoryValidator() {
            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(BookingHistoryValidationRuleSets.Default, () => {
                RuleFor(x => x.BookingHistory.PickupDate)
                 .NotEmpty().WithMessage("required");
                RuleFor(x => x.BookingHistory.DropOffDate)
                 .NotEmpty().WithMessage("required");
                RuleFor(x => x.BookingHistory.PickupBranchId)
                 .NotEmpty().WithMessage("required");
                RuleFor(x => x.BookingHistory.DropoffBranchId)
                 .NotEmpty().WithMessage("required");
                RuleFor(x => x.BookingHistory.Email)
                 .NotEmpty().WithMessage("required");
                RuleFor(x => x.BookingHistory.AlternateId)
                 .NotEmpty().WithMessage("required");
                RuleFor(x => x.BookingHistory.RentalDays)
                 .NotEmpty().WithMessage("required");
                RuleFor(x => x.BookingHistory.TotalForLoyaltyAward)
                 .NotEmpty().WithMessage("required");
                RuleFor(x => x.BookingHistory.TotalAmount)
                 .NotEmpty().WithMessage("required");
                RuleFor(x => x.BookingHistory.ExternalId)
                 .NotEmpty().WithMessage("required");

                //RuleFor(x => x.BookingHistory.RentalDays)
                //.NotNull().WithMessage("required");
                //RuleFor(x => x.BookingHistory.TotalForLoyaltyAward)
                // .NotNull().WithMessage("required");
                //RuleFor(x => x.BookingHistory.TotalAmount)
                // .NotNull().WithMessage("required");
                //RuleFor(x => x.BookingHistory.ExternalId)
                // .NotNull().WithMessage("required");
            });    
        }
    }
}