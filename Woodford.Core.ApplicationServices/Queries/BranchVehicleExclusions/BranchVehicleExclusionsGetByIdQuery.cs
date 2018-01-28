using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BranchVehicleExclusionsGetByIdQuery : IQuery<BranchVehicleExclusionModel> {
        public int Id { get; set; }        
    }

    public class BranchVehicleExclusionsGetByIdQueryHandler : IQueryHandler<BranchVehicleExclusionsGetByIdQuery, BranchVehicleExclusionModel> {
        private readonly IBranchVehicleExclusionService _branchVehicleExclusionService;
        public BranchVehicleExclusionsGetByIdQueryHandler(IBranchVehicleExclusionService branchVehicleExclusionService) {
            _branchVehicleExclusionService = branchVehicleExclusionService;
        }

        public BranchVehicleExclusionModel Process(BranchVehicleExclusionsGetByIdQuery query) {
            return _branchVehicleExclusionService.GetById(query.Id);
        }
    }
}
