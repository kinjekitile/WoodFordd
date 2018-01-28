using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRateRepository {
        RateModel Create(RateModel model);
        RateModel Update(RateModel model);
        RateModel GetById(int id);
        void MarkAsDeleted(int id);
        List<RateModel> Get(RateFilterModel filter);
        List<RateModel> GetRatesForSearchCriteria(SearchCriteriaModel criteria, List<int> availableVehicleGroupIds);
        //void Dispose();
    }
}
