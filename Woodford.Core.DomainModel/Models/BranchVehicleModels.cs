using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class BranchVehicleModel {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int VehicleId { get; set; }
        public VehicleModel Vehicle { get; set; }

        public IEnumerable<BranchVehicleExclusionModel> Exclusions { get; set; }
        public BranchVehicleModel() {
            Exclusions = new List<BranchVehicleExclusionModel>();
        }
    }

    public class BranchVehicleFilterModel {
        public int? BranchId { get; set; }
        public int? VehicleId { get; set; }
        public bool ShowPastExclusions { get; set; }
    }

    public class BranchVehicleExclusionModel {
        public int Id { get; set; }
        public int BranchVehicleId { get; set; }
        public BranchVehicleModel BranchVehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BranchVehicleExclusionFilterModel {
        public int? BranchVehicleId { get; set; }
        public int? BranchId { get; set; }
        public DateTime? SearchStart { get; set; }

        public DateTime? SearchEnd { get; set; }

    }
}
