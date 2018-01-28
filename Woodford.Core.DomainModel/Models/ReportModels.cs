using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class ReportModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public ReportType ReportType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool UseCurrentDateAsStartDate { get; set; }
        public int? DateUnitsToAdd { get; set; }
        public ReportDateUnitType? DateUnitType { get; set; }

        public string ReportFilter { get; set; }

        public string ReportFilterUrl { get; set; }
        public ReservationFilterModel ReservationFilter { get; set; }

        public UserFilterModel UserFilter { get; set; }
    }

    public class ReportFilterModel {
        public ReportType? ReportType { get; set; }
        public int? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Title { get; set; }

    }
}
