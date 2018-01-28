using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchVehicleExclusionsDeleteCommand : ICommand {
        public int Id { get; set; }
    }

    public class BranchVehicleExclusionsDeleteCommandHandler : ICommandHandler<BranchVehicleExclusionsDeleteCommand> {

        private readonly IBranchVehicleExclusionRepository _branchVehicleCodeExclusionService;

        public BranchVehicleExclusionsDeleteCommandHandler(IBranchVehicleExclusionRepository branchVehicleCodeExclusionService) {
            _branchVehicleCodeExclusionService = branchVehicleCodeExclusionService;
        }
        public void Handle(BranchVehicleExclusionsDeleteCommand command) {
            _branchVehicleCodeExclusionService.Delete(command.Id);
        }
    }
}
