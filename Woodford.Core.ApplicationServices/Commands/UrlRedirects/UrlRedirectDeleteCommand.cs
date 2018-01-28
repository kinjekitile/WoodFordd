using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class UrlRedirectDeleteCommand : ICommand {
        public int Id { get; set; }
    }

    public class UrlRedirectDeleteCommandHandler : ICommandHandler<UrlRedirectDeleteCommand> {
        private readonly IUrlRedirectService _redirectService;
        public UrlRedirectDeleteCommandHandler(IUrlRedirectService redirectService) {
            _redirectService = redirectService;
        }
        public void Handle(UrlRedirectDeleteCommand command) {
            _redirectService.Delete(command.Id);
        }
    }
}
