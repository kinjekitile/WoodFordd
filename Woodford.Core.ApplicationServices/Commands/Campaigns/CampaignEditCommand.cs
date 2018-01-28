using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class CampaignEditCommand : ICommand {
        public CampaignModel Model { get; set; }
    }

    public class CampaignEditCommandHandler : ICommandHandler<CampaignEditCommand> {
        private readonly ICampaignService _campaignService;
        public CampaignEditCommandHandler(ICampaignService campaignService) {
            _campaignService = campaignService;
        }
        public void Handle(CampaignEditCommand command) {
            command.Model = _campaignService.Update(command.Model);
        }
    }
}
