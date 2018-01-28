using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IVehicleGroupRepository {
        VehicleGroupModel Create(VehicleGroupModel model);
        VehicleGroupModel Update(VehicleGroupModel model);
        VehicleGroupModel GetById(int id);
        VehicleGroupModel GetByUrl(string url);
        List<VehicleGroupModel> Get(VehicleGroupFilterModel filter, ListPaginationModel pagination);
        int GetCount(VehicleGroupFilterModel filter);
        List<VehicleGroupModel> GetGroupsForVehicleIds(List<int> vehicleIds);
        //void Dispose();
    }
}
