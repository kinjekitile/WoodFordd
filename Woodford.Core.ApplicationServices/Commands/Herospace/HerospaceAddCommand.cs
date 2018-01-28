using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class HerospaceAddCommand : ICommand {
        public HerospaceItemModel Model { get; set; }
    }

    public class HerospaceAddCommandHandler : ICommandHandler<HerospaceAddCommand> {
        private readonly IHerospaceService _herospaceService;        
        public HerospaceAddCommandHandler(IHerospaceService herospaceService) {
            _herospaceService = herospaceService;            
        }

        public void Handle(HerospaceAddCommand command) {
            command.Model = _herospaceService.Create(command.Model);
        }
    }
}
