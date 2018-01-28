using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Queries {
    public class DashboardGetSummaryQuery : IQuery<DashboardSummaryModel> {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


    }

    public class DashboardGetSummaryQueryHandler : IQueryHandler<DashboardGetSummaryQuery, DashboardSummaryModel> {

        
        private readonly IReservationService _reservationService;

        public DashboardGetSummaryQueryHandler(IReservationService reservationService) {
            _reservationService = reservationService;
        }

        public DashboardSummaryModel Process(DashboardGetSummaryQuery query) {


            DashboardSummaryModel model = new DashboardSummaryModel();

            
            return model;
        }
    }
}
