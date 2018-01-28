using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
	public class UserAuthenticateCommand : ICommand {
		public string Username { get; set; }
		public string Password { get; set; }
		public bool Success { get; set; }
	}

	public class UserAuthenticateCommandHandler : ICommandHandler<UserAuthenticateCommand> {

		private IAuthenticate _authenticate;
        private IUserService _userService;

		public UserAuthenticateCommandHandler(IAuthenticate authenticate, IUserService userService) {
			_authenticate = authenticate;
            _userService = userService;

        }
		public void Handle(UserAuthenticateCommand command) {
            var user = _userService.GetByUsername(command.Username);
            if (user.IsAccountDisabled) {
                command.Success = false;
            } else {
                command.Success = _authenticate.LogOn(command.Username, command.Password);
            }
			
		}
	}
}
