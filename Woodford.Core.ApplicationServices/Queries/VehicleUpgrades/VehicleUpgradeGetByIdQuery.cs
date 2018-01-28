using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class VehicleUpgradeGetByIdQuery : IQuery<VehicleUpgradeModel> {
        public int Id { get; set; }        
    }

    public class VehicleUpgradeGetByIdQueryHandler : IQueryHandler<VehicleUpgradeGetByIdQuery, VehicleUpgradeModel> {
        private readonly IVehicleUpgradeService _vehicleUpgradeService;
        public VehicleUpgradeGetByIdQueryHandler(IVehicleUpgradeService vehicleUpgradeService) {
            _vehicleUpgradeService = vehicleUpgradeService;
        }

        public VehicleUpgradeModel Process(VehicleUpgradeGetByIdQuery query) {
            return _vehicleUpgradeService.GetById(query.Id);
        }
    }
}
