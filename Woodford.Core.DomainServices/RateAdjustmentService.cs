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
    public class RateAdjustmentService : IRateAdjustmentService {

        private readonly IRateAdjustmentRepository _repo;
        public RateAdjustmentService(IRateAdjustmentRepository repo) {
            _repo = repo;
        }
        public RateAdjustmentModel Create(RateAdjustmentModel model) {
            return _repo.Create(model);
        }

        public RateAdjustmentModel Delete(int id) {
            return _repo.Delete(id);
        }

        public ListOf<RateAdjustmentModel> Get(RateAdjustmentFilterModel filter, ListPaginationModel pagination) {
            ListOf<RateAdjustmentModel> res = new ListOf<RateAdjustmentModel>();
            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }
            return res;
        }

        public List<RateAdjustmentModel> GetAdjustmentsForSearchCriteria(SearchCriteriaModel criteria) {
            return _repo.GetAdjustmentsForSearchCriteria(criteria);
        }

        public RateAdjustmentModel GetById(int id) {
            return _repo.GetById(id);
        }

        public RateAdjustmentModel Update(RateAdjustmentModel model) {
            return _repo.Update(model);
        }
    }
}
