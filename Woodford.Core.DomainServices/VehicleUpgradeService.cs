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
    public class VehicleUpgradeService : IVehicleUpgradeService {
        private readonly IVehicleUpgradeRepository _repo;        

        public VehicleUpgradeService(IVehicleUpgradeRepository repo) {
            _repo = repo;            
        }

        public VehicleUpgradeModel Create(VehicleUpgradeModel model) {
            return _repo.Create(model);
        }

        public VehicleUpgradeModel Update(VehicleUpgradeModel model) {
            return _repo.Update(model);

        }

        public VehicleUpgradeModel GetById(int id) {
            return _repo.GetById(id);
        }
        
        public ListOf<VehicleUpgradeModel> Get(VehicleUpgradeFilterModel filter, ListPaginationModel pagination) {
            ListOf<VehicleUpgradeModel> res = new ListOf<VehicleUpgradeModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;
        }

        public VehicleUpgradeModel MarkAs(int id, bool markAs) {
            VehicleUpgradeModel v = _repo.GetById(id);
            v.IsActive = markAs;
            v = _repo.Update(v);
            return v;
        }
    }
}
