using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationSetUpgradeCommand : ICommand {
        public int ReservationId { get; set; }
        public int UpgradeId { get; set; }
        public decimal UpgradePrice { get; set; }
        public ReservationModel ReservationOut { get; set; }
    }

    public class ReservationSetUpgradeCommandHandler : ICommandHandler<ReservationSetUpgradeCommand> {
        private readonly IReservationService _reservationService;
        public ReservationSetUpgradeCommandHandler(IReservationService reservationService) {
            _reservationService = reservationService;
        }
        public void Handle(ReservationSetUpgradeCommand command) {
            command.ReservationOut = _reservationService.SetUpgrade(command.ReservationId, command.UpgradeId, command.UpgradePrice);
        }
    }
}
