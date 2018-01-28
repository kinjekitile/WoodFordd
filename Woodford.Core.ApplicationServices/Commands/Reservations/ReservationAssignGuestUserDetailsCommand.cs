using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationAssignGuestUserDetailsCommand : ICommand {
        public int ReservationId { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public string MobileNumber { get; set; }
    }

    public class ReservationAssignGuestUserDetailsCommandHandler : ICommandHandler<ReservationAssignGuestUserDetailsCommand> {
        
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;   

        public ReservationAssignGuestUserDetailsCommandHandler(IReservationService reservationService, IUserService userService) {
            _reservationService = reservationService;
            _userService = userService;
        }
        public void Handle(ReservationAssignGuestUserDetailsCommand command) {                        
            _reservationService.AssignGuestUser(command.ReservationId, command.FirstName, command.LastName, command.Email, command.IdNumber, command.MobileNumber);
        }
    }
}
