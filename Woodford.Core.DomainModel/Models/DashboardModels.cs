using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class DashboardModel {

        public DashboardSummaryModel Summary { get; set; }
        public DashboardChartModel Chart { get; set; }
        public DashboardBranchesModel BranchBookings { get; set; }

        public DashboardRecentBookingsModel RecentBookings { get; set; }
        public DashboardModel() {
            Summary = new DashboardSummaryModel();
            Chart = new DashboardChartModel();
            RecentBookings = new DashboardRecentBookingsModel();
            BranchBookings = new DashboardBranchesModel();
        }

    }

    public class DashboardBranchesModel {
        public List<BranchModel> Branches { get; set; }
        public DashboardBranchesModel() {
            Branches = new List<BranchModel>();
        }
    }
    public class DashboardRecentBookingsModel {
        public List<ReservationModel> RecentBookings { get; set; }

        public DashboardRecentBookingsModel() {
            RecentBookings = new List<ReservationModel>();
        }
    }
    public class DashboardSummaryModel {

        public int TotalBookings { get; set; }
        public int TotalSignUps { get; set; }
        public int TotalPickups { get; set; }
        public int TotalQuotes { get; set; }

        public DateTime SummaryStartDate { get; set; }
        public DateTime SummaryEndDate { get; set; }
    }


    public class DashboardChartModel {
        public DateTime ChartStartDate { get; set; }

        public DateTime ChartEndDate { get; set; }

        public ChartDataModel ChartData { get; set; }
    }

    public class ChartDataModel {
        public string XAxisLabels { get; set; }

        public string XAxisData { get; set; }

        public string YAxisLabels { get; set; }

        public string YAxisData { get; set; }
    }
}
