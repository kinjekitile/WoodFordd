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
    public class BranchVehicleExclusionService : IBranchVehicleExclusionService {
        private IBranchVehicleExclusionRepository _repo;
        public BranchVehicleExclusionService(IBranchVehicleExclusionRepository repo) {
            _repo = repo;
        }

        public BranchVehicleExclusionModel Create(BranchVehicleExclusionModel model) {
            return _repo.Create(model);
        }

        public void Delete(int id) {
            _repo.Delete(id);
        }

        public ListOf<BranchVehicleExclusionModel> Get(BranchVehicleExclusionFilterModel filter, ListPaginationModel pagination) {
            ListOf<BranchVehicleExclusionModel> res = new ListOf<BranchVehicleExclusionModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, pagination);
            }

            return res;
        }

        public BranchVehicleExclusionModel GetById(int id) {
            return _repo.GetById(id);
        }

        public BranchVehicleExclusionModel Update(BranchVehicleExclusionModel model) {
            return _repo.Update(model);
        }
    }
}
