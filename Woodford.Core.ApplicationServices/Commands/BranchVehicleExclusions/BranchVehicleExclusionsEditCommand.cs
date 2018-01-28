using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchVehicleExclusionsEditCommand : ICommand {
        public BranchVehicleExclusionModel Model { get; set; }
    }

    public class BranchVehicleExclusionsEditCommandHandler : ICommandHandler<BranchVehicleExclusionsEditCommand> {

        private readonly IBranchVehicleExclusionService _branchVehicleExclusionService;

        public BranchVehicleExclusionsEditCommandHandler(IBranchVehicleExclusionService branchVehicleExclusionService) {
            _branchVehicleExclusionService = branchVehicleExclusionService;
        }
        public void Handle(BranchVehicleExclusionsEditCommand command) {
            _branchVehicleExclusionService.Update(command.Model);
        }
    }
}
