using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class BookingHistoryModel {
        public BookingHistoryModel() {

        }

        public BookingHistoryModel(BookingClaimModel claim) {
            PickupBranchId = claim.BookingPickupBranchId;
            DropoffBranchId = claim.BookingDropofBranchId;
            PickupDate = claim.BookingPickupDate;
            DropOffDate = claim.BookingDropoffDate;
            Email = claim.Email;
            AlternateId = claim.IdNumber;
            UserId = claim.UserId;
        }
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string AlternateId { get; set; }
        public int PickupBranchId { get; set; }
        public int DropoffBranchId { get; set; }

        public BranchModel PickupBranch { get; set; }

        public BranchModel DropoffBranch { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DropOffDate { get; set; }

        public int? RentalDays { get; set; }
        public int? KmsDriven { get; set; }
        public int? FreeKms { get; set; }
        public decimal? KmsRate { get; set; }

        public decimal? TotalForLoyaltyAward { get; set; }
        public decimal? LoyaltyPointsEarned { get; set; }
        public decimal? TotalAmount { get; set; }

        public bool LoyaltyPointsEarnedEmailSent { get; set; }
        public bool SendLoyaltyPointsEarnedEmail { get; set; }
    }

    public class BookingHistoryFilterModel {
        public int? UserId { get; set; }
        public string ExternalId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? LoyaltyPointsEarnedEmailSent { get; set; }
        public bool? SendLoyaltyPointsEmail { get; set; }

    }
}