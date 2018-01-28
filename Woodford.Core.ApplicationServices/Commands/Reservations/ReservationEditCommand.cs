using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationEditCommand : ICommand {
        public ReservationModel Reservation { get; set; }
    }

    public class ReservationEditCommandHandler : ICommandHandler<ReservationEditCommand> {
        private readonly IReservationService _reservationService;
        public ReservationEditCommandHandler(IReservationService reservationService) {
            _reservationService = reservationService;
        }
        public void Handle(ReservationEditCommand command) {
            command.Reservation = _reservationService.Update(command.Reservation);
        }
    }
}
