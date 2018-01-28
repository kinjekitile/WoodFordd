using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces.Services {
    public interface IEmailSignatureCampaignService {
        EmailSignatureCampaignModel Create(EmailSignatureCampaignModel model);
        EmailSignatureCampaignModel Update(EmailSignatureCampaignModel model);
        EmailSignatureCampaignModel GetById(int id);
        ListOf<EmailSignatureCampaignModel> Get(EmailSignatureCampaignFilterModel filter, ListPaginationModel pagination);
        

    }
}
