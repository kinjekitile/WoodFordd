using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class UserRepository : RepositoryBase, IUserRepository {
        private const string UserNotFound = "User could not be found";

        public UserRepository(IDbConnectionConfig connection) : base(connection) { }


        public void AddUserToRole(int userId, int roleId, string roleName) {
            UserProfile p = _db.UserProfiles.SingleOrDefault(x => x.Id == userId);
            if (p != null) {
                webpages_Roles r = _db.webpages_Roles.SingleOrDefault(x => x.RoleId == roleId);
                if (r != null) {
                    r.UserProfiles.Add(p);

                    _db.SaveChanges();
                }
            }
        }

        public void UpdateLoyaltyTier(int id, int loyaltyTierId) {
            UserProfile p = GetBy(x => x.Id == id);
            p.LoyaltyTierId = loyaltyTierId;
            _db.SaveChanges();

        }


        public void SetLoyaltyTierNoDropTier(int id, bool isNoDropTier) {
            UserProfile p = GetBy(x => x.Id == id);
            p.IsLoyaltyTierLocked = isNoDropTier;
            _db.SaveChanges();
        }
        public void SetCorporate(int userId, int? corporateId) {
            UserProfile p = GetBy(x => x.Id == userId);
            p.CorporateId = corporateId;
            _db.SaveChanges();
        }

        public void SetAccountDisabled(int userId, bool disabled) {
            UserProfile p = GetBy(x => x.Id == userId);
            p.IsAccountDisabled = disabled;
            _db.SaveChanges();
        }

        public void SetUsernameEmail(int userId, string email) {
            UserProfile p = GetBy(x => x.Id == userId);
            p.Email = email;
            _db.SaveChanges();
        }

        public void UpdateLoyaltyPeriod(int id, DateTime startDate, DateTime endDate) {
            UserProfile p = GetBy(x => x.Id == id);
            p.LoyaltyPeriodStart = startDate;
            p.LoyaltyPeriodEnd = endDate;
            _db.SaveChanges();
        }

        public UserModel Create(UserModel model) {

            UserProfile u = new UserProfile();


            u.Email = model.Email;
            u.FirstName = model.FirstName;
            u.LastName = model.LastName;
            u.MobileNumber = model.MobileNumber;
            u.IdNumber = model.IdNumber;
            u.DateCreated = model.DateCreated;
            u.CorporateId = model.CorporateId;
            u.IsLoyaltyMember = model.IsLoyaltyMember;
            u.LoyaltySignUpDate = model.LoyaltySignUpDate;
            _db.UserProfiles.Add(u);


            _db.SaveChanges();

            model.Id = u.Id;

            return model;
        }


        public UserModel Update(UserModel model, bool updateUsername) {

            UserProfile p = GetBy(x => x.Id == model.Id);
            if (updateUsername) {
                p.Email = model.Email;
            }
            p.FirstName = model.FirstName;
            p.LastName = model.LastName;
            p.MobileNumber = model.MobileNumber;
            p.IdNumber = model.IdNumber;
            p.CorporateId = model.CorporateId;
            p.IsLoyaltyMember = model.IsLoyaltyMember;
            p.LoyaltySignUpDate = model.LoyaltySignUpDate;
            p.LoyaltyPeriodStart = model.LoyaltyPeriodStart;
            p.LoyaltyPeriodEnd = model.LoyaltyPeriodEnd;
            p.HasExistingLoyaltyNumber = model.HasExistingLoyaltyNumber;
            p.ExistingLoyaltyNumber = model.ExistingLoyaltyNumber;
            _db.SaveChanges();
            return model;

        }

        public UserModel GetById(int id) {
            UserModel user = GetAsIQueryable().SingleOrDefault(x => x.Id == id);
            return user;
        }

        public UserModel GetByUsername(string username) {
            UserModel user = GetAsIQueryable().SingleOrDefault(x => x.Email == username);
            return user;
        }

        private IQueryable<UserModel> GetAsIQueryable() {
            return _db.UserProfiles.Select(x => new UserModel {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MobileNumber = x.MobileNumber,
                IdNumber = x.IdNumber,
                DateCreated = x.DateCreated,
                LoyaltyTierId = x.LoyaltyTierId,
                
                CorporateId = x.CorporateId,
                LoyaltyPeriodStart = x.LoyaltyPeriodStart,
                LoyaltyPeriodEnd = x.LoyaltyPeriodEnd,
                LoyaltySignUpDate = x.LoyaltySignUpDate,
                IsLoyaltyMember = x.IsLoyaltyMember,
                IsLoyaltyTierLocked = x.IsLoyaltyTierLocked,
                HasExistingLoyaltyNumber = x.HasExistingLoyaltyNumber,
                ExistingLoyaltyNumber = x.ExistingLoyaltyNumber,
                IsAccountDisabled = x.IsAccountDisabled,
                Roles = x.webpages_Roles.Select(y => new RoleModel {
                    Id = y.RoleId,
                    Title = y.RoleName
                }),
                Corporate = x.CorporateId.HasValue ? new CorporateModel {
                    Id = x.Corporate.Id,
                    Title = x.Corporate.Title
                } : null,
                LoyaltyPointsEarned = _db.BookingHistories.Where(y => y.UserId == x.Id && y.LoyaltyPointsEarned.HasValue).Select(y => y.LoyaltyPointsEarned).DefaultIfEmpty().Sum(),
                LoyaltyPointsSpent = _db.ReservationsAdmins
                .Where(z => !z.IsQuote)
                .Where(z => z.UserId.HasValue)
                .Where(z => z.UserId.Value == x.Id)
                .Where(z => z.LoyaltyPointsSpent.HasValue)
                .Where(z => z.InvoiceId.HasValue)
                .Where(z => z.IsMobileCheckout.HasValue)
                .Where(z => z.IsCompleted.HasValue)
                .Where(z => z.MyGateTransactionID.HasValue || (z.IsMobileCheckout.Value && z.IsCompleted.Value))
                .Select(z => z.LoyaltyPointsSpent)
                .DefaultIfEmpty()
                .Sum()
            });
        }

        public List<UserModel> Get(UserFilterModel filter, ListPaginationModel pagination) {

            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            if (filter.SortField != UserSortField.None) {
                switch (filter.SortField) {
                    case UserSortField.Email:
                        if (filter.SortDirection == UserSortDirection.Ascending) {
                            list = list.OrderBy(x => x.Email);
                        }
                        else {
                            list = list.OrderByDescending(x => x.Email);
                        }

                        break;
                    case UserSortField.Name:
                        if (filter.SortDirection == UserSortDirection.Ascending) {
                            list = list.OrderBy(x => x.LastName);
                        }
                        else {
                            list = list.OrderByDescending(x => x.LastName);
                        }

                        break;
                    case UserSortField.FRPNumber:
                        if (filter.SortDirection == UserSortDirection.Ascending) {
                            list = list.OrderBy(x => x.LoyaltyNumberFull);
                        }
                        else {
                            list = list.OrderByDescending(x => x.LoyaltyNumberFull);
                        }

                        break;
                    case UserSortField.LoyaltyPoints:
                        if (filter.SortDirection == UserSortDirection.Ascending) {
                            list = list.OrderBy(x => x.LoyaltyPointsEarned);
                        }
                        else {
                            list = list.OrderByDescending(x => x.LoyaltyPointsEarned);
                        }

                        break;
                }
            }
            else {
                list = list.OrderBy(x => x.Id);
            }
            if (pagination == null) {
                return list.ToList();
            }
            else {
                return list.Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public int GetCount(UserFilterModel filter) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }


        private IQueryable<UserModel> applyFilter(IQueryable<UserModel> list, UserFilterModel filter) {
            if (filter != null) {
                if (filter.Id.HasValue) { list = list.Where(x => x.Id == filter.Id.Value); }
                if (!string.IsNullOrEmpty(filter.Email)) { list = list.Where(x => x.Email.Contains(filter.Email)); }
                if (!string.IsNullOrEmpty(filter.FirstName)) { list = list.Where(x => x.FirstName.Contains(filter.FirstName)); }
                if (!string.IsNullOrEmpty(filter.LastName)) {
                    list = list.Where(x => x.LastName.Contains(filter.LastName));
                }
                if (filter.DateFilterType == UserDateFilterTypes.CreatedDate) {
                    if (filter.DateCreatedStart.HasValue && filter.DateCreatedEnd.HasValue)
                        list = list.Where(x => x.DateCreated >= filter.DateCreatedStart.Value && x.DateCreated < filter.DateCreatedEnd.Value);
                }

                if (filter.LoyaltyPeriodEndDate.HasValue)
                    list = list.Where(x => x.LoyaltyPeriodEnd == filter.LoyaltyPeriodEndDate.Value);

                if (filter.CorporateId.HasValue)
                    list = list.Where(x => x.CorporateId == filter.CorporateId.Value);
                if (filter.IsLoyaltyMember.HasValue)
                    list = list.Where(x => x.IsLoyaltyMember == filter.IsLoyaltyMember.Value);
                if (filter.LoyaltyLevel.HasValue) {
                    if (filter.LoyaltyLevel.Value == LoyaltyTierLevel.All) {
                        list = list.Where(x => x.IsLoyaltyMember && (x.LoyaltyTierId == (int)LoyaltyTierLevel.Green || x.LoyaltyTierId == (int)LoyaltyTierLevel.Silver || x.LoyaltyTierId == (int)LoyaltyTierLevel.Gold));
                    }
                    else {
                        if (filter.LoyaltyLevel.Value == LoyaltyTierLevel.NotSet) {

                        }
                        else {
                            list = list.Where(x => x.IsLoyaltyMember && x.LoyaltyTierId == (int)filter.LoyaltyLevel.Value);
                        }

                    }

                }
                if (filter.HasSpentPoints.HasValue) {
                    if (filter.HasSpentPoints.Value) {
                        list = list.Where(x => x.LoyaltyPointsSpent > 0);
                    } else {
                        list = list.Where(x => x.LoyaltyPointsSpent == 0);
                    }
                }

            }
            return list;
        }


        private UserProfile GetUserProfileById(int id) {
            UserProfile p = GetBy(x => x.Id == id);

            return p;
        }

        private UserProfile GetBy(Func<UserProfile, bool> condition, bool throwExceptionIfNotExists = false) {
            UserProfile p = _db.UserProfiles.SingleOrDefault(condition);
            if (p == null && throwExceptionIfNotExists) {
                throw new Exception(UserNotFound);
            }
            return p;
        }

        //public override void Dispose() {
        //    base.Dispose();
        //}


        public List<UserModel> GetUsersByFilter(UserFilterModel filter) {
            throw new NotImplementedException();
        }
    }
}
