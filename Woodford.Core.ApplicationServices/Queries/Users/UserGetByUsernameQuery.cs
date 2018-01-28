using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries
{
    public class UserGetByUsernameQuery : IQuery<UserModel>
    {
        public string UserName { get; set; }
    }

    public class UserGetByUsernameQueryHandler : IQueryHandler<UserGetByUsernameQuery, UserModel>
    {
        private IUserService _userService;        
        public UserGetByUsernameQueryHandler(IUserService userService)
        {
            _userService = userService;
        }
        public UserModel Process(UserGetByUsernameQuery query)
        {
            UserModel u = _userService.GetByUsername(query.UserName);
            return u;                
        }
    }
}
