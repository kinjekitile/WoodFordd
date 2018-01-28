using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IVehicleService {
        VehicleModel Create(VehicleModel model);
        VehicleModel Update(VehicleModel model);
        VehicleModel GetById(int id, bool includePageContent);
        List<VehicleModel> GetByIds(List<int> ids, bool includePageContent);
        VehicleModel GetByUrl(string url, bool includePageContent = false);
        ListOf<VehicleModel> Get(VehicleFilterModel filter, ListPaginationModel pagination);
    }
}
