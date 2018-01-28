using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface ICorporateService {
        CorporateModel Create(CorporateModel model);
        CorporateModel Update(CorporateModel model);
        CorporateModel GetById(int id);
        ListOf<CorporateModel> Get(CorporateFilterModel filter, ListPaginationModel pagination);

        void AddRateCodeToCorporate(int corporateId, int rateCodeId);
        void RemoveRateCodeFromCorporate(int corporateId, int rateCodeId);
    }
}
