using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchSurchageAddCommand : ICommand {
        public BranchSurchargeModel Model { get; set; }               
    }

    public class BranchSurchageAddCommandHandler : ICommandHandler<BranchSurchageAddCommand> {
        private readonly IBranchSurchargeService _surchargeService;
        public BranchSurchageAddCommandHandler(IBranchSurchargeService surchargeService) {
            _surchargeService = surchargeService;
        }

        public void Handle(BranchSurchageAddCommand command) {
            command.Model = _surchargeService.Create(command.Model);
        }
    }
}
