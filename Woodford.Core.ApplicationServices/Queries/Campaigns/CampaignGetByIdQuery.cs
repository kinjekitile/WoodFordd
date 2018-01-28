using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class CampaignGetByIdQuery : IQuery<CampaignModel> {
        public int Id { get; set; }
        public bool includePageContent { get; set; }
    }

    public class CampaignGetByIdQueryHandler : IQueryHandler<CampaignGetByIdQuery, CampaignModel> {
        private readonly ICampaignService _campaignService;
        public CampaignGetByIdQueryHandler(ICampaignService campaignService) {
            _campaignService = campaignService;
        }

        public CampaignModel Process(CampaignGetByIdQuery query) {
            return _campaignService.GetById(query.Id, query.includePageContent);
        }
    }
}
