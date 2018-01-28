using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class WaiverDeleteCommand : ICommand {
        public int Id { get; set; }               
    }

    public class WaiverDeleteCommandHandler : ICommandHandler<WaiverDeleteCommand> {
        private readonly IWaiverService _waiverService;
        public WaiverDeleteCommandHandler(IWaiverService waiverService) {
            _waiverService = waiverService;
        }

        public void Handle(WaiverDeleteCommand command) {
            _waiverService.Delete(command.Id);
        }
    }
}
