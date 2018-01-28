using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {

    
    public class ReportSearchViewModel {
        public ReportFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
        public List<ReportModel> Items { get; set; }
    }

    [Validator(typeof(ReservationValidation))]
    public class ReservationReportViewModel {
        public ReservationFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
        public List<ReservationModel> Items { get; set; }

        public ReportModel Report { get; set; }

        public List<ReservationModel> FailedPaymentResponseItems { get; set; }
    }
}