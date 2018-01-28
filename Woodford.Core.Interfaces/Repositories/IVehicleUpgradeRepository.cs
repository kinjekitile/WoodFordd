using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IVehicleUpgradeRepository {
        VehicleUpgradeModel Create(VehicleUpgradeModel model);
        VehicleUpgradeModel Update(VehicleUpgradeModel model);
        VehicleUpgradeModel GetById(int id);        
        List<VehicleUpgradeModel> Get(VehicleUpgradeFilterModel filter, ListPaginationModel pagination);
        int GetCount(VehicleUpgradeFilterModel filter);
    }
}
