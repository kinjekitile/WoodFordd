using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class LoyaltyOverviewModel {
        public int BookingPerLoyaltyPeriod { get; set; }
        public decimal PointsEarnedPerPeriod { get; set; }
        public decimal TotalPointsEarned { get; set; }

        public decimal TotalPointsSpent { get; set; }
        public decimal PointsRemaining {
            get
            {
                return TotalPointsEarned - TotalPointsSpent;
            }
        }

        public List<BookingHistoryModel> HistoryItemsForPeriod { get; set; }
        public LoyaltyOverviewModel() {
            HistoryItemsForPeriod = new List<BookingHistoryModel>();
        }
    }
}
