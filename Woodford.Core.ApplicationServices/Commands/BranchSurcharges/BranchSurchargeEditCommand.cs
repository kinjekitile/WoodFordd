using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchSurchargeEditCommand : ICommand {
        public BranchSurchargeModel Model { get; set; }               
    }

    public class BranchSurchargeEditCommandHandler : ICommandHandler<BranchSurchargeEditCommand> {
        private readonly IBranchSurchargeService _surchargeService;
        public BranchSurchargeEditCommandHandler(IBranchSurchargeService surchargeService) {
            _surchargeService = surchargeService;
        }

        public void Handle(BranchSurchargeEditCommand command) {
            _surchargeService.Update(command.Model);
        }
    }
}
