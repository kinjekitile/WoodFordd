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
    public static class VehicleUpgradeValidationRuleSets {
        public const string Default = "default";
    }
    public class VehicleUpgradeValidator : AbstractValidator<VehicleUpgradeViewModel> {        
        public VehicleUpgradeValidator() {
            RuleSet(VehicleUpgradeValidationRuleSets.Default, () => {
                RuleFor(x => x.VehicleUpgrade.FromVehicleId)
                    .NotEmpty().WithMessage("From Vehicle is required");
                RuleFor(x => x.VehicleUpgrade.ToVehicleId)
                    .NotEmpty().WithMessage("To Vehicle is required");
                RuleFor(x => x.VehicleUpgrade.BranchId)
                    .NotEmpty().WithMessage("Branch is required");
                RuleFor(x => x.VehicleUpgrade.StartDate)
                    .NotEmpty().WithMessage("Start Date is required");

                RuleFor(x => x.VehicleUpgrade.EndDate)
                    .NotEmpty().WithMessage("End Date is required")
                    .GreaterThan(x => x.VehicleUpgrade.StartDate).WithMessage("End date must be after start date");
            });
        }       
    }    
}
