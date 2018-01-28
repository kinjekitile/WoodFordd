using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.ModelValidators {
    public static class CountdownSpecialValidationRuleSets {
        public const string Default = "default";
    }
    public class CountdownSpecialValidator : AbstractValidator<CountdownSpecialViewModel> {        
        public CountdownSpecialValidator() {            
            RuleSet(VoucherValidationRuleSets.Default, () => {

                RuleFor(x => x.CountdownSpecial.Title)
                    .NotEmpty().WithMessage("Title is required");

                RuleFor(x => x.CountdownSpecial.SpecialType)
                    .NotEmpty().WithMessage("Special Type is required");

                //RuleFor(x => x.CountdownSpecial.OfferDiscount)
                //    .NotEmpty().WithMessage("Discount Amount value is required")
                //    .GreaterThan(0).WithMessage("Discount Amount value is required")
                //    .When(x => x.CountdownSpecial.SpecialType == CountdownSpecialType.DiscountAmount);

                RuleFor(x => x.CountdownSpecial.OfferText)
                    .NotEmpty().WithMessage("Offer Text value is required")                    
                    .When(x => x.CountdownSpecial.SpecialType == CountdownSpecialType.TextOnInvoice);

                When(x => x.CountdownSpecial.SpecialType == CountdownSpecialType.VehicleUpgrade, () => {

                    RuleFor(x => x.CountdownSpecial.VehicleUpgradeId)
                        .GreaterThan(0).WithMessage("Vehicle Upgrade is required");

                    RuleFor(x => x.CountdownSpecial.VehicleUpgradeAmountOverride)
                        .NotEmpty().WithMessage("Vehicle Upgrade Override Amount is required");
                });                
            });
        }
        
    }    
}
