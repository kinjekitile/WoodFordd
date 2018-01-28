using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class UsersGetQuery : IQuery<ListOf<UserModel>> {
        public UserFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class UsersGetQueryHandler : IQueryHandler<UsersGetQuery, ListOf<UserModel>> {
        private readonly IUserService _userService;
        public UsersGetQueryHandler(IUserService userService) {
            _userService = userService;
        }

        public ListOf<UserModel> Process(UsersGetQuery query) {
            return _userService.Get(query.Filter, query.Pagination);
        }
    }
}
