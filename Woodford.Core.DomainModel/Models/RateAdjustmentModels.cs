using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class RateAdjustmentModel {
        public int Id { get; set; }
        public RateAdjustmentType AdjustmentType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BranchId { get; set; }
        public BranchModel Branch { get; set; }
        public int? VehicleGroupId { get; set; }
        public VehicleGroupModel VehicleGroup { get; set; }
        public int? RateCodeId { get; set; }
        public RateCodeModel RateCode { get; set; }
        public int? NumberOfBookings { get; set; }
        public decimal AdjustmentPercentage { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsArchived { get; set; }
    }

    public class RateAdjustmentFilterModel {
        public RateAdjustmentType? AdjustmentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchId { get; set; }
        public int? VehicleGroupId { get; set; }
        public int? RateCodeId { get; set; }
        public bool? IsArchived { get; set; }

        public bool ShowPastAdjustments { get; set; }

    }
}
