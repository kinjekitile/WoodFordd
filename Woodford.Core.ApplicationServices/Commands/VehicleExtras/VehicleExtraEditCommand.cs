using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VehicleExtraEditCommand : ICommand {
        public VehicleExtrasModel Model { get; set; }
    }

    public class VehicleExtraEditCommandHandler : ICommandHandler<VehicleExtraEditCommand> {
        private readonly IVehicleExtrasService _extraService;
        public VehicleExtraEditCommandHandler(IVehicleExtrasService extraService) {
            _extraService = extraService;
        }
        public void Handle(VehicleExtraEditCommand command) {
            _extraService.Edit(command.Model);
        }
    }
}
