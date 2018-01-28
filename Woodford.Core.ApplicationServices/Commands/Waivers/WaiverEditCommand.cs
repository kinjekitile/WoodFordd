using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class WaiverEditCommand : ICommand {
        public WaiverModel Model { get; set; }               
    }

    public class WaiverEditCommandHandler : ICommandHandler<WaiverEditCommand> {
        private readonly IWaiverService _waiverService;
        public WaiverEditCommandHandler(IWaiverService waiverService) {
            _waiverService = waiverService;
        }

        public void Handle(WaiverEditCommand command) {
            command.Model = _waiverService.Update(command.Model);
        }
    }
}
