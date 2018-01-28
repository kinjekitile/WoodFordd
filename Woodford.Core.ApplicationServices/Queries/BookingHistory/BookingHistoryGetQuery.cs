using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BookingHistoryGetQuery : IQuery<ListOf<BookingHistoryModel>> {
        public BookingHistoryFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class BookingHistoryGetQueryHandler : IQueryHandler<BookingHistoryGetQuery, ListOf<BookingHistoryModel>> {
        private readonly IBookingHistoryService _bookingHistoryService;

        public BookingHistoryGetQueryHandler(IBookingHistoryService bookingHistoryService) {
            _bookingHistoryService = bookingHistoryService;
        }
        public ListOf<BookingHistoryModel> Process(BookingHistoryGetQuery query) {
            return _bookingHistoryService.Get(query.Filter, query.Pagination);
        }
    }
}
