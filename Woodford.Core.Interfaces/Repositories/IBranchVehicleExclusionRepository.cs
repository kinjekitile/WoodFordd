using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBranchVehicleExclusionRepository {
        BranchVehicleExclusionModel Create(BranchVehicleExclusionModel model);
        BranchVehicleExclusionModel Update(BranchVehicleExclusionModel model);
        void Delete(int id);
        BranchVehicleExclusionModel GetById(int id);
        List<BranchVehicleExclusionModel> Get(BranchVehicleExclusionFilterModel filter, ListPaginationModel pagination);
        int GetCount(BranchVehicleExclusionFilterModel filter);
    }
}
