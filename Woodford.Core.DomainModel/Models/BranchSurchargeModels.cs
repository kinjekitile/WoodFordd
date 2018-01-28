using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class BranchSurchargeModel {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Title { get; set; }
        public decimal SurchargeAmount { get; set; }
        public bool IsOnceOff { get; set; }
        public decimal? MaximumCharge { get; set; }

    }

    public class BranchSurchargeFilterModel {
        public int? BranchId { get; set; }
    }
}
