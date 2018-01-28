using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IVehicleRepository {
        VehicleModel Create(VehicleModel model);
        VehicleModel Update(VehicleModel model);
        VehicleModel GetById(int id);
        List<VehicleModel> GetByIds(List<int> ids);
        VehicleModel GetByUrl(string url);
        List<VehicleModel> Get(VehicleFilterModel filter, ListPaginationModel pagination);
        int GetCount(VehicleFilterModel filter);
        //void Dispose();
    }
}
