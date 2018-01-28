using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class CorporateEditCommand : ICommand {
        public CorporateModel Model { get; set; }

    }

    public class CorporateEditCommandHandler : ICommandHandler<CorporateEditCommand> {
        private readonly ICorporateService _corpService;
        public CorporateEditCommandHandler(ICorporateService corpService) {
            _corpService = corpService;
        }
        public void Handle(CorporateEditCommand command) {
            command.Model = _corpService.Update(command.Model);
        }
    }
}
