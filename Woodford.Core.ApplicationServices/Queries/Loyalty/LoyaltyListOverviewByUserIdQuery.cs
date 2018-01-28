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
    public class LoyaltyListOverviewByUserIdQuery : IQuery<LoyaltyOverviewModel> {
        public int UserId { get; set; }

    }

    public class LoyaltyListOverviewByUserIdQueryHandler : IQueryHandler<LoyaltyListOverviewByUserIdQuery, LoyaltyOverviewModel> {
        private readonly ILoyaltyService _loyaltyService;
        private readonly IUserService _userService;
        private readonly IBookingHistoryService _bookingHistoryService;
        private readonly IReservationService _reservationService;
        public LoyaltyListOverviewByUserIdQueryHandler(ILoyaltyService loyaltyService, IUserService userService, IBookingHistoryService bookingHistoryService, IReservationService reservationService) {
            _loyaltyService = loyaltyService;
            _userService = userService;
            _bookingHistoryService = bookingHistoryService;
            _reservationService = reservationService;
        }
        public LoyaltyOverviewModel Process(LoyaltyListOverviewByUserIdQuery query) {

            

      


            LoyaltyOverviewModel model = new LoyaltyOverviewModel();

            model = _loyaltyService.GetOverviewForUserId(query.UserId);

            return model;
        }
    }
}
