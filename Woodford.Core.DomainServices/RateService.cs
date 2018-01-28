using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class RateService : IRateService {
        private readonly IRateRepository _repo;
        public RateService(IRateRepository repo) {
            _repo = repo;
        }

        public RateModel Create(RateModel model) {
            return _repo.Create(model);
            
        }

        public List<RateModel> Get(RateFilterModel filter) {
            return _repo.Get(filter);
        }
        public RateModel Update(RateModel model) {
            return _repo.Update(model);
        }

        public RateModel GetById(int id) {
            return _repo.GetById(id);
        }

        public ListOf<RateModel> Get(RateFilterModel filter, ListPaginationModel pagination) {
            var result = new ListOf<RateModel>();
            result.Items = _repo.Get(filter);
            return result;
        }

        public List<RateModel> GetRatesForSearchCriteria(SearchCriteriaModel criteria, List<int> availableVehicleGroupIds) {
            return _repo.GetRatesForSearchCriteria(criteria, availableVehicleGroupIds);
        }

        public void MarkAsDeleted(int id) {
            _repo.MarkAsDeleted(id);
        }
    }
}
