using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Services;

namespace Woodford.Core.ApplicationServices.Commands {
    public class EmailSignatureAddCommand : ICommand {
        public EmailSignatureModel Model { get; set; }

    }

    public class EmailSignatureAddCommandHandler : ICommandHandler<EmailSignatureAddCommand> {
        private readonly IEmailSignatureService _emailSignatureService;
        public EmailSignatureAddCommandHandler(IEmailSignatureService emailSignatureService) {
            _emailSignatureService = emailSignatureService;
        }
        public void Handle(EmailSignatureAddCommand command) {
            command.Model = _emailSignatureService.Create(command.Model);
        }
    }
}
