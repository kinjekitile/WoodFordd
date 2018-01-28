using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationAddBenefitsCommand : ICommand {
        public int ReservationId { get; set; }
    }

    public class ReservationAddBenefitsCommandHandler : ICommandHandler<ReservationAddBenefitsCommand> {

        private readonly IReservationService _reservationService;
        private readonly ILoyaltyService _loyaltyService;
        private readonly IUserService _userService;
        public ReservationAddBenefitsCommandHandler(IReservationService reservationService, ILoyaltyService loyaltyService, IUserService userService) {
            _reservationService = reservationService;
            _loyaltyService = loyaltyService;
            _userService = userService;
        }
        public void Handle(ReservationAddBenefitsCommand command) {
            var user = _userService.GetCurrentUser();
            if (user != null) {
                if (user.IsLoyaltyMember) {
                    var benefits = _loyaltyService.GetTierBenefits((LoyaltyTierLevel)user.LoyaltyTierId, null);
                    _reservationService.AddBenefits(command.ReservationId, benefits.Items);
                } else {
                    _reservationService.AddBenefits(command.ReservationId, new List<LoyaltyTierBenefitModel>());
                }
            } else {
                _reservationService.AddBenefits(command.ReservationId, new List<LoyaltyTierBenefitModel>());
            }
        }
    }
}
