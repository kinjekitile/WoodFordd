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
    public class LoyaltyTierBenefitEditCommand : ICommand {
        public LoyaltyTierBenefitModel Model { get; set; }
    }

    public class LoyaltyTierBenefitEditCommandHandler : ICommandHandler<LoyaltyTierBenefitEditCommand> {
        private readonly ILoyaltyService _loyaltyService;

        public LoyaltyTierBenefitEditCommandHandler(ILoyaltyService loyaltyService) {
            _loyaltyService = loyaltyService;
        }
        public void Handle(LoyaltyTierBenefitEditCommand command) {
            command.Model = _loyaltyService.UpdateBenefit(command.Model);
        }
    }
}
