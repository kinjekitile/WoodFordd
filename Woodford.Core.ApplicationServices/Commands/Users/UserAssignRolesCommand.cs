using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class UserAssignRolesCommand : ICommand {
        public UserModel User { get; set; }
        public List<UserRoles> Roles { get; set; }
        public bool Success { get; set; }
    }

    public class UserAssignRolesCommandHandler : ICommandHandler<UserAssignRolesCommand> {
        private IAuthenticate _authenticate;

        public UserAssignRolesCommandHandler(IAuthenticate authenticate) {
            _authenticate = authenticate;
        }

        public void Handle(UserAssignRolesCommand command) {            
            _authenticate.AssignRolesToUser(command.User.Email, command.Roles);
            command.Success = true;
        }
    }
}
