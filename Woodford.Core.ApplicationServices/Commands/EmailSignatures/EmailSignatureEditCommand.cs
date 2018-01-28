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
    public class EmailSignatureEditCommand : ICommand {
        public EmailSignatureModel Model { get; set; }
    }

    public class EmailSignatureEditCommandHandler : ICommandHandler<EmailSignatureEditCommand> {
        private readonly IEmailSignatureService _emailSignatureService;
        public EmailSignatureEditCommandHandler(IEmailSignatureService emailSignatureService) {
            _emailSignatureService = emailSignatureService;
        }
        public void Handle(EmailSignatureEditCommand command) {
            command.Model = _emailSignatureService.Update(command.Model);
        }
    }
}
