using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBranchSurchargeService {
        BranchSurchargeModel Create(BranchSurchargeModel model);
        BranchSurchargeModel Update(BranchSurchargeModel model);
        BranchSurchargeModel GetById(int id);
        ListOf<BranchSurchargeModel> Get(BranchSurchargeFilterModel filter, ListPaginationModel pagination);
    }
}
