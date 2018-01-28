using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {

    public class VehicleUpgradeModel {
        public int Id { get; set; }
        public int FromVehicleId { get; set; }
        public int ToVehicleId { get; set; }
        public VehicleModel FromVehicle { get; set; }
        public VehicleModel ToVehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BranchId { get; set; }
        public BranchModel Branch { get; set; }
        public decimal UpgradeAmount { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public LoyaltyTierBenefitModel LoyaltyBenefit { get; set; }

    }

    public class VehicleUpgradeFilterModel {
        public int? Id { get; set; }
        public int? FromVehicleId { get; set; }
        public int? BranchId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? PickupDate { get; set; }
        
    }
}
