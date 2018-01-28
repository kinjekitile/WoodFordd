using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;


namespace Woodford.Core.Interfaces {
    public interface ICorporateRepository {
        CorporateModel Create(CorporateModel model);
        CorporateModel Update(CorporateModel model);
        CorporateModel GetById(int id);
        List<CorporateModel> Get(CorporateFilterModel filter, ListPaginationModel pagination);
        int GetCount(CorporateFilterModel filter);

        void AddRateCodeToCorporate(int corporateId, int rateCodeId);
        void RemoveRateCodeFromCorporate(int corporateId, int rateCodeId);
    }
}
