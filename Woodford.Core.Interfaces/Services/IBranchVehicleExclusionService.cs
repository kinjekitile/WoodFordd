using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBranchVehicleExclusionService {
        BranchVehicleExclusionModel Create(BranchVehicleExclusionModel model);
        BranchVehicleExclusionModel Update(BranchVehicleExclusionModel model);
        void Delete(int id);
        BranchVehicleExclusionModel GetById(int id);
        ListOf<BranchVehicleExclusionModel> Get(BranchVehicleExclusionFilterModel filter, ListPaginationModel pagination);
    }
}
