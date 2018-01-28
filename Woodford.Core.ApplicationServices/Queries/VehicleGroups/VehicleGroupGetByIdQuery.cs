using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleGroupGetByIdQuery : IQuery<VehicleGroupModel> {
        public int Id { get; set; }
        public bool includePageContent { get; set; }
    }

    public class VehicleGroupGetByIdQueryHandler : IQueryHandler<VehicleGroupGetByIdQuery, VehicleGroupModel> {
        private readonly IVehicleGroupService _vehicleGroupService;
        public VehicleGroupGetByIdQueryHandler(IVehicleGroupService vehicleGroupService) {
            _vehicleGroupService = vehicleGroupService;
        }

        public VehicleGroupModel Process(VehicleGroupGetByIdQuery query) {
            return _vehicleGroupService.GetById(query.Id, query.includePageContent);
        }
    }
}
