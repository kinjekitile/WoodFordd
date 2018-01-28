using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;


namespace Woodford.Core.Interfaces.Repositories {
    public interface IEmailSignatureRepository {
        EmailSignatureModel Create(EmailSignatureModel model);
        EmailSignatureModel Update(EmailSignatureModel model);
        EmailSignatureModel GetById(int id);
        List<EmailSignatureModel> Get(EmailSignatureFilterModel filter, ListPaginationModel pagination);
        int GetCount(EmailSignatureFilterModel filter);
    }
}
