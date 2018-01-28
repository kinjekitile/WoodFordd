using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ClaimBookingConfirmCommand : ICommand {
        public int UserId { get; set; }
        public int BookingClaimId { get; set; }
        public string ExternalBookingId { get; set; }
        public BookingClaimState OUTClaimState { get; set; }
    }

    public class ClaimBookingConfirmCommandHandler : ICommandHandler<ClaimBookingConfirmCommand> {
        private readonly IClaimBookingRepository _claimBookingRepo;
        private readonly IExternalSystemService _externalBooking;
        private readonly IBookingHistoryRepository _bookingHistoryRepo;
        public ClaimBookingConfirmCommandHandler(IClaimBookingRepository claimBookingRepo, IExternalSystemService externalBooking, IBookingHistoryRepository bookingHistoryRepo) {
            _claimBookingRepo = claimBookingRepo;
            _externalBooking = externalBooking;
            _bookingHistoryRepo = bookingHistoryRepo;
        }
        public void Handle(ClaimBookingConfirmCommand command) {
            command.OUTClaimState = BookingClaimState.Claimed;

            //Check if booking already claimed
            BookingHistoryModel historyItem = _bookingHistoryRepo.GetByExternalId(command.ExternalBookingId);
            if (historyItem != null) {
                _claimBookingRepo.SetState(command.BookingClaimId, BookingClaimState.DeniedBookingAlreadyClaimed);
                command.OUTClaimState = BookingClaimState.DeniedBookingAlreadyClaimed;
                return;
            }

            //Get the external Booking
            historyItem = _externalBooking.GetBookingByExternalId(command.ExternalBookingId);
            if (historyItem == null) {
                _claimBookingRepo.SetState(command.BookingClaimId, BookingClaimState.NotFound);
                command.OUTClaimState = BookingClaimState.NotFound;
                return;
            }

            //Add the booking history item to the user
            historyItem.UserId = command.UserId;
            _bookingHistoryRepo.Create(historyItem);
            _claimBookingRepo.SetState(command.BookingClaimId, BookingClaimState.Claimed);
            command.OUTClaimState = BookingClaimState.Claimed;
            return;
        }
    }
}
