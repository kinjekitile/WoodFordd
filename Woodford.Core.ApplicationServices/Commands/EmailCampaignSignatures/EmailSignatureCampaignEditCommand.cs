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
    public class EmailSignatureCampaignEditCommand : ICommand {
        public EmailSignatureCampaignModel Model { get; set; }
    }

    public class EmailSignatureCampaignEditCommandHandler : ICommandHandler<EmailSignatureCampaignEditCommand> {
        private readonly IEmailSignatureCampaignService _emailSignatureCampaignService;
        public EmailSignatureCampaignEditCommandHandler(IEmailSignatureCampaignService emailSignatureCampaignService) {
            _emailSignatureCampaignService = emailSignatureCampaignService;
        }
        public void Handle(EmailSignatureCampaignEditCommand command) {
            command.Model = _emailSignatureCampaignService.Update(command.Model);
        }
    }
}
