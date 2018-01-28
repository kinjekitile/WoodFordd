using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.UI.Web.Public.ViewModels {
    public class BookingHistoryViewModel {
        public ListOf<BookingHistoryModel> Result { get; set; }
        public bool ShowLoyaltyPointsEarned { get; set; }
    }
}