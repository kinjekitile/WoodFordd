using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {

    public class CountdownSpecialModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OfferText { get; set; }
        public decimal? OfferDiscount { get; set; }
        public CountdownSpecialType SpecialType { get; set; }
        public bool IsActive { get; set; }
        public int? VehicleUpgradeId { get; set; }
        public string UpgradeVehicleTitle { get; set; }
        public int? VehicleUpgradeFromId { get; set; }
        public decimal? VehicleUpgradeAmountOverride { get; set; }
    }

    public class CountdownSpecialFilterModel {
        public int? Id { get; set; }
        public bool? IsActive { get; set; }
        public CountdownSpecialType? SpecialType { get; set; }        
    }
}
