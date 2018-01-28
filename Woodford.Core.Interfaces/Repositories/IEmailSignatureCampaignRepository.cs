using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces.Repositories {
    public interface IEmailSignatureCampaignRepository {
        EmailSignatureCampaignModel Create(EmailSignatureCampaignModel model);
        EmailSignatureCampaignModel Update(EmailSignatureCampaignModel model);
        EmailSignatureCampaignModel GetById(int id);
        List<EmailSignatureCampaignModel> Get(EmailSignatureCampaignFilterModel filter, ListPaginationModel pagination);
        int GetCount(EmailSignatureCampaignFilterModel filter);
    }
}
