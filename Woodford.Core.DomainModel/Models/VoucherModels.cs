using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class VoucherModel {
        public int Id { get; set; }
        public bool RequireClientValidation { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime DateExpiry { get; set; }
        public DateTime? DateRedeemed { get; set; }
        public int? ReservationId { get; set; }
        public string VoucherNumber { get; set; }
        public VoucherRewardType VoucherRewardType { get; set; }
        public decimal? VoucherDiscount { get; set; }
        public decimal? VoucherDiscountPercentage { get; set; }
        public string VoucherReward { get; set; }
        public bool IsMultiUse { get; set; }
    }

    public class VoucherFilterModel {
        public string VoucherNumber { get; set; }
        public bool? IsRedeemed { get; set; }
        public bool? IsExpired { get; set; }
        public bool? IsMultiUse { get; set; }
        public VoucherRewardType? VoucherRewardType { get; set; }
    }

    public class VoucherResponseModel {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class VoucherRedeemModel {
        public VoucherResponseModel Response { get; set; }
        public VoucherModel Voucher { get; set; }
        public VoucherRedeemModel() {
            Response = new VoucherResponseModel();
        }
    }
}
