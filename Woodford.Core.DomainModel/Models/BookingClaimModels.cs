using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {

    public class BookingClaimFilterModel {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public BookingClaimState? State { get; set; }

    }
    public class BookingClaimModel {
        public int Id { get; set; }
        public int UserId { get; set; }
        public BookingClaimState State { get; set; }
        [Required]
        public DateTime BookingPickupDate { get; set; }
        [Required]
        public DateTime BookingDropoffDate { get; set; }
        [Required]
        public int BookingPickupBranchId { get; set; }
        public BranchModel PickupBranch { get; set; }
        public int BookingDropofBranchId { get; set; }
        public BranchModel DropoffBranch { get; set; }
        [Required]
        public string Email { get; set; }
        public string IdNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string ExternalId { get; set; }
        public decimal TotalBill { get; set; }
        public decimal TotalForLoyaltyAward { get; set; }

    }
}
