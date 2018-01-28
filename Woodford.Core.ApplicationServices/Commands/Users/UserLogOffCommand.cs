using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
	public class UserLogOffCommand : ICommand {
	}

	public class UserLogOffCommandHandler : ICommandHandler<UserLogOffCommand> {
		IAuthenticate _authenticate;
		public UserLogOffCommandHandler(IAuthenticate authenticate) {
			_authenticate = authenticate;
		}

		public void Handle(UserLogOffCommand command) {
			_authenticate.LogOff();
		}
	}
}
