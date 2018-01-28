using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RateFindDatesQuery : IQuery<RateFindResultsModel> {
        public List<int> BranchIds { get; set; }
        public int RateCodeId { get; set; }
    }

    public class RateFindDatesQueryHandler : IQueryHandler<RateFindDatesQuery, RateFindResultsModel> {
        private readonly IRateService _rateService;
        private readonly IBranchService _branchService;
        private readonly IVehicleGroupService _vehicleGroupService;
        public RateFindDatesQueryHandler(IRateService rateService, IBranchService branchService, IVehicleGroupService vehicleGroupService) {
            _rateService = rateService;
            _branchService = branchService;
            _vehicleGroupService = vehicleGroupService;
        }
        public RateFindResultsModel Process(RateFindDatesQuery query) {

            var rates = _rateService.Get(new RateFilterModel { BranchIds = query.BranchIds, RateCodeId = query.RateCodeId, IsDeleted = false }, null);

            RateFindResultsModel results = new RateFindResultsModel();
            if (rates != null) {
                foreach (var r in rates.Items) {
                    results.AddResult(new RateFindResultModel { IsOpenEnded = r.IsOpenEnded, StartDate = r.ValidStartDate, EndDate = r.ValidEndDate });
                }
            }
            return results;
        }
    }
}
