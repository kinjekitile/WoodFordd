using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {

    public class UserReservationFilterModel {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? BranchId { get; set; }
        public int? VehicleGroupId { get; set; }
        public bool? IsMobileBooking { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }

    }

    public class UserFilterModel {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? LoyaltyPeriodEndDate { get; set; }

        public int? CorporateId { get; set; }

        public LoyaltyTierLevel? LoyaltyLevel { get; set; }
        

        public DateTime? DateCreatedStart { get; set; }
        public DateTime? DateCreatedEnd { get; set; }

        public UserDateFilterTypes DateFilterType { get; set; }

        public bool? IsLoyaltyMember { get; set; }

        public UserSortField SortField { get; set; }

        public UserSortDirection SortDirection { get; set; }

        public bool? HasSpentPoints { get; set; }
    }

    public class UserReservationsModel : UserModel {
        public int NumberOfBookings { get; set; }
    }
    public class UserModel {
        public int Id { get; set; }
        //public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string MobileNumber { get; set; }
        public string IdNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public int LoyaltyTierId { get; set; }
        public bool IsLoyaltyTierLocked { get; set; }
        public bool IsAccountDisabled { get; set; }
        public LoyaltyTierLevel LoyaltyTier { get { return (LoyaltyTierLevel)LoyaltyTierId; } }
        public DateTime? LoyaltyPeriodStart { get; set; }
        public DateTime? LoyaltyPeriodEnd { get; set; }
        public bool IsLoyaltyMember { get; set; }
        public DateTime? LoyaltySignUpDate { get; set; }
        public int? CorporateId { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
        public CorporateModel Corporate { get; set; }
        public LoyaltyOverviewModel LoyaltyOverview { get; set; }
        public bool HasExistingLoyaltyNumber { get; set; }
        public string ExistingLoyaltyNumber { get; set; }
        public string LoyaltyNumberFull {
            get {
                if (HasExistingLoyaltyNumber) {
                    return ExistingLoyaltyNumber;
                }
                return "FRP" + (Id + 953).ToString();
            }
        }

        public decimal? LoyaltyPointsEarned { get; set; }

        public decimal? LoyaltyPointsSpent { get; set; }

        public decimal LoyaltyPointsRemaining {

            get {
                decimal earned = 0m;
                if (LoyaltyPointsEarned.HasValue) {
                    earned = LoyaltyPointsEarned.Value;
                }
                decimal spent = 0m;
                if (LoyaltyPointsSpent.HasValue) {
                    spent = LoyaltyPointsSpent.Value;
                }
                return earned - spent;
            }
        }

        public UserModel() {
            Roles = new List<RoleModel>();
        }

    }
}
