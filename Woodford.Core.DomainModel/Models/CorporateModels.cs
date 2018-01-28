using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class CorporateModel {
        public int Id { get; set; }
        public string Title { get; set; }

        public IEnumerable<RateCodeModel> RateCodes { get; set; }
        public CorporateModel() {
            RateCodes = new List<RateCodeModel>();
        }
    }

    public class CorporateFilterModel {
        public string Title { get; set; }
    }

    
}
