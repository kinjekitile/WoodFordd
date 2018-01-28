using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IVehicleUpgradeService {
        VehicleUpgradeModel Create(VehicleUpgradeModel model);
        VehicleUpgradeModel Update(VehicleUpgradeModel model);
        VehicleUpgradeModel MarkAs(int id, bool markAs);
        VehicleUpgradeModel GetById(int id);        
        ListOf<VehicleUpgradeModel> Get(VehicleUpgradeFilterModel filter, ListPaginationModel pagination);
    }
}
