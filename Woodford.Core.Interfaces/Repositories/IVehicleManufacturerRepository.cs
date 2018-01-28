using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces.Repositories {
    public interface IVehicleManufacturerRepository {
        VehicleManufacturerModel Create(VehicleManufacturerModel model);
        VehicleManufacturerModel Update(VehicleManufacturerModel model);
        VehicleManufacturerModel GetById(int id);
        List<VehicleManufacturerModel> Get(VehicleManufacturerFilterModel filter, ListPaginationModel pagination);
    }
}
