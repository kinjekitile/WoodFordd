using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.ModelValidators {
    public static class LoyaltyValidatorRuleSets {
        public const string Default = "default";
    }
    public class LoyaltyValidator : AbstractValidator<LoyaltyTierBenefitViewModel> {

        public LoyaltyValidator() {
            RuleSet(LoyaltyValidatorRuleSets.Default, () => {
                RuleFor(x => x.Benefit.Tier)
                .NotEmpty().WithMessage("Tier is required");
                RuleFor(x => x.Benefit.BenefitType)
                .NotEmpty().WithMessage("Type is required");
                RuleFor(x => x.Benefit.Title)
                .NotEmpty().WithMessage("Title is required");
                RuleFor(x => x.Benefit.StartDate)
                .NotEmpty().WithMessage("Start Date required");
                RuleFor(x => x.Benefit.EndDate)
                .NotEmpty().WithMessage("End Date required");


                When(x => x.Benefit.BenefitType == Core.DomainModel.Enums.BenefitType.DropOffGraceTime, () => {
                    RuleFor(x => x.Benefit.DropOffGraceHours)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull().WithMessage("Drop off grace time is required")
                    .NotEmpty().WithMessage("Drop off grace time is required")
                    .GreaterThan(0).WithMessage("Drop off grace time must be greater than 0");
                });

                When(x => x.Benefit.BenefitType == Core.DomainModel.Enums.BenefitType.FreeOneWayDrops, () => {
                    RuleFor(x => x.Benefit.PickupLocationId)
                    .NotEmpty().WithMessage("Pick up location required")
                    .GreaterThan(0).WithMessage("Pick up location required")
                    .NotEqual(x => x.Benefit.DropOffLocationId).WithMessage("Pickup cannot be the same as drop off");
                    RuleFor(x => x.Benefit.DropOffLocationId)
                    .NotEmpty().WithMessage("Drop off location is required")
                    .GreaterThan(0).WithMessage("Drop off location required")
                    .NotEqual(x => x.Benefit.PickupLocationId).WithMessage("Drop off cannot be the same as drop off");
                });



                When(x => x.Benefit.BenefitType == Core.DomainModel.Enums.BenefitType.ExtraKms, () => {
                    RuleFor(x => x.Benefit.FreeKms)
                    .NotEmpty().WithMessage("Free kms is required")
                    .GreaterThan(0).WithMessage("Free kms must be greater than 0");

                });

                When(x => x.Benefit.BenefitType == Core.DomainModel.Enums.BenefitType.FreeDays, () => {
                    RuleFor(x => x.Benefit.FreeDays)
                    .NotEmpty().WithMessage("Free days is required")
                    .GreaterThan(0).WithMessage("Free days must be greater than 0");
                });

                When(x => x.Benefit.BenefitType == Core.DomainModel.Enums.BenefitType.Upgrades, () => {
                    RuleFor(x => x.Benefit.UpgradeId)
                    .NotEmpty().WithMessage("Upgrade is required")
                    .GreaterThan(0).WithMessage("Upgrade is required");
                });
            });
        }
    }
}