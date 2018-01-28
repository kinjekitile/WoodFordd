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
    public static class RateRuleValidationRuleSets {
        public const string Default = "default";
    }
    public class RateRuleValidator : AbstractValidator<RateRuleViewModel> {
        public RateRuleValidator() {
            RuleSet(VehicleGroupValidationRuleSets.Default, () => {
                RuleFor(x => x.Rule.MinDays)
                    .GreaterThan(0).WithMessage("Minimum Days is required");
                RuleFor(x => x.Rule.Title)
                .NotEmpty().WithMessage("Rule name required");
            });
        }
    }

    public static class RateCodeValidationRuleSets {
        public const string Default = "default";
    }
    public class RateCodeValidator : AbstractValidator<RateCodeViewModel> {
        public RateCodeValidator() {
            RuleSet(RateCodeValidationRuleSets.Default, () => {
                RuleFor(x => x.Code.Title)
                    .NotEmpty().WithMessage("Title is required");
            });
        }
    }

    public static class RateUpdateFilterValidationRuleSets {
        public const string Default = "default";
    }
    public class RateUpdateFilterValidator : AbstractValidator<RateSearchAndUpdateViewModel> {
        public RateUpdateFilterValidator() {

            RuleSet(RateCodeValidationRuleSets.Default, () => {
                RuleFor(x => x.Search.RateCodeId)
                 .NotEmpty().WithMessage("Rate Code required");
                RuleFor(x => x.Search.RateCodeId)
                .GreaterThan(0).WithMessage("Rate Code required");
                //RuleFor(x => x.SelectedBranchIds)
                //.NotNull().WithMessage("At least one branch must be selected");
                RuleFor(x => x.Search.StartDate)
                .NotNull().WithMessage("Start Date required")
                .When(x => !x.Search.IsOpenEnded);
                RuleFor(x => x.Search.EndDate)
                .NotNull().WithMessage("End Date required")
                .When(x => !x.Search.IsOpenEnded);
            });
        }
    }
}
