using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationAddBranchSurchargesCommand : ICommand {
        //public ReservationModel Reservation { get; set; }
        public int ReservationId { get; set; }     
    }

    public class ReservationAddBranchSurchargesCommandHandler : ICommandHandler<ReservationAddBranchSurchargesCommand> {
        
        private readonly IReservationService _reservationService;
        private readonly IBranchSurchargeService _branchSurchargeService;

        public ReservationAddBranchSurchargesCommandHandler(IReservationService reservationService, IBranchSurchargeService branchSurchargeService) {
            _reservationService = reservationService;
            _branchSurchargeService = branchSurchargeService;

        }
        public void Handle(ReservationAddBranchSurchargesCommand command) {
            ReservationModel reservation = _reservationService.GetById(command.ReservationId);

            List<BranchSurchargeModel> surcharges = _branchSurchargeService.Get(new BranchSurchargeFilterModel { BranchId = reservation.PickupBranchId }, null).Items;

            _reservationService.AddBranchSurcharges(command.ReservationId, surcharges);
        }
    }
}
