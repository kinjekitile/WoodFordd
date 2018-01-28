using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class UserSetLoyaltyCommand : ICommand {
        public int UserId { get; set; }
        public int LoyaltyTierId { get; set; }
        
    }

    public class UserSetLoyaltyCommandHandler : ICommandHandler<UserSetLoyaltyCommand> {
        private IUserService _userService;

        public UserSetLoyaltyCommandHandler(IUserService userService) {
            _userService = userService;
        }
        public void Handle(UserSetLoyaltyCommand command) {
            _userService.UpdateLoyaltyTier(command.UserId, command.LoyaltyTierId);
        }
    }
}
