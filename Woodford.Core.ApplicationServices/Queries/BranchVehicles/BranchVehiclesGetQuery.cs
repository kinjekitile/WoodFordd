using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BranchVehiclesGetQuery : IQuery<ListOf<BranchVehicleModel>> {
        public BranchVehicleFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class BranchVehiclesGetQueryHandler : IQueryHandler<BranchVehiclesGetQuery, ListOf<BranchVehicleModel>> {
        private readonly IBranchVehicleService _branchVehicleService;
        public BranchVehiclesGetQueryHandler(IBranchVehicleService branchVehicleService) {
            _branchVehicleService = branchVehicleService;
        }

        public ListOf<BranchVehicleModel> Process(BranchVehiclesGetQuery query) {
            return _branchVehicleService.Get(query.Filter, query.Pagination);
        }
    }
}
