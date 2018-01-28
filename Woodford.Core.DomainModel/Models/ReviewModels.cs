using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class ReviewModel {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        public int? VoucherId { get; set; }
        public bool VoucherSent { get; set; }
        public DateTime? VoucherSentDate { get; set; }
        public string Email { get; set; }
        public ReservationModel Reservation { get; set; }
    }

    public class ReviewFilterModel {
        public int? Id { get; set; }
        public int? ReservationId { get; set; }
        public int? VoucherId { get; set; }
        public bool? VoucherSent { get; set; }
        public DateTime? VoucherSentDate { get; set; }
        public string Email { get; set; }

        public bool? IsCompleted { get; set; }
    }

    public class ReviewLinkRequestModel {
        public int ReservationId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ReviewLinkResponseModel {
        public string Id { get; set; }
        public string ReviewUrl { get; set; }
    }
}
