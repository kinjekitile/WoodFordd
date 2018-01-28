using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.ModelValidators {
    public static class SearchValidationRuleSets {
        public const string Default = "default";
        public const string MonthlySearch = "monthly";
    }
    public class SearchValidator : AbstractValidator<SearchCriteriaViewModel> {

        private readonly ISettingService _settings;
        private readonly IBranchRepository _branchRepo;
        int minLeadTime = 1;
        public SearchValidator() {

            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            minLeadTime = Convert.ToInt32(_settings.Get(Core.DomainModel.Models.Setting.Booking_Lead_Time_Hours).Value);
            _branchRepo = MvcApplication.Container.GetInstance<IBranchRepository>();

            RuleSet(SearchValidationRuleSets.Default, () => {

                RuleFor(x => x.PickupDate).NotNull().WithMessage("Pickup date is required");
                RuleFor(x => x.DropOffDate).NotNull().WithMessage("Dropoff date is required");
                //RuleFor(x => x.PickupDate).Must((x, PickupDate) => x.PickupDate.AddHours(x.Criteria.PickupTime) >= DateTime.Now.AddHours(minLeadTime)).WithMessage("To make a booking from the website, we require at least " + minLeadTime + " hours lead time. Please contact us directly to make this booking.");
                //When(x => x.PickupDate >= DateTime.Today, () => {
                //    RuleFor(x => x.PickupDate).Must((x, PickupDate) => x.PickupDate.AddHours(x.Criteria.PickupTime) >= DateTime.Now.AddHours(minLeadTime)).WithMessage("To make a booking from the website, we require at least " + minLeadTime + " hours lead time. Please contact us directly to make this booking.");
                //});
                When(x => x.PickupDate >= DateTime.Today, () => {
                    RuleFor(x => x.PickupDate)
                    .Must((x, PickupDate) => IsWithinLeadTime(x, PickupDate, ref minLeadTime))
                    .WithMessage("{0}", LeadTimeMessage);
                    //.WithMessage("To make a booking from the website, we require at least {0} hours lead time. Please contact us directly to make this booking.", minLeadTime);
                    //.WithMessage("To make a booking from the website, we require at least " + minLeadTime + " hours lead time. Please contact us directly to make this booking.");
                });


                RuleFor(x => x.DropOffDate).Must((x, DropOffDate) => x.PickupDate.AddHours(x.Criteria.PickUpTimeFullInt) <= DropOffDate.AddHours(x.Criteria.DropOffTimeFullInt)).WithMessage("Please pick a drop off date after your pick up date.");

                RuleFor(x => x.PickupDate)
                    .NotEmpty().WithMessage("Pickup Date is required");

                RuleFor(x => x.DropOffDate)
                    .NotEmpty().WithMessage("Drop off Date is required")
                    .GreaterThan(x => x.Criteria.PickupDateTime).WithMessage("Drop off date must be after pickup date");

                RuleFor(x => x.PickupDate)
                .Must(pickup => pickup.Date >= DateTime.Today.Date)
                .WithMessage("Pickup date cannot be in the past");
            });

            RuleSet(SearchValidationRuleSets.MonthlySearch, () => {

                RuleFor(x => x.PickupDate).NotNull().WithMessage("Pickup date is required");
                RuleFor(x => x.DropOffDate).NotNull().WithMessage("Dropoff date is required");
                //RuleFor(x => x.PickupDate).Must((x, PickupDate) => x.PickupDate.AddHours(x.Criteria.PickupTime) >= DateTime.Now.AddHours(minLeadTime)).WithMessage("To make a booking from the website, we require at least " + minLeadTime + " hours lead time. Please contact us directly to make this booking.");
                When(x => x.PickupDate >= DateTime.Today, () => {
                    RuleFor(x => x.PickupDate)
                    .Must((x, PickupDate) => IsWithinLeadTime(x, PickupDate, ref minLeadTime))
                    .WithMessage("To make a booking from the website, we require at least " + minLeadTime + " hours lead time. Please contact us directly to make this booking.");
                });

                When(x => (x.DropOffDate.Date - x.PickupDate.Date).Days < 27, () => {
                    RuleFor(x => x.PickupDate).Must((x, PickupDate) => x.PickupDate.AddHours(x.Criteria.PickupTime) >= DateTime.Now.AddHours(minLeadTime)).WithMessage("Monthly bookings require a minimum of 27 days.");
                });

                RuleFor(x => x.DropOffDate).Must((x, DropOffDate) => x.PickupDate.AddHours(x.Criteria.PickupTime) <= DropOffDate.AddHours(x.Criteria.DropOffTime)).WithMessage("Please pick a drop off date after your pick up date.");

                RuleFor(x => x.PickupDate)
                    .NotEmpty().WithMessage("Pickup Date is required");

                RuleFor(x => x.DropOffDate)
                    .NotEmpty().WithMessage("Drop off Date is required")
                    .GreaterThan(x => x.Criteria.PickupDateTime).WithMessage("Drop off date must be after pickup date");

                RuleFor(x => x.PickupDate)
                .Must(pickup => pickup.Date >= DateTime.Today.Date)
                .WithMessage("Pickup date cannot be in the past");
            });
        }

        private string LeadTimeMessage(SearchCriteriaViewModel criteria) {
            string message = "To make a booking from the website, we require at least {0} hours lead time. Please contact us directly to make this booking.";
            int leadTime = minLeadTime;
            var b = _branchRepo.GetById(criteria.Criteria.PickUpLocationId);
            if (DateTime.Now.Hour > 17 || DateTime.Now.Hour < 8) {
                //After hours
                if (b.BookingLeadTimeNight.HasValue) {
                    leadTime = b.BookingLeadTimeNight.Value;
                }
            }
            else {
                if (b.BookingLeadTimeDay.HasValue) {
                    leadTime = b.BookingLeadTimeDay.Value;
                }
            }

            message = string.Format(message, leadTime);
            return message;
        }

        private bool IsWithinLeadTime(SearchCriteriaViewModel criteria, DateTime pickupDate, ref int minLeadTime) {
            //minLeadTime = Convert.ToInt32(_settings.Get(Core.DomainModel.Models.Setting.Booking_Lead_Time_Hours).Value);
            var b = _branchRepo.GetById(criteria.Criteria.PickUpLocationId);
            if (DateTime.Now.Hour > 17 || DateTime.Now.Hour < 8) {
                //After hours
                if (b.BookingLeadTimeNight.HasValue) {
                    minLeadTime = b.BookingLeadTimeNight.Value;
                }
            } else {
                if (b.BookingLeadTimeDay.HasValue) {
                    minLeadTime = b.BookingLeadTimeDay.Value;
                }
            }

            if (criteria.PickupDate.AddHours(criteria.Criteria.PickUpTimeFullInt) >= DateTime.Now.AddHours(minLeadTime)) {
                return true;
            } else {
                return false;
            }
        }
    }
}
