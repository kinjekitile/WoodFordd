using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class InterBranchDropOffFeeAddCommand : ICommand {
        public InterBranchDropOffFeeModel Model { get; set; }               
    }

    public class InterBranchDropOffFeeAddCommandHandler : ICommandHandler<InterBranchDropOffFeeAddCommand> {
        private readonly IInterBranchDropOffFeeService _interBranchDropOffFeeService;
        public InterBranchDropOffFeeAddCommandHandler(IInterBranchDropOffFeeService interBranchDropOffFeeService) {
            _interBranchDropOffFeeService = interBranchDropOffFeeService;
        }

        public void Handle(InterBranchDropOffFeeAddCommand command) {
            command.Model = _interBranchDropOffFeeService.Create(command.Model);
        }
    }
}
