using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class UserSetLoyaltyTierFixedCommand : ICommand {
        public int UserId { get; set; }
        public bool IsFixedTier { get; set; }

    }

    public class UserSetLoyaltyTierFixedCommandHandler : ICommandHandler<UserSetLoyaltyTierFixedCommand> {
        private IUserService _userService;

        public UserSetLoyaltyTierFixedCommandHandler(IUserService userService) {
            _userService = userService;
        }
        public void Handle(UserSetLoyaltyTierFixedCommand command) {
            _userService.SetLoyaltyTierNoDropTier(command.UserId, command.IsFixedTier);
        }
    }
}
