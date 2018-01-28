using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class LoyaltyTierEditCommand : ICommand {
        public LoyaltyTierModel Model { get; set; }
    }

    public class LoyaltyTierEditCommandHandler : ICommandHandler<LoyaltyTierEditCommand> {
        private readonly ILoyaltyService _loyaltyService;
        public LoyaltyTierEditCommandHandler(ILoyaltyService loyaltyService) {
            _loyaltyService = loyaltyService;
        }
        public void Handle(LoyaltyTierEditCommand command) {
            command.Model = _loyaltyService.Update(command.Model);
        }
    }
}
