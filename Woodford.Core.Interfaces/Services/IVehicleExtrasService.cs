using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IVehicleExtrasService {
        List<VehicleExtrasModel> Get();
        VehicleExtrasModel GetById(int id);
        List<VehicleExtrasModel> GetByIds(List<int> ids);

        VehicleExtrasModel Edit(VehicleExtrasModel model);
    }
}
