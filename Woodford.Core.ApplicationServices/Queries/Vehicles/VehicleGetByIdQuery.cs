using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleGetByIdQuery : IQuery<VehicleModel> {
        public int Id { get; set; }
        public bool includePageContent { get; set; }
    }

    public class VehicleGetByIdQueryHandler : IQueryHandler<VehicleGetByIdQuery, VehicleModel> {
        private readonly IVehicleService _vehicleService;
        public VehicleGetByIdQueryHandler(IVehicleService vehicleService) {
            _vehicleService = vehicleService;
        }

        public VehicleModel Process(VehicleGetByIdQuery query) {
            return _vehicleService.GetById(query.Id, query.includePageContent);
        }
    }
}
