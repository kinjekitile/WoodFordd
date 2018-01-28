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
    public static class VehicleAvailabilityExclusionValidationRuleSets {
        public const string Default = "default";
    }
    public class VehicleAvailabilityExclusionValidator : AbstractValidator<VehicleExlusionViewModel> {        
        public VehicleAvailabilityExclusionValidator() {

           
            RuleSet(VehicleAvailabilityExclusionValidationRuleSets.Default, () => {
                RuleFor(x => x.Exclusion.StartDate)
                    .NotEmpty().WithMessage("Start Date Required");
                RuleFor(x => x.Exclusion.EndDate)
                    .NotEmpty().WithMessage("End Date Required")
                    .GreaterThan(x => x.Exclusion.StartDate).WithMessage("End date must be after the start date");
            });
        }
        
    }

    public static class VehicleAvailabilityFilterValidationRuleSets {
        public const string Default = "default";
    }
    public class VehicleAvailabilityFilterValidator : AbstractValidator<VehicleAvailabilityFilterAndUpdateModel> {
        public VehicleAvailabilityFilterValidator() {


            RuleSet(VehicleAvailabilityFilterValidationRuleSets.Default, () => {
                RuleFor(x => x.Filter.BranchId)
                    .NotEmpty().WithMessage("Branch is Required");                

            });
        }

    }

    public static class VehicleAvailabilityExclusionsFilterValidationRuleSets {
        public const string Default = "default";
    }
    public class VehicleAvailabilityExclusionsFilterValidator : AbstractValidator<VehicleExclusionsViewModel> {
        public VehicleAvailabilityExclusionsFilterValidator() {


            RuleSet(VehicleAvailabilityExclusionsFilterValidationRuleSets.Default, () => {
                RuleFor(x => x.BranchId)
                    .NotEmpty().WithMessage("Branch is Required");
            });
        }

    }
}
