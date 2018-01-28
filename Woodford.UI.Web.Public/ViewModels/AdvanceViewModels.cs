using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;

namespace Woodford.UI.Web.Public.ViewModels {
    public class AdvanceViewModel {
        public UserModel User { get; set; }
        public LoyaltyOverviewModel LoyaltyOverview { get; set; }
        public int LoyaltyBookingsPerPeriod { get; set; }
        public string LoyaltyTier { get; set; }
        public int BookingsRequiredForNextLoyaltyTier { get; set; }
        public int BookingsRequiredForCurrentLoyaltyTier { get; set; }
        public decimal LoyaltyTierPointsPercentage { get; set; }
        public int BookingsToRemainOnCurrentTier
        {
            get
            {
                int diff = BookingsRequiredForCurrentLoyaltyTier - LoyaltyBookingsPerPeriod;
                if (diff < 0) {
                    diff = 0;
                }
                return diff;
            }
        }
        public int BookingsToMoveUpTier
        {
            get
            {
                int diff = BookingsRequiredForNextLoyaltyTier - LoyaltyBookingsPerPeriod;
                if (diff < 0) {
                    diff = 0;
                }
                return diff;
            }
        }
        public bool IsFinalTier { get; set; }
        public List<ReservationModel> Reservations { get; set; }


    }
}