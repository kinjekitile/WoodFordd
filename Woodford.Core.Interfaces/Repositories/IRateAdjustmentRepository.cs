using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRateAdjustmentRepository {
        RateAdjustmentModel Create(RateAdjustmentModel model);
        RateAdjustmentModel Update(RateAdjustmentModel model);
        RateAdjustmentModel Delete(int id);
        RateAdjustmentModel GetById(int id);
        List<RateAdjustmentModel> Get(RateAdjustmentFilterModel filter, ListPaginationModel pagination);
        int GetCount(RateAdjustmentFilterModel filter);
        List<RateAdjustmentModel> GetAdjustmentsForSearchCriteria(SearchCriteriaModel criteria);
    }
}
