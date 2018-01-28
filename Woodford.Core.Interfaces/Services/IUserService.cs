using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
	public interface IUserService {
        void UpdateLoyaltyPeriod(int id, DateTime startDate, DateTime endDate);
        void AddUserToRole(int userId, int roleId, string roleName);
        void EnrollLoyalty(int id);
        void UpdateLoyaltyTier(int id, int loyaltyTierId);
        void SetLoyaltyTierNoDropTier(int id, bool isNoDropTier);
        void SetCorporate(int userId, int? corporateId);
        void SetEmailUsername(int userId, string email);
        void SetAccountDisabled(int userId, bool disabled);
        UserModel CreateUser(UserModel model, string password);
		UserModel UpdateUser(UserModel model);
		UserModel GetById(int id);
		ListOf<UserModel> GetAll(ListPaginationModel pagination);
        ListOf<UserModel> Get(UserFilterModel filter, ListPaginationModel pagination);

        List<UserModel> GetUsersByFilter(UserFilterModel filter);
		UserModel GetCurrentUser();
		UserModel GetByUsername(string username);
		bool UserExists(string username);
		void SendPasswordReset(string username);
		string GetPasswordResetToken(string username);
		bool ChangePassword(string username, string currentPassword, string newPassword);
        bool ChangeUsername(string oldUsername, string newUsername);
	}
}
