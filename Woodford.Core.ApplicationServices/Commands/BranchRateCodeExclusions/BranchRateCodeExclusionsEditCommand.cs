using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchRateCodeExclusionsEditCommand : ICommand {
        public BranchRateCodeExclusionModel Model { get; set; }
    }

    public class BranchRateCodeExclusionsEditCommandHandler : ICommandHandler<BranchRateCodeExclusionsEditCommand> {

        private readonly IBranchRateCodeExclusionService _branchRateCodeExclusionService;

        public BranchRateCodeExclusionsEditCommandHandler(IBranchRateCodeExclusionService branchRateCodeExclusionService) {
            _branchRateCodeExclusionService = branchRateCodeExclusionService;
        }
        public void Handle(BranchRateCodeExclusionsEditCommand command) {
            _branchRateCodeExclusionService.Update(command.Model);
        }
    }
}
