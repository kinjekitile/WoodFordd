using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class BookingHistoryAddCommand : ICommand {
        public BookingHistoryModel BookingHistory { get; set; }
    }

    public class BookingHistoryAddCommandHandler : ICommandHandler<BookingHistoryAddCommand> {
        private readonly IBookingHistoryService _bookingHistoryService;
        private readonly IUserService _userService;
        private readonly ILoyaltyService _loyaltyService;

        public BookingHistoryAddCommandHandler(IBookingHistoryService bookingHistoryService, IUserService userService, ILoyaltyService loyaltyService) {
            _bookingHistoryService = bookingHistoryService;
            _userService = userService;
            _loyaltyService = loyaltyService;
        }

        public void Handle(BookingHistoryAddCommand command) {
            //Calculate points if user is loyalty
            var loyaltyTiers = _loyaltyService.GetAll().ToList();
            var user = _userService.GetById(command.BookingHistory.UserId.Value);

            if (user.IsLoyaltyMember) {
                var tier = loyaltyTiers.Single(x => x.TierLevel == (LoyaltyTierLevel)user.LoyaltyTierId);
                decimal pointsEarned = decimal.Round(Convert.ToDecimal(command.BookingHistory.TotalForLoyaltyAward) * tier.PointsEarnedPerRandSpent, 2);
                command.BookingHistory.LoyaltyPointsEarned = pointsEarned;
                command.BookingHistory.SendLoyaltyPointsEarnedEmail = true;
            } else {
                command.BookingHistory.LoyaltyPointsEarned = 0m;
                command.BookingHistory.SendLoyaltyPointsEarnedEmail = false;
            }

            

            command.BookingHistory = _bookingHistoryService.Create(command.BookingHistory);
        }
    }
}
