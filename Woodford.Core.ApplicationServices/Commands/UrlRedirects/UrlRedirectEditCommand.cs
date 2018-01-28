using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class UrlRedirectEditCommand : ICommand {
        public UrlRedirectModel Model { get; set; }
    }

    public class UrlRedirectEditCommandHandler : ICommandHandler<UrlRedirectEditCommand> {
        private readonly IUrlRedirectService _redirectService;
        public UrlRedirectEditCommandHandler(IUrlRedirectService redirectService) {
            _redirectService = redirectService;
        }
        public void Handle(UrlRedirectEditCommand command) {
            command.Model = _redirectService.Update(command.Model);
        }
    }
}
