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
    public class LoyaltyTierBenefitAddCommand : ICommand {
        public LoyaltyTierBenefitModel Model { get; set; }
    }


    public class LoyaltyTierBenefitAddCommandHandler : ICommandHandler<LoyaltyTierBenefitAddCommand> {
        private readonly ILoyaltyService _loyaltyService;

        public LoyaltyTierBenefitAddCommandHandler(ILoyaltyService loyaltyService) {
            _loyaltyService = loyaltyService;
        }
        public void Handle(LoyaltyTierBenefitAddCommand command) {
            command.Model = _loyaltyService.CreateBenefit(command.Model);
        }
    }
}
