using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class UrlRedirectAddCommand : ICommand {
        public UrlRedirectModel Model { get; set; }
    }

    public class UrlRedirectAddCommandHandler : ICommandHandler<UrlRedirectAddCommand> {
        private readonly IUrlRedirectService _redirectService;
        public UrlRedirectAddCommandHandler(IUrlRedirectService redirectService) {
            _redirectService = redirectService;
        }
        public void Handle(UrlRedirectAddCommand command) {
            command.Model = _redirectService.Create(command.Model);
        }
    }
}
