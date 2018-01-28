using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleGroupGetByUrlQuery : IQuery<VehicleGroupModel> {
        public string Url { get; set; }
        public bool IncludePageContent { get; set; }
    }

    public class VehicleGroupGetByUrlQueryHandler : IQueryHandler<VehicleGroupGetByUrlQuery, VehicleGroupModel> {
        private readonly IVehicleGroupService _vehicleGroupService;
        public VehicleGroupGetByUrlQueryHandler(IVehicleGroupService vehicleGroupService) {
            _vehicleGroupService = vehicleGroupService;
        }

        public VehicleGroupModel Process(VehicleGroupGetByUrlQuery query) {
            return _vehicleGroupService.GetByUrl(query.Url, query.IncludePageContent);
        }
    }
}
