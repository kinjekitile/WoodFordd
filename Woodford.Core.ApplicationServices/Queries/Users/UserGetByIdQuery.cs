using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class UserGetByIdQuery : IQuery<UserModel> {
        public int Id { get; set; }
    }

    public class UserGetByIdQueryHandler : IQueryHandler<UserGetByIdQuery, UserModel> {
        private IUserService _userService;
        public UserGetByIdQueryHandler(IUserService userService) {
            _userService = userService;
        }
        public UserModel Process(UserGetByIdQuery query) {
            UserModel u = _userService.GetById(query.Id);
            return u;
        }
    }
}
