using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VehicleUpgradeAddCommand : ICommand {
        public VehicleUpgradeModel Model { get; set; }        
    }

    public class VehicleUpgradeAddCommandHandler : ICommandHandler<VehicleUpgradeAddCommand> {
        private readonly IVehicleUpgradeService _vehicleUpgradeService;
        public VehicleUpgradeAddCommandHandler(IVehicleUpgradeService vehicleUpgradeService) {
            _vehicleUpgradeService = vehicleUpgradeService;            
        }

        public void Handle(VehicleUpgradeAddCommand command) {
            command.Model = _vehicleUpgradeService.Create(command.Model);
        }
    }
}
