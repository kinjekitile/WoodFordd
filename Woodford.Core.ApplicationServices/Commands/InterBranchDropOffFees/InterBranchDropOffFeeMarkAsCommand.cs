using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class InterBranchDropOffFeeMarkAsCommand : ICommand {
        public int Id { get; set; }        
        public bool MarkAs { get; set; }
        public InterBranchDropOffFeeModel ModelOut { get; set; }
    }

    public class InterBranchDropOffFeeMarkAsCommandHandler : ICommandHandler<InterBranchDropOffFeeMarkAsCommand> {
        private readonly IInterBranchDropOffFeeService _interBranchDropOffFeeService;
        public InterBranchDropOffFeeMarkAsCommandHandler(IInterBranchDropOffFeeService interBranchDropOffFeeService) {
            _interBranchDropOffFeeService = interBranchDropOffFeeService;            
        }

        public void Handle(InterBranchDropOffFeeMarkAsCommand command) {
            command.ModelOut = _interBranchDropOffFeeService.MarkAs(command.Id, command.MarkAs);
        }
    }
}
