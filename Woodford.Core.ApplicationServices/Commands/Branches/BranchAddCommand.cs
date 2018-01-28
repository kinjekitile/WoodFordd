using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchAddCommand : ICommand {
        public BranchModel Model { get; set; }               
    }

    public class BranchAddCommandHandler : ICommandHandler<BranchAddCommand> {
        private readonly IBranchService _branchService;
        public BranchAddCommandHandler(IBranchService branchService) {
            _branchService = branchService;
        }

        public void Handle(BranchAddCommand command) {
            command.Model = _branchService.Create(command.Model);
        }
    }
}
