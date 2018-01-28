using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchVehicleExclusionsAddCommand : ICommand {

        public BranchVehicleExclusionModel Model { get; set; }

    }

    public class BranchVehicleExclusionsAddCommandHandler : ICommandHandler<BranchVehicleExclusionsAddCommand> {

        private readonly IBranchVehicleExclusionService _branchVehicleExclusionService;

        public BranchVehicleExclusionsAddCommandHandler(IBranchVehicleExclusionService branchVehicleExclusionService) {
            _branchVehicleExclusionService = branchVehicleExclusionService;
        }
        public void Handle(BranchVehicleExclusionsAddCommand command) {

            _branchVehicleExclusionService.Create(command.Model);

        }
    }
}
