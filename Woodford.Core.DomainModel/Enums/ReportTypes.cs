using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Enums {
    public enum ReportType {
        User = 1,
        Reservation = 2,
        UserActivity = 3
    }

    public enum ReportDateUnitType {
        Day = 1,
        Month = 2,
        Year = 3
    }
}
