using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IInterBranchDropOffFeeService {
        InterBranchDropOffFeeModel Create(InterBranchDropOffFeeModel model);
        InterBranchDropOffFeeModel Update(InterBranchDropOffFeeModel model);
        InterBranchDropOffFeeModel GetById(int id);        
        ListOf<InterBranchDropOffFeeModel> Get(InterBranchDropOffFeeFilterModel filter, ListPaginationModel pagination);
        InterBranchDropOffFeeModel MarkAs(int id, bool markAs);
    }
}
