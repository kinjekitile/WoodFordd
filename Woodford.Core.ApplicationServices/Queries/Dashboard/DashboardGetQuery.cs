using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Queries {
    public class DashboardGetQuery : IQuery<DashboardModel> {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


    }

    public class DashboardGetQueryHandler : IQueryHandler<DashboardGetQuery, DashboardModel> {

        private readonly IBranchService _branchService;
        private readonly IReservationService _reservationService;

        public DashboardGetQueryHandler(IBranchService branchService, IReservationService reservationService) {
            _branchService = branchService;
            _reservationService = reservationService;
        }

        public DashboardModel Process(DashboardGetQuery query) {
            //Refactor into separate queries
            throw new NotImplementedException();

            DashboardModel model = new DashboardModel();

            ReservationFilterModel reservationFilter = new ReservationFilterModel();
            reservationFilter.DateFilterType = DomainModel.Enums.ReservationDateFilterTypes.BookingDate;
            reservationFilter.DateSearchStart = query.StartDate;
            reservationFilter.DateSearchEnd = query.EndDate;
            reservationFilter.ReservationState = DomainModel.Enums.ReservationState.Completed;

            
            //model.TotalBookings = _reservationService.Get(reservationFilter, null).Items.Count;

            //reservationFilter.ReservationState = DomainModel.Enums.ReservationState.Started;

            //model.TotalQuotes = _reservationService.Get(reservationFilter, null).Items.Count;

            //reservationFilter.ReservationState = DomainModel.Enums.ReservationState.Completed;
            //reservationFilter.DateFilterType = DomainModel.Enums.ReservationDateFilterTypes.PickupDate;

            //model.TotalPickups = _reservationService.Get(reservationFilter, null).Items.Count();

            //reservationFilter.ReservationState = DomainModel.Enums.ReservationState.Completed;
            //reservationFilter.DateFilterType = DomainModel.Enums.ReservationDateFilterTypes.BookingDate;
            //reservationFilter.DateSearchStart = DateTime.Today.AddDays(-3);
            //reservationFilter.DateSearchEnd = DateTime.Today;

            //model.RecentBookings = _reservationService.Get(reservationFilter, null).Items;


            var branches = _branchService.Get(new BranchFilterModel { IsArchived = false }, null).Items;

            foreach (var branch in branches) {

                reservationFilter.ReservationState = DomainModel.Enums.ReservationState.Completed;
                reservationFilter.DateSearchStart = query.StartDate;
                reservationFilter.DateSearchEnd = query.EndDate;
                reservationFilter.PickupBranchId = branch.Id;

                branch.TotalPickups = _reservationService.Get(reservationFilter, null).Items.Count();
                //model.Branches.Add(branch);
            }

            return model;
        }
    }
}
