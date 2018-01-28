using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationAssignUserCommand : ICommand {
        public int ReservationId { get; set; }  
        public int UserId { get; set; }
    }

    public class ReservationAssignUserCommandHandler : ICommandHandler<ReservationAssignUserCommand> {
        
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;
        private readonly ILoyaltyService _loyaltyService;

        public ReservationAssignUserCommandHandler(IReservationService reservationService, IUserService userService, ILoyaltyService loyaltyService) {
            _reservationService = reservationService;
            _userService = userService;
            _loyaltyService = loyaltyService;
        }
        public void Handle(ReservationAssignUserCommand command) { 
           
            _reservationService.AssignUser(command.ReservationId, command.UserId);
        }
    }
}
