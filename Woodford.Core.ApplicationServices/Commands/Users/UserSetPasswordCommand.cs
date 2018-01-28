using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class UserSetPasswordCommand : ICommand {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        
    }

    public class UserSetPasswordCommandHandler : ICommandHandler<UserSetPasswordCommand> {
        private IUserService _userService;
        private readonly IAuthenticate _auth;

        public UserSetPasswordCommandHandler(IUserService userService, IAuthenticate auth) {
            _userService = userService;
            _auth = auth;
        }
        public void Handle(UserSetPasswordCommand command) {
            UserModel user = _userService.GetById(command.UserId);
            var token = _auth.GeneratePasswordResetToken(user.Email);

            _auth.ResetPasswordWithToken(command.NewPassword, token);
        }
    }
}
