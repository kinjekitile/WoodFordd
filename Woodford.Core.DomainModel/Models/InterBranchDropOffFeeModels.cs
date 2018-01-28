using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class InterBranchDropOffFeeModel {
        public int Id { get; set; }
        public int Branch1Id { get; set; }
        public BranchModel Branch1 { get; set; }
        public int Branch2Id { get; set; }
        public BranchModel Branch2 { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }

    public class InterBranchDropOffFeeFilterModel {
        public int? Branch1Id { get; set; }
        public int? Branch2Id { get; set; }                
        public bool? IsActive { get; set; }
    }
}
