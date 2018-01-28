using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBranchVehicleRepository {
        BranchVehicleModel Create(BranchVehicleModel model);
        BranchVehicleModel Update(BranchVehicleModel model);
        void Delete(int id);
        BranchVehicleModel GetById(int id);
        List<BranchVehicleModel> Get(BranchVehicleFilterModel filter, ListPaginationModel pagination);
        int GetCount(BranchVehicleFilterModel filter);
        List<int> GetAvailableVehicleIds(int branchId, DateTime pickupDate);
    }
}
