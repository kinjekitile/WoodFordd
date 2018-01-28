using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Queries {
    public class BookingHistoryByExternalIdQuery : IQuery<BookingHistoryModel> {
        public string ExternalId { get; set; }
    }

    public class BookingHistoryByExternalIdQueryHandler : IQueryHandler<BookingHistoryByExternalIdQuery, BookingHistoryModel> {
        private readonly IBookingHistoryRepository _bookingHistoryRepo;
        public BookingHistoryByExternalIdQueryHandler(IBookingHistoryRepository bookingHistoryRepo) {
            _bookingHistoryRepo = bookingHistoryRepo;
        }
        public BookingHistoryModel Process(BookingHistoryByExternalIdQuery query) {
            return _bookingHistoryRepo.GetByExternalId(query.ExternalId);
        }

    }
}
