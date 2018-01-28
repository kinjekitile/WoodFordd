using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VehicleEditCommand : ICommand {
        public VehicleModel Model { get; set; }
    }

    public class VehicleEditCommandHandler : ICommandHandler<VehicleEditCommand> {
        private readonly IVehicleService _vehicleService;
        public VehicleEditCommandHandler(IVehicleService vehicleService) {
            _vehicleService = vehicleService;
        }

        public void Handle(VehicleEditCommand command) {            
            _vehicleService.Update(command.Model);
        }
    }
}
