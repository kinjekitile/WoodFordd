using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RolesGetQuery : IQuery<ListOf<RoleModel>> {
        public BranchFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class RolesGetQueryHandler : IQueryHandler<RolesGetQuery, ListOf<RoleModel>> {
        private readonly IRolesService _rolesService;
        public RolesGetQueryHandler(IRolesService rolesService) {
            _rolesService = rolesService;
        }

        public ListOf<RoleModel> Process(RolesGetQuery query) {
            ListOf<RoleModel> result = new ListOf<RoleModel>();
            result.Items = _rolesService.Get();
            return result;
        }
    }
}
