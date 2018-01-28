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
    public class VehicleExtrasService : IVehicleExtrasService {
        private readonly IVehicleExtrasRepository _repo;
        public VehicleExtrasService(IVehicleExtrasRepository repo) {
            _repo = repo;
        }

        public VehicleExtrasModel Edit(VehicleExtrasModel model) {
            return _repo.Edit(model);
        }

        public List<VehicleExtrasModel> Get() {
            return _repo.Get();
        }

        public VehicleExtrasModel GetById(int id) {
            return _repo.GetById(id);
        }

        public List<VehicleExtrasModel> GetByIds(List<int> ids) {
            return _repo.GetByIds(ids);
        }
    }
}
