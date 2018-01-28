using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchRateCodeExclusionsAddCommand : ICommand {

        public BranchRateCodeExclusionModel Model { get; set; }

    }

    public class BranchRateCodeExclusionsAddCommandHandler : ICommandHandler<BranchRateCodeExclusionsAddCommand> {

        private readonly IBranchRateCodeExclusionService _branchRateCodeExclusionService;

        public BranchRateCodeExclusionsAddCommandHandler(IBranchRateCodeExclusionService branchRateCodeExclusionService) {
            _branchRateCodeExclusionService = branchRateCodeExclusionService;
        }
        public void Handle(BranchRateCodeExclusionsAddCommand command) {

            _branchRateCodeExclusionService.Create(command.Model);

        }
    }
}
