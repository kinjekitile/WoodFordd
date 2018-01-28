using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class WaiverModel {
        public int Id { get; set; }
        public WaiverType WaiverType { get; set; }
        public VehicleGroupModel VehicleGroup { get; set; }
        public int VehicleGroupId { get; set; }
        public decimal? CostPerDay { get; set; }
        public decimal? Liability { get; set; }
    }

    public class WaiverFilterModel {
        public int? VehicleGroupId { get; set; }
    }
}
