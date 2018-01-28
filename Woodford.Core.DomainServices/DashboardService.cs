using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Repositories;
using Woodford.Core.Interfaces.Services;

namespace Woodford.Core.DomainServices {
    public class DashboardService : IDashboardService  {

        private readonly IDashboardRepository _repo;
        public DashboardService(IDashboardRepository repo) {
            _repo = repo;
        }

        public DashboardSummaryModel GetSummary(DateTime startDate, DateTime endDate) {
            DashboardSummaryModel model = new DashboardSummaryModel();

            model = _repo.GetSummary(startDate, endDate);



            return model;
        }
    }
}
