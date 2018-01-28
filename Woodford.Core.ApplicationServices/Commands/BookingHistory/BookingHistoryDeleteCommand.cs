using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class BookingHistoryDeleteCommand : ICommand {
        public int BookingHistoryId { get; set; }
    }

    public class BookingHistoryDeleteCommandHandler : ICommandHandler<BookingHistoryDeleteCommand> {
        private readonly IBookingHistoryService _bookingHistoryService;
        public BookingHistoryDeleteCommandHandler(IBookingHistoryService bookingHistoryService) {
            _bookingHistoryService = bookingHistoryService;
        }

        public void Handle(BookingHistoryDeleteCommand command) {
            _bookingHistoryService.Delete(command.BookingHistoryId);
        }
    }
}
