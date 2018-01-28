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

    public static class RateAdjustmentValidatorRuleSets {
        public const string Default = "default";
        public const string ByVehicleGroup = "byvehiclegroup";
    }
    public class RateAdjustmentValidator : AbstractValidator<RateAdjustmentViewModel> {

        public RateAdjustmentValidator() {
            RuleSet(RateAdjustmentValidatorRuleSets.Default, () => {
                //RuleFor(x => x.Campaign.Title)
                //    .NotEmpty().WithMessage("Title is required");               
            });

            RuleSet(RateAdjustmentValidatorRuleSets.ByVehicleGroup, () => {
                //RuleFor(x => x.Campaign.Title)
                //    .NotEmpty().WithMessage("Title is required");               
            });
        }
    }
}