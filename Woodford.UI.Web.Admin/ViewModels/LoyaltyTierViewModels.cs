using FluentValidation.Attributes;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;
namespace Woodford.UI.Web.Admin.ViewModels {

    public class LoyaltyTierViewModel {
        public LoyaltyTierModel Tier { get; set; }

    }



    [Validator(typeof(LoyaltyValidator))]
    public class LoyaltyTierBenefitViewModel {
        public LoyaltyTierBenefitModel Benefit { get; set; }
    }
}