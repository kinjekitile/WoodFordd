using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IVehicleGroupService {
        VehicleGroupModel Create(VehicleGroupModel model);
        VehicleGroupModel Update(VehicleGroupModel model);
        VehicleGroupModel GetById(int id, bool includePageContent);
        VehicleGroupModel GetByUrl(string url, bool includePageContent = false);
        ListOf<VehicleGroupModel> Get(VehicleGroupFilterModel filter, ListPaginationModel pagination);
        List<VehicleGroupModel> GetGroupsForVehicleIds(List<int> vehicleIds);
    }
}
