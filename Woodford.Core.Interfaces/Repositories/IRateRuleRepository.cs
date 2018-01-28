using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRateRuleRepository {
        RateRuleModel Create(RateRuleModel model);
        RateRuleModel Update(RateRuleModel model);
        RateRuleModel GetById(int id);
        List<RateRuleModel> Get(RateRuleFilterModel filter, ListPaginationModel pagination);
        int GetCount(RateRuleFilterModel filter);
    }
}
