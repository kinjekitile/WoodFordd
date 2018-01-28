using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces.Repositories {
    public interface IDashboardRepository {
        DashboardSummaryModel GetSummary(DateTime startDate, DateTime endDate);
    }
}
