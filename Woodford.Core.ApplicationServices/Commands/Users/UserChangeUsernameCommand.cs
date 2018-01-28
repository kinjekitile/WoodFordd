using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class UserChangeUsernameCommand : ICommand {
        public UserModel User { get; set; }
        public string NewUserName { get; set; }
    }

    public class UserChangeUsernameCommandHandler : ICommandHandler<UserChangeUsernameCommand> {
        private readonly IUserService _userService;
        public UserChangeUsernameCommandHandler(IUserService userService) {
            _userService = userService;
        }

        public void Handle(UserChangeUsernameCommand command) {
            _userService.ChangeUsername(command.User.Email, command.NewUserName);
        }
    }
}
