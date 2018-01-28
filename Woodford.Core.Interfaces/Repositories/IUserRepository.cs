using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IUserRepository {
        void AddUserToRole(int userId, int roleId, string roleName);
        void UpdateLoyaltyPeriod(int id, DateTime startDate, DateTime endDate);
        void UpdateLoyaltyTier(int id, int loyaltyTierId);
        void SetLoyaltyTierNoDropTier(int id, bool isNoDropTier);
        void SetCorporate(int userId, int? corporateId);
        void SetAccountDisabled(int userId, bool disabled);
        void SetUsernameEmail(int userId, string email);
        UserModel Create(UserModel model);
        UserModel Update(UserModel model, bool updateUsername);
        UserModel GetById(int id);
        UserModel GetByUsername(string username);
        List<UserModel> Get(UserFilterModel filter, ListPaginationModel pagination);
        int GetCount(UserFilterModel filter);
        List<UserModel> GetUsersByFilter(UserFilterModel filter);
        //void Dispose();
    }
}
