using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationSetAsAddedToExternalSystemCommand : ICommand {
        public int ReservationId { get; set; }
        public bool HasBeenAddedToExternalSystem { get; set; }

    }

    public class ReservationSetAsAddedToExternalSystemCommandHandler : ICommandHandler<ReservationSetAsAddedToExternalSystemCommand> {
        private readonly IReservationService _reservationService;

        public ReservationSetAsAddedToExternalSystemCommandHandler(IReservationService reservationService, IVoucherService voucherService) {
            _reservationService = reservationService;

        }
        public void Handle(ReservationSetAsAddedToExternalSystemCommand command) {
            _reservationService.SetAsAddedToExternalSystem(command.ReservationId, command.HasBeenAddedToExternalSystem);


        }
    }
}
