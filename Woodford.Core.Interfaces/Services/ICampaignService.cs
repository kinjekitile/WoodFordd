using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface ICampaignService {
        CampaignModel Create(CampaignModel model);
        CampaignModel Update(CampaignModel model);
        CampaignModel GetById(int id, bool includePageContent = false);
        CampaignModel GetByUrl(string url, bool includePageContent = false);
        ListOf<CampaignModel> Get(CampaignFilterModel filter, ListPaginationModel pagination);
    }
}
