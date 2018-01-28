using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface ICampaignRepository {
        CampaignModel Create(CampaignModel model);
        CampaignModel Update(CampaignModel model);
        CampaignModel GetById(int id);
        CampaignModel GetByUrl(string url);
        List<CampaignModel> Get(CampaignFilterModel filter, ListPaginationModel pagination);
        int GetCount(CampaignFilterModel filter);
    }
}
