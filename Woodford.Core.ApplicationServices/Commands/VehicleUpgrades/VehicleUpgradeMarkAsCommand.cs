using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VehicleUpgradeMarkAsCommand : ICommand {
        public int Id { get; set; }        
        public bool MarkAs { get; set; }
        public VehicleUpgradeModel ModelOut { get; set;}
    }

    public class VehicleUpgradeMarkAsCommandHandler : ICommandHandler<VehicleUpgradeMarkAsCommand> {
        private readonly IVehicleUpgradeService _vehicleUpgradeService;
        public VehicleUpgradeMarkAsCommandHandler(IVehicleUpgradeService vehicleUpgradeService) {
            _vehicleUpgradeService = vehicleUpgradeService;            
        }

        public void Handle(VehicleUpgradeMarkAsCommand command) {
            command.ModelOut = _vehicleUpgradeService.MarkAs(command.Id, command.MarkAs);
        }
    }
}
