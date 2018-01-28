using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class RolesRepository : RepositoryBase, IRolesRepository {

        public RolesRepository(IDbConnectionConfig connection) : base(connection) {

        }

        public List<RoleModel> Get() {
            return _db.webpages_Roles.Select(x => new RoleModel {
                Id = x.RoleId,
                Title = x.RoleName
            }).ToList();
        }
    }
}
