using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IBranchVehicleService {
        BranchVehicleModel Create(BranchVehicleModel model);
        BranchVehicleModel Update(BranchVehicleModel model);
        void Delete(int id);
        BranchVehicleModel GetById(int id);
        ListOf<BranchVehicleModel> Get(BranchVehicleFilterModel filter, ListPaginationModel pagination);
        List<VehicleModel> GetAvailableVehicles(int branchId, DateTime pickupDate);
    }
}
