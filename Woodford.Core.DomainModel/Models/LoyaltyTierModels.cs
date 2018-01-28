using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class LoyaltyTierModel {
        public LoyaltyTierLevel TierLevel { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BookingThresholdPerPeriod { get; set; }
        public int RandSpent { get; set; }
        public decimal PointsEarnedPerRandSpent { get; set; }
        public List<LoyaltyTierBenefitModel> Benefits { get; set; }
        public LoyaltyTierModel() {
            Benefits = new List<LoyaltyTierBenefitModel>();
        }

    }



    public class LoyaltyTierBenefitModel {
        public int Id { get; set; }
        public LoyaltyTierLevel Tier { get; set; }
        public BenefitType BenefitType { get; set; }
        
        
        public string Title { get; set; }
        public string Description { get; set; }
        //Type 1
        public int? DropOffGraceHours { get; set; }

        //Type 2
        public int? PickupLocationId { get; set; }
        public int? DropOffLocationId { get; set; }

        //Type 3
        public int? FreeKms { get; set; }

        //Type 4
        public int? FreeDays { get; set; }

        //Type 5
        public int? UpgradeId { get; set; }

        //Type 6
        //TODO extras
        public int? ExtraId { get; set; }
        public decimal? ExtraPriceOverride { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //Type 7
        //no fields needed
    }
}
