using FluentValidation.Attributes;
using System.Collections.Generic;
using System.Web;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;
namespace Woodford.UI.Web.Admin.ViewModels {

    public class ReservationViewModel {
        public ReservationModel Reservation { get; set; }
    }

    [Validator(typeof(ReservationValidation))]
    public class ReservationSearchViewModel {
        public ReservationFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
        public List<ReservationModel> Items { get; set; }

        public ReportModel Report { get; set; }

        public List<ReservationModel> FailedPaymentResponseItems { get; set; }
    }

    public class ReservationSearchUsersViewModel {
        public UserReservationFilterModel Filter { get; set; }

        public ListPaginationModel Pagination { get; set; }

        public List<UserReservationsModel> Items { get; set; }

        public ReportModel Report { get; set; }
    }
}