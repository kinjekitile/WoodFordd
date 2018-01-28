using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBranchRepository {
        BranchModel Create(BranchModel model);
        BranchModel Update(BranchModel model);
        BranchModel GetById(int id);
        BranchModel GetByUrl(string url);
        List<BranchModel> Get(BranchFilterModel filter, ListPaginationModel pagination);
        int GetCount(BranchFilterModel filter);
    }
}
