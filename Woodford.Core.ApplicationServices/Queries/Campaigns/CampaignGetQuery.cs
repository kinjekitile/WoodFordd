using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class CampaignGetQuery : IQuery<ListOf<CampaignModel>> {
        public CampaignFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class CampaignGetQueryHandler : IQueryHandler<CampaignGetQuery, ListOf<CampaignModel>> {
        private readonly ICampaignService _campaignService;
        public CampaignGetQueryHandler(ICampaignService campaignService) {
            _campaignService = campaignService;
        }
        public ListOf<CampaignModel> Process(CampaignGetQuery query) {
            return _campaignService.Get(query.Filter, query.Pagination);
        }
    }
}
