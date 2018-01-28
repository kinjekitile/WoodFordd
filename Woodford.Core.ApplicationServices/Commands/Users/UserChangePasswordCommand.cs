using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class UserChangePasswordCommand : ICommand {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserChangePasswordCommandHandler : ICommandHandler<UserChangePasswordCommand> {
        private readonly IUserService _userService;
        public UserChangePasswordCommandHandler(IUserService userService) {
            _userService = userService;
        }

        public void Handle(UserChangePasswordCommand command) {
            UserModel currentUser = _userService.GetCurrentUser();
            _userService.ChangePassword(currentUser.Email, command.OldPassword, command.NewPassword);
        }
    }
}
