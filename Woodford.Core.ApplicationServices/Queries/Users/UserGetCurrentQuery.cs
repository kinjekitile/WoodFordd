using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries
{
    public class UserGetCurrentQuery : IQuery<UserModel>
    {
        
    }

    public class UserGetCurrentQueryHandler : IQueryHandler<UserGetCurrentQuery, UserModel>
    {
        private IUserService _userService;
        private IAuthenticate _auth;
        public UserGetCurrentQueryHandler(IUserService userService, IAuthenticate auth)
        {
            _userService = userService;
            _auth = auth;
        }
        public UserModel Process(UserGetCurrentQuery query)
        {
            string email = _auth.CurrentUserName();

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            else
            {
                return _userService.GetByUsername(email);
            }            
        }
    }
}
