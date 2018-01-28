using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces.Services {
    public interface IEmailSignatureService {
        EmailSignatureModel Create(EmailSignatureModel model);
        EmailSignatureModel Update(EmailSignatureModel model);
        EmailSignatureModel GetById(int id);
        ListOf<EmailSignatureModel> Get(EmailSignatureFilterModel filter, ListPaginationModel pagination);

        byte[] GetEmailSignatureImageData(EmailSignatureModel signature, EmailSignatureCampaignModel campaign);
    }
}
