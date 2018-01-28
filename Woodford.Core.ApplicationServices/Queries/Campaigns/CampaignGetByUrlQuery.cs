using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class CampaignGetByUrlQuery : IQuery<CampaignModel> {
        public string Url { get; set; }
        public bool IncludePageContent { get; set; }
    }

    public class CampaignGetByUrlQueryHandler : IQueryHandler<CampaignGetByUrlQuery, CampaignModel> {
        private readonly ICampaignService _campaignService;
        public CampaignGetByUrlQueryHandler(ICampaignService campaignService) {
            _campaignService = campaignService;
        }

        public CampaignModel Process(CampaignGetByUrlQuery query) {
            return _campaignService.GetByUrl(query.Url, query.IncludePageContent);
        }
    }
}
