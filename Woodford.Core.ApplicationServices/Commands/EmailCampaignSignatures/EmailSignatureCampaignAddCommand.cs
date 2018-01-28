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
    public class EmailSignatureCampaignAddCommand : ICommand {
        public EmailSignatureCampaignModel Model { get; set; }

    }

    public class EmailSignatureCampaignAddCommandHandler : ICommandHandler<EmailSignatureCampaignAddCommand> {
        private readonly IEmailSignatureCampaignService _emailSignatureCampaignService;
        public EmailSignatureCampaignAddCommandHandler(IEmailSignatureCampaignService emailSignatureCampaignService) {
            _emailSignatureCampaignService = emailSignatureCampaignService;
        }
        public void Handle(EmailSignatureCampaignAddCommand command) {
            command.Model = _emailSignatureCampaignService.Create(command.Model);
        }
    }
}
