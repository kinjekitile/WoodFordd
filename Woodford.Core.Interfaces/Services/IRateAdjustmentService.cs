using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRateAdjustmentService {
        RateAdjustmentModel Create(RateAdjustmentModel model);
        RateAdjustmentModel Update(RateAdjustmentModel model);
        RateAdjustmentModel Delete(int id);
        RateAdjustmentModel GetById(int id);
        ListOf<RateAdjustmentModel> Get(RateAdjustmentFilterModel filter, ListPaginationModel pagination);
        List<RateAdjustmentModel> GetAdjustmentsForSearchCriteria(SearchCriteriaModel criteria);
    }
}
