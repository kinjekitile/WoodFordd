using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class BranchRateCodeExclusionModel {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int RateCodeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RateCodeModel RateCode { get; set; }
        public BranchModel Branch { get; set; }
    }

    public class BranchRateCodeExclusionFilterModel {
        public int? Id { get; set; }
        public int? BranchId { get; set; }
        public int? RateCodeId { get; set; }
        public List<int> RateCodeIds { get; set; }
        public DateTime? SearchStart { get; set; }
        public DateTime? SearchEnd { get; set; }
        public bool ShowPastExclusions { get; set; }
    }
}
