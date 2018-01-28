using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BranchRateCodeExclusionsGetByIdQuery : IQuery<BranchRateCodeExclusionModel> {
        public int Id { get; set; }        
    }

    public class BranchRateCodeExclusionsGetByIdQueryHandler : IQueryHandler<BranchRateCodeExclusionsGetByIdQuery, BranchRateCodeExclusionModel> {
        private readonly IBranchRateCodeExclusionService _branchRateCodeExclusionService;
        public BranchRateCodeExclusionsGetByIdQueryHandler(IBranchRateCodeExclusionService branchRateCodeExclusionService) {
            _branchRateCodeExclusionService = branchRateCodeExclusionService;
        }

        public BranchRateCodeExclusionModel Process(BranchRateCodeExclusionsGetByIdQuery query) {
            return _branchRateCodeExclusionService.GetById(query.Id);
        }
    }
}
