using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class InterBranchDropOffFeeEditCommand : ICommand {
        public InterBranchDropOffFeeModel Model { get; set; }               
    }

    public class InterBranchDropOffFeeEditCommandHandler : ICommandHandler<InterBranchDropOffFeeEditCommand> {
        private readonly IInterBranchDropOffFeeService _interBranchDropOffFeeService;
        public InterBranchDropOffFeeEditCommandHandler(IInterBranchDropOffFeeService interBranchDropOffFeeService) {
            _interBranchDropOffFeeService = interBranchDropOffFeeService;
        }

        public void Handle(InterBranchDropOffFeeEditCommand command) {
            command.Model = _interBranchDropOffFeeService.Update(command.Model);
        }
    }
}
