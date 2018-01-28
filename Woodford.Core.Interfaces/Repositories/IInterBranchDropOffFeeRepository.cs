using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IInterBranchDropOffFeeRepository {
        InterBranchDropOffFeeModel Create(InterBranchDropOffFeeModel model);
        InterBranchDropOffFeeModel Update(InterBranchDropOffFeeModel model);
        InterBranchDropOffFeeModel GetById(int id);                
        List<InterBranchDropOffFeeModel> Get(InterBranchDropOffFeeFilterModel filter, ListPaginationModel pagination);
        int GetCount(InterBranchDropOffFeeFilterModel filter);
    }
}
