using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Queries {
    public class BookingHistoryByIdQuery : IQuery<BookingHistoryModel> {
        public int Id { get; set; }
    }

    public class BookingHistoryByIdQueryHandler : IQueryHandler<BookingHistoryByIdQuery, BookingHistoryModel> {
        private readonly IBookingHistoryService _bookingHistoryService;
        public BookingHistoryByIdQueryHandler(IBookingHistoryService bookingHistoryService) {
            _bookingHistoryService = bookingHistoryService;
        }
        public BookingHistoryModel Process(BookingHistoryByIdQuery query) {
            return _bookingHistoryService.GetById(query.Id);
        }

    }
}
