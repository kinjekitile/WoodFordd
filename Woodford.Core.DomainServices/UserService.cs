using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
	public class UserService : IUserService {

		private IUserRepository _repo;
		private IAuthenticate _authenticate;
        
		//private INotify _notify;
		//private ISettingService _settings;
		//private IEmailResource _emailResource;
        //public UserService(IAuthenticate authenticate, IUserRepository repo, INotify notify, ISettingService settings, IEmailResource emailResource) {
        public UserService(IAuthenticate authenticate, IUserRepository repo)
        {
            _authenticate = authenticate;
			_repo = repo;
            
            //_notify = notify;
            //_settings = settings;
            //_emailResource = emailResource;
        }
        public void AddUserToRole(int userId, int roleId, string roleName) {
            _repo.AddUserToRole(userId, roleId, roleName);
        }
		public UserModel GetByUsername(string username) {
			return _repo.GetByUsername(username);
		}

		public UserModel GetById(int id) {
			return _repo.GetById(id);
		}

		public List<UserModel> GetUsersByFilter(UserFilterModel filter) {
			return _repo.GetUsersByFilter(filter);
		}
		public UserModel CreateUser(UserModel model, string password) {
			var newUser = _repo.Create(model);
			return model;
		}

		public UserModel UpdateUser(UserModel model) {
			return _repo.Update(model, false);
		}

		public UserModel GetCurrentUser() {
			return _repo.GetByUsername(_authenticate.CurrentUserName());
		}

		//void IDisposable.Dispose() {
		//	_repo.Dispose();
		//}


		public bool UserExists(string username) {
			return _authenticate.UserExists(username);
		}

		public void SendPasswordReset(string username) {

			//string siteDomain = _settings.GetValue<string>(Setting.AdminWebsiteDomain);
			//string subject = _settings.GetValue<string>(Setting.Email_PasswordReset_Subject);
			//string from = _settings.GetValue<string>(Setting.Email_PasswordReset_FromAddress);

			//string resetToken = _authenticate.GeneratePasswordResetToken(username);

			//var user = GetByUsername(username);

			//string[] to = { user.Email };
			

			//Dictionary<string, string> replacements = new Dictionary<string, string>();
			//replacements.Add("username", username);
			//replacements.Add("domain", siteDomain);
			//replacements.Add("resetToken", resetToken);

			//string textTemplate = _emailResource.GetEmailResource(EmailResources.PasswordResetLink);
			//string htmlTemplate = _emailResource.GetEmailResource(EmailResources.PasswordResetLink, resourceExtension: ".html");
			//string containerTxt = _emailResource.GetEmailResource(EmailResources.Container.ToString() + "Txt", "cshtml");
			//string containerHtml = _emailResource.GetEmailResource(EmailResources.Container.ToString() + "Html", "cshtml");
			//_notify.SendMail(to, from, subject, replacements, textTemplate, htmlTemplate, containerHtml, containerTxt);

		}

		public string GetPasswordResetToken(string username) {
			string resetToken = _authenticate.GeneratePasswordResetToken(username);
			return resetToken;
		}

		public bool ChangePassword(string username, string currentPassword, string newPassword) {
			return _authenticate.ChangePassword(username, currentPassword, newPassword);
		}

        public bool ChangeUsername(string oldUsername, string newUsername) {
            if (_authenticate.UserExists(newUsername)) {
                return false;
            } else {
                UserModel model = _repo.GetByUsername(oldUsername);
                model.Email = newUsername;
                model = _repo.Update(model, true);
                return (model.Email == newUsername);
            }
        }

		public ListOf<UserModel> GetAll(ListPaginationModel pagination) {
			//return _repo.Get(new UserFilterModel(), pagination).ToList();

            ListOf<UserModel> res = new ListOf<UserModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(new UserFilterModel(), pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(new UserFilterModel());
                res.Items = _repo.Get(new UserFilterModel(), pagination);
            }
            return res;
        }
        public ListOf<UserModel> Get(UserFilterModel filter, ListPaginationModel pagination) {
            ListOf<UserModel> res = new ListOf<UserModel>();
            res.Pagination = pagination;
            res.Items = _repo.Get(filter, pagination);
            if (pagination != null) {
                res.Pagination.TotalItems = _repo.GetCount(filter);
            }
            return res;
        }

        public void UpdateLoyaltyPeriod(int id, DateTime startDate, DateTime endDate) {
            _repo.UpdateLoyaltyPeriod(id, startDate, endDate);
        }

        public void EnrollLoyalty(int id) {
            var user = _repo.GetById(id);
            user.IsLoyaltyMember = true;
            user.LoyaltyPeriodStart = DateTime.Today;
            user.LoyaltyPeriodEnd = DateTime.Today.AddMonths(3);
            user.LoyaltySignUpDate = DateTime.Now;
            _repo.Update(user, updateUsername: false);

        }

        public void UpdateLoyaltyTier(int id, int loyaltyTierId) {
            _repo.UpdateLoyaltyTier(id, loyaltyTierId);
        }

        public void SetCorporate(int userId, int? corporateId) {
            _repo.SetCorporate(userId, corporateId);
        }

        public void SetAccountDisabled(int userId, bool disabled) {
            _repo.SetAccountDisabled(userId, disabled);
        }
        public void SetEmailUsername(int userId, string email) {
            _repo.SetUsernameEmail(userId, email);
        }

        public void SetLoyaltyTierNoDropTier(int id, bool isNoDropTier) {
            _repo.SetLoyaltyTierNoDropTier(id, isNoDropTier);
        }
    }
}
