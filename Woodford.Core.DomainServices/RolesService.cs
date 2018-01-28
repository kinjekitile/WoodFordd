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
    public class RolesService : IRolesService {
        private readonly IRolesRepository _repo;
        public RolesService(IRolesRepository repo) {
            _repo = repo;
        }
        public List<RoleModel> Get() {
            return _repo.Get();
        }
    }
}
