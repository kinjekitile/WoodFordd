using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class UserSetCorporateCommand : ICommand {
        public int UserId { get; set; }
        public int? CorporateId { get; set; }
        
    }

    public class UserSetCorporateCommandHandler : ICommandHandler<UserSetCorporateCommand> {
        private IUserService _userService;

        public UserSetCorporateCommandHandler(IUserService userService) {
            _userService = userService;
        }
        public void Handle(UserSetCorporateCommand command) {
            _userService.SetCorporate(command.UserId, command.CorporateId);
        }
    }
}
