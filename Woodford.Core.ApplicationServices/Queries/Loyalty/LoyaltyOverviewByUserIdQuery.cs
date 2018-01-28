using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class LoyaltyOverviewByUserIdQuery : IQuery<LoyaltyOverviewModel> {
        public int UserId { get; set; }

    }

    public class LoyaltyOverviewByUserIdQueryHandler : IQueryHandler<LoyaltyOverviewByUserIdQuery, LoyaltyOverviewModel> {
        private readonly ILoyaltyService _loyaltyService;
        private readonly IUserService _userService;
        private readonly IBookingHistoryService _bookingHistoryService;
        private readonly IReservationService _reservationService;
        public LoyaltyOverviewByUserIdQueryHandler(ILoyaltyService loyaltyService, IUserService userService, IBookingHistoryService bookingHistoryService, IReservationService reservationService) {
            _loyaltyService = loyaltyService;
            _userService = userService;
            _bookingHistoryService = bookingHistoryService;
            _reservationService = reservationService;
        }
        public LoyaltyOverviewModel Process(LoyaltyOverviewByUserIdQuery query) {



            UserModel user = _userService.GetById(query.UserId);
           // var reservations = _reservationService.Get(new ReservationFilterModel { UserId = user.Id, IsCompletedInvoice = true }, null).Items;

            var bookings = _bookingHistoryService.Get(new BookingHistoryFilterModel { UserId = user.Id }, null).Items;
            bookings = bookings.Where(x => x.PickupDate >= user.LoyaltySignUpDate).ToList();
            var bookingsPerPeriod = bookings.Where(x => x.PickupDate >= user.LoyaltyPeriodStart.Value && x.PickupDate <= user.LoyaltyPeriodEnd).ToList();




            LoyaltyOverviewModel model = new LoyaltyOverviewModel();
            model.HistoryItemsForPeriod = bookingsPerPeriod;

            model.BookingPerLoyaltyPeriod = bookingsPerPeriod.Count();

            decimal? pointsEarnedPerPeriod = bookingsPerPeriod.Sum(x => x.LoyaltyPointsEarned);
            if (pointsEarnedPerPeriod.HasValue) {
                model.PointsEarnedPerPeriod = pointsEarnedPerPeriod.Value;
            }
            decimal? totalPointsEarned = user.LoyaltyPointsEarned;
            if (totalPointsEarned.HasValue) {
                model.TotalPointsEarned = totalPointsEarned.Value;
            }

            decimal? totalPointsSpent = user.LoyaltyPointsSpent;
            if (totalPointsSpent.HasValue) {
                model.TotalPointsSpent = totalPointsSpent.Value;
            }


            return model;
        }
    }
}
