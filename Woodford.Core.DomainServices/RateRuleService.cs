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
    public class RateRuleService : IRateRuleService {
        private readonly IRateRuleRepository _repo;
        public RateRuleService(IRateRuleRepository repo) {
            _repo = repo;
        }
        public RateRuleModel Create(RateRuleModel model) {
            return _repo.Create(model);
        }

        public ListOf<RateRuleModel> Get(RateRuleFilterModel filter, ListPaginationModel pagination) {
            var results = new ListOf<RateRuleModel>();
            results.Pagination = pagination;

            if (pagination == null) {
                results.Items = _repo.Get(filter, pagination);
            } else {
                results.Pagination.TotalItems = _repo.GetCount(filter);
                results.Items = _repo.Get(filter, results.Pagination);
            }
            
            return results;
        }

        public RateRuleModel GetById(int id) {
            return _repo.GetById(id);
        }        

        public RateRuleModel Update(RateRuleModel model) {
            return _repo.Update(model);
        }
    }
}
