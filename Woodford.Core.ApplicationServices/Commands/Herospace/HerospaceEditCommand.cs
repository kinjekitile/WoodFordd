using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class HerospaceEditCommand : ICommand {
        public HerospaceItemModel Model { get; set; }
    }

    public class HerospaceEditCommandHandler : ICommandHandler<HerospaceEditCommand> {
        private readonly IHerospaceService _herospaceService;        
        public HerospaceEditCommandHandler(IHerospaceService herospaceService) {
            _herospaceService = herospaceService;            
        }

        public void Handle(HerospaceEditCommand command) {
            _herospaceService.Update(command.Model);
        }
    }
}
