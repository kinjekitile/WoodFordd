using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class CampaignAddCommand : ICommand {
        public CampaignModel Model { get; set; }

    }

    public class CampaignAddCommandHandler : ICommandHandler<CampaignAddCommand> {
        private readonly ICampaignService _campaignService;
        public CampaignAddCommandHandler(ICampaignService campaignService) {
            _campaignService = campaignService;
        }
        public void Handle(CampaignAddCommand command) {
            command.Model = _campaignService.Create(command.Model);
        }
    }
}
