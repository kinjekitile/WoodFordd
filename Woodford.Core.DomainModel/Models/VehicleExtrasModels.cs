using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class VehicleExtrasModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string  ImageClass { get; set; }
        public LoyaltyTierBenefitModel LoyaltyBenefit { get; set; }

        public VehicleExtraOption OptionType { get; set; }
    }
}
