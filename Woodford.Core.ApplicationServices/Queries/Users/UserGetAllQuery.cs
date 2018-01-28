using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class UserGetAllQuery : IQuery<ListOf<UserModel>> {
        public ListPaginationModel Pagination { get; set; }
    }

    public class UserGetAllQueryHandler : IQueryHandler<UserGetAllQuery, ListOf<UserModel>> {
        private readonly IUserService _userService;
        public UserGetAllQueryHandler(IUserService userService) {
            _userService = userService;
        }

        public ListOf<UserModel> Process(UserGetAllQuery query) {
            return _userService.GetAll(query.Pagination);
        }
    }
}
