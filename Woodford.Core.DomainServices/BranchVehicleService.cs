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
    public class BranchVehicleService : IBranchVehicleService {
        private IBranchVehicleRepository _repo;
        private IVehicleService _vehicleService;
        public BranchVehicleService(IBranchVehicleRepository repo, IVehicleService vehicleService) {
            _repo = repo;
            _vehicleService = vehicleService;
        }

        public BranchVehicleModel Create(BranchVehicleModel model) {
            return _repo.Create(model);
        }

        public void Delete(int id) {
            _repo.Delete(id);
        }

        public ListOf<BranchVehicleModel> Get(BranchVehicleFilterModel filter, ListPaginationModel pagination) {
            ListOf<BranchVehicleModel> res = new ListOf<BranchVehicleModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, pagination);
            }

            if (!filter.ShowPastExclusions) {
                foreach (var item in res.Items) {
                    item.Exclusions = item.Exclusions.Where(x => x.EndDate >= DateTime.Today);
                }
            }

            return res;
        }

        public List<VehicleModel> GetAvailableVehicles(int branchId, DateTime pickupDate) {
            List<int> availableVehicleIds = _repo.GetAvailableVehicleIds(branchId, pickupDate);
            return _vehicleService.GetByIds(availableVehicleIds, false);            
        }

        public BranchVehicleModel GetById(int id) {
            return _repo.GetById(id);
        }

        public BranchVehicleModel Update(BranchVehicleModel model) {
            return _repo.Update(model);
        }
    }
}
