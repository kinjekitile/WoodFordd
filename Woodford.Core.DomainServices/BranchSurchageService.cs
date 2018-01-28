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
    public class BranchSurchargeService : IBranchSurchargeService {
        private readonly IBranchSurchageRepository _repo;
        public BranchSurchargeService(IBranchSurchageRepository repo) {
            _repo = repo;
        }
        public BranchSurchargeModel Create(BranchSurchargeModel model) {
            return _repo.Create(model);
        }

        public ListOf<BranchSurchargeModel> Get(BranchSurchargeFilterModel filter, ListPaginationModel pagination) {
            ListOf<BranchSurchargeModel> res = new ListOf<BranchSurchargeModel>();


            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;
        }

        public BranchSurchargeModel GetById(int id) {
            return _repo.GetById(id);
        }

        public BranchSurchargeModel Update(BranchSurchargeModel model) {
            return _repo.Update(model);
        }
    }
}
