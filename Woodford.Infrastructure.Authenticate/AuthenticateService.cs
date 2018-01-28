using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using WebMatrix.WebData;
using Woodford.Core.DomainModel.Enums;
using System.Web.Security;

namespace Woodford.Infrastructure.Authenticate
{
    public class AuthenticationService : IAuthenticate
    {
        private SimpleRoleProvider _roles;

        public AuthenticationService() {
            _roles = (SimpleRoleProvider)Roles.Provider;
        }

        public string CreateUser(UserModel model, string password)
        {
            object propertyValues = new {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MobileNumber = model.MobileNumber,
                DateCreated = model.DateCreated,
                IdNumber = model.IdNumber,
                LoyaltyTierId = (int)LoyaltyTierLevel.Green, 
                IsLoyaltyMember = model.IsLoyaltyMember,
                LoyaltySignUpDate = model.LoyaltySignUpDate,
                HasExistingLoyaltyNumber = model.HasExistingLoyaltyNumber,
                ExistingLoyaltyNumber = model.ExistingLoyaltyNumber
            };
            
            return WebSecurity.CreateUserAndAccount(model.Email, password, propertyValues);
        }


        public bool LogOn(string username, string password)
        {
            return WebSecurity.Login(username, password);
        }

        public void LogOff()
        {
            WebSecurity.Logout();
        }

        public string CurrentUserName()
        {
            return WebSecurity.CurrentUserName;
        }


        public string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow = 1440)
        {
            return WebSecurity.GeneratePasswordResetToken(username, tokenExpirationInMinutesFromNow);
        }

        public bool UserExists(string username)
        {
            return WebSecurity.UserExists(username);
        }

        public bool ChangePassword(string username, string currentPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(username, currentPassword, newPassword);
        }

        public bool ResetPassword(string username)
        {
            string token = GeneratePasswordResetToken(username);
            return WebSecurity.ResetPassword(token, createRandomPassword(10));
        }

        public bool ResetPasswordWithToken(string newPassword, string token)
        {
            bool res = WebSecurity.ResetPassword(token, newPassword);
            return res;
        }

        private string createRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public bool AssignRolesToUser(string username, List<UserRoles> roles) {
            
            if (WebSecurity.UserExists(username)) {                
                string[] allUserRoles = _roles.GetRolesForUser(username);
                _roles.RemoveUsersFromRoles(new string[] { username }, allUserRoles);
                List<string> rolesToAssign = new List<string>();
                foreach(UserRoles r in roles) {
                    rolesToAssign.Add(r.ToString());
                }
                _roles.AddUsersToRoles(new string[] { username }, rolesToAssign.ToArray());
                return true;
            } else {
                return false;
            }
        }
    }
}
