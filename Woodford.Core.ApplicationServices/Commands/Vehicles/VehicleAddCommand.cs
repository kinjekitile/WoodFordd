using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VehicleAddCommand : ICommand {
        public VehicleModel Model { get; set; }        
    }

    public class VehicleAddCommandHandler : ICommandHandler<VehicleAddCommand> {
        private readonly IVehicleService _vehicleService;
        public VehicleAddCommandHandler(IVehicleService vehicleService) {
            _vehicleService = vehicleService;            
        }

        public void Handle(VehicleAddCommand command) {
            command.Model = _vehicleService.Create(command.Model);
        }
    }
}
