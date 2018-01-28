using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
	public class UserUpdateCommand : ICommand {
		public UserModel Model { get; set; }
	}

	public class UserUpdateCommandHandler : ICommandHandler<UserUpdateCommand> {

		private IUserService _userService;

		public UserUpdateCommandHandler(IUserService userService) {
			_userService = userService;
		}
		public void Handle(UserUpdateCommand command) {			
			_userService.UpdateUser(command.Model);
		}
	}
}
