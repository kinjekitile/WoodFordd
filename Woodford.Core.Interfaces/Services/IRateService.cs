using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRateService {
        RateModel Create(RateModel model);
        RateModel Update(RateModel model);
        RateModel GetById(int id);
        void MarkAsDeleted(int id);
        ListOf<RateModel> Get(RateFilterModel filter, ListPaginationModel pagination);
        List<RateModel> GetRatesForSearchCriteria(SearchCriteriaModel criteria, List<int> availableVehicleGroupIds);
        
    }
}
