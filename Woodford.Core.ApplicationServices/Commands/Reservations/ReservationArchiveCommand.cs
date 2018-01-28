using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationArchiveCommand : ICommand {
        
        public int ReservationId { get; set; }
        public bool IsArchived { get; set; }
    }

    public class ReservationArchiveCommandHandler : ICommandHandler<ReservationArchiveCommand> {
        
        private readonly IReservationService _reservationService;
        
        public ReservationArchiveCommandHandler(IReservationService reservationService) {
            _reservationService = reservationService;
        }
        public void Handle(ReservationArchiveCommand command) {
            _reservationService.Archive(command.ReservationId, command.IsArchived);
        }
    }
}
