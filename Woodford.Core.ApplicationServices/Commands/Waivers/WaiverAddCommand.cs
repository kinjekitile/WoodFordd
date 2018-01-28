using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class WaiverAddCommand : ICommand {
        public WaiverModel Model { get; set; }               
    }

    public class WaiverAddCommandHandler : ICommandHandler<WaiverAddCommand> {
        private readonly IWaiverService _waiverService;
        public WaiverAddCommandHandler(IWaiverService waiverService) {
            _waiverService = waiverService;
        }

        public void Handle(WaiverAddCommand command) {
            command.Model = _waiverService.Create(command.Model);
        }
    }
}
