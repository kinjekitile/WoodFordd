using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RateSearchQuery : IQuery<RateSearchResultModel> {
        public RateSearchModel SearchModel { get; set; }
    }

    public class RateSearchQueryHandler : IQueryHandler<RateSearchQuery, RateSearchResultModel> {
        private readonly IRateService _rateService;
        private readonly IBranchService _branchService;
        private readonly IVehicleGroupService _vehicleGroupService;
        public RateSearchQueryHandler(IRateService rateService, IBranchService branchService, IVehicleGroupService vehicleGroupService) {
            _rateService = rateService;
            _branchService = branchService;
            _vehicleGroupService = vehicleGroupService;
        }
        public RateSearchResultModel Process(RateSearchQuery query) {

            var branches = _branchService.Get(new BranchFilterModel { Ids = query.SearchModel.BranchIds, IsArchived = false }, null).Items;
            var vehicleGroups = _vehicleGroupService.Get(new VehicleGroupFilterModel(), null).Items;
            var rates = _rateService.Get(new RateFilterModel { BranchIds = query.SearchModel.BranchIds, IsOpenEnded = query.SearchModel.IsOpenEnded, RateCodeId = query.SearchModel.RateCodeId, ValidEndDate = query.SearchModel.EndDate, ValidStartDate = query.SearchModel.StartDate, IsDeleted = false }, null).Items;

            RateSearchResultModel results = new RateSearchResultModel();
            results.Branches = branches;
            results.VehicleGroups = vehicleGroups;
            results.Rates = rates;

            return results;
        }
    }
}
