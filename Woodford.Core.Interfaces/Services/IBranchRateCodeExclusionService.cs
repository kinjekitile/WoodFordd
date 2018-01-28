using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBranchRateCodeExclusionService {
        BranchRateCodeExclusionModel Create(BranchRateCodeExclusionModel model);
        BranchRateCodeExclusionModel Update(BranchRateCodeExclusionModel model);
        void Delete(int id);
        BranchRateCodeExclusionModel GetById(int id);
        ListOf<BranchRateCodeExclusionModel> Get(BranchRateCodeExclusionFilterModel filter, ListPaginationModel pagination);
    }
}
