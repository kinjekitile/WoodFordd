using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces
{
    public interface IAuthenticate
    {        
        string CurrentUserName();
        string CreateUser(UserModel model, string password);
        
        string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow = 1440);
        bool UserExists(string username);
        bool LogOn(string username, string password);
        void LogOff();
        bool ChangePassword(string username, string currentPassword, string newPassword);
        bool ResetPassword(string username);
        bool ResetPasswordWithToken(string password, string token);
        bool AssignRolesToUser(string username, List<UserRoles> roles);
    }

}
