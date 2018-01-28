using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class UserSetEmailCommand : ICommand {
        public int UserId { get; set; }
        public string Email { get; set; }
        
    }

    public class UserSetEmailCommandHandler : ICommandHandler<UserSetEmailCommand> {
        private IUserService _userService;

        public UserSetEmailCommandHandler(IUserService userService) {
            _userService = userService;
        }
        public void Handle(UserSetEmailCommand command) {
            _userService.SetEmailUsername(command.UserId, command.Email);
        }
    }
}
