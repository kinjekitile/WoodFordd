using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class UserSetAccountDisabledCommand : ICommand {
        public int UserId { get; set; }
        public bool Disabled { get; set; }

    }

    public class UserSetAccountDisabledCommandHandler : ICommandHandler<UserSetAccountDisabledCommand> {
        private IUserService _userService;

        public UserSetAccountDisabledCommandHandler(IUserService userService) {
            _userService = userService;
        }
        public void Handle(UserSetAccountDisabledCommand command) {

            _userService.SetAccountDisabled(command.UserId, command.Disabled);
            
        }
    }
}
