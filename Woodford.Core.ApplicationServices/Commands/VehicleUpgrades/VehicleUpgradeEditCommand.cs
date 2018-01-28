using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VehicleUpgradeEditCommand : ICommand {
        public VehicleUpgradeModel Model { get; set; }        
    }

    public class VehicleUpgradeEditCommandHandler : ICommandHandler<VehicleUpgradeEditCommand> {
        private readonly IVehicleUpgradeService _vehicleUpgradeService;
        public VehicleUpgradeEditCommandHandler(IVehicleUpgradeService vehicleUpgradeService) {
            _vehicleUpgradeService = vehicleUpgradeService;            
        }

        public void Handle(VehicleUpgradeEditCommand command) {
            command.Model = _vehicleUpgradeService.Update(command.Model);
        }
    }
}
