using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries.Reservations {
    public class ReservationDateChangeValidQuery : IQuery<ReservationDateCheckResponseModel> {
        public int ReservationId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DropOffDate { get; set; }
    }

    public class ReservationDateChangeValidQueryHandler {
        private IReservationService _reservationService;
        private IUserService _userService;
        private ISearchService _searchService;


        public ReservationDateChangeValidQueryHandler(IReservationService reservationService, IUserService userService, ISearchService searchService) {
            _reservationService = reservationService;
            _userService = userService;
            _searchService = searchService;
        }

    }
}
