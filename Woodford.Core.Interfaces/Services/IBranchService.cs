using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBranchService {
        BranchModel Create(BranchModel model);
        BranchModel Update(BranchModel model);
        BranchModel GetById(int id, bool includePageContent = false);
        BranchModel GetByUrl(string url, bool includePageContent = false);
        ListOf<BranchModel> Get(BranchFilterModel filter, ListPaginationModel pagination);
        decimal GetTaxRateByBranchId(int id);
    }
}
