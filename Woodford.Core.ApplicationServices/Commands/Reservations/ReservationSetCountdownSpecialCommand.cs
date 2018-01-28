using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationSetCountdownSpecialCommand : ICommand {
        public int CountDownSpecialId { get; set; }
        public int ReservationId { get; set; }
        public CountdownSpecialType SpecialType { get; set; }
        public string TextReward { get; set; }
        public int VehicleUpgradeId { get; set; }
        public decimal Amount { get; set; }
        public ReservationModel ReservationOut { get; set; }
    }

    public class ReservationSetCountdownSpecialCommandHandler : ICommandHandler<ReservationSetCountdownSpecialCommand> {
        private readonly IReservationService _reservationService;
        public ReservationSetCountdownSpecialCommandHandler(IReservationService reservationService) {
            _reservationService = reservationService;
        }
        public void Handle(ReservationSetCountdownSpecialCommand command) {
            command.ReservationOut = _reservationService.SetCountdownSpecial(command.ReservationId, command.CountDownSpecialId, command.SpecialType, command.TextReward, command.VehicleUpgradeId, command.Amount);
        }
    }
}
