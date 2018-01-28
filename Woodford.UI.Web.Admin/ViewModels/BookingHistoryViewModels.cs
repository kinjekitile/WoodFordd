using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    public class BookingHistoryUploadViewModel {

        public HttpPostedFileBase Upload { get; set; }
    }
    [Validator(typeof(BookingHistoryValidator))]
    public class AddBookingHistoryViewModel {
        public BookingHistoryModel BookingHistory { get; set; }
        public int? ClaimBookingId { get; set; }
    }
}