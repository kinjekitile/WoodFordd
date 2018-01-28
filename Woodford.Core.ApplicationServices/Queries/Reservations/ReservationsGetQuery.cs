using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class ReservationsGetQuery : IQuery<ListOf<ReservationModel>> {
        public ReservationFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class ReservationsGetQueryHandler : IQueryHandler<ReservationsGetQuery, ListOf<ReservationModel>> {
        private readonly IReservationService _reservationService;
        public ReservationsGetQueryHandler(IReservationService reservationService) {
            _reservationService = reservationService;
        }
        public ListOf<ReservationModel> Process(ReservationsGetQuery query) {
            return _reservationService.Get(query.Filter, query.Pagination);
        }
    }
}
