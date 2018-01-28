using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class UserCreateCommand : ICommand {
        public UserModel User { get; set; }
        public List<UserRoles> Roles { get; set; }
        public string Password { get; set; }
        public bool Success { get; set; }
    }

    public class UserCreateCommandHandler : ICommandHandler<UserCreateCommand> {

        private IAuthenticate _authenticate;

        public UserCreateCommandHandler(IAuthenticate authenticate) {
            _authenticate = authenticate;
        }
        public void Handle(UserCreateCommand command) {
            command.User.DateCreated = DateTime.Now;
            string result = _authenticate.CreateUser(command.User, command.Password);
            if (command.Roles != null) {
                _authenticate.AssignRolesToUser(command.User.Email, command.Roles);
            }
        }
    }
}
