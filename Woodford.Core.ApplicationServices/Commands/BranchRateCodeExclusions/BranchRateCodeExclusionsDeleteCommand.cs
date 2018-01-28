using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchRateCodeExclusionsDeleteCommand : ICommand {
        public int Id { get; set; }
    }

    public class BranchRateCodeExclusionsDeleteCommandHandler : ICommandHandler<BranchRateCodeExclusionsDeleteCommand> {

        private readonly IBranchRateCodeExclusionService _branchRateCodeExclusionService;

        public BranchRateCodeExclusionsDeleteCommandHandler(IBranchRateCodeExclusionService branchRateCodeExclusionService) {
            _branchRateCodeExclusionService = branchRateCodeExclusionService;
        }
        public void Handle(BranchRateCodeExclusionsDeleteCommand command) {
            _branchRateCodeExclusionService.Delete(command.Id);
        }
    }
}
