using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class UserUpdateProfileCommand : ICommand {
        public UserModel User { get; set; }
    }


    public class UserUpdateProfileCommandHandler : ICommandHandler<UserUpdateProfileCommand> {

        private IUserService _userService;
        private IAuthenticate _authenticate;

        public UserUpdateProfileCommandHandler(IUserService userService, IAuthenticate authenticate) {
            _userService = userService;
            _authenticate = authenticate;
        }

        public void Handle(UserUpdateProfileCommand command) {
            string email = _authenticate.CurrentUserName();
            UserModel currentUser = _userService.GetByUsername(email);

            command.User.Id = currentUser.Id;
            command.User.Email = currentUser.Email;

            command.User = _userService.UpdateUser(command.User);
        }
    }
}
