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
    public class BranchRateCodeExclusionService : IBranchRateCodeExclusionService {
        private IBranchRateCodeExclusionRepository _repo;
        public BranchRateCodeExclusionService(IBranchRateCodeExclusionRepository repo) {
            _repo = repo;
        }

        public BranchRateCodeExclusionModel Create(BranchRateCodeExclusionModel model) {
            return _repo.Create(model);
        }

        public void Delete(int id) {
            _repo.Delete(id);
        }

        public ListOf<BranchRateCodeExclusionModel> Get(BranchRateCodeExclusionFilterModel filter, ListPaginationModel pagination) {
            ListOf<BranchRateCodeExclusionModel> res = new ListOf<BranchRateCodeExclusionModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, pagination);
            }

            return res;
        }

        public BranchRateCodeExclusionModel GetById(int id) {
            return _repo.GetById(id);
        }

        public BranchRateCodeExclusionModel Update(BranchRateCodeExclusionModel model) {
            return _repo.Update(model);
        }
    }
}
