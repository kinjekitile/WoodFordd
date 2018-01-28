using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class UserResetPasswordFromTokenCommand : ICommand {
        public string Password { get; set; }
        public string ResetToken { get; set; }
    }

    public class UserResetPasswordFromTokenCommandHandler : ICommandHandler<UserResetPasswordFromTokenCommand> {
        private readonly IAuthenticate _authenticate;

        public UserResetPasswordFromTokenCommandHandler(IAuthenticate authenticate) {
            _authenticate = authenticate;
        }

        public void Handle(UserResetPasswordFromTokenCommand command) {
            _authenticate.ResetPasswordWithToken(command.Password, command.ResetToken);
        }
    }
}
