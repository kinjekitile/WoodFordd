using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRateRuleService {
        RateRuleModel Create(RateRuleModel model);
        RateRuleModel Update(RateRuleModel model);
        RateRuleModel GetById(int id);
        ListOf<RateRuleModel> Get(RateRuleFilterModel filter, ListPaginationModel pagination);
    }
}
