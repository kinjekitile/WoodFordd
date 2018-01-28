using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BranchRateCodeExclusionsGetQuery : IQuery<ListOf<BranchRateCodeExclusionModel>> {
        public BranchRateCodeExclusionFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class BranchRateCodeExclusionsGetQueryHandler : IQueryHandler<BranchRateCodeExclusionsGetQuery, ListOf<BranchRateCodeExclusionModel>> {
        private readonly IBranchRateCodeExclusionService _branchRateCodeExclusionService;
        public BranchRateCodeExclusionsGetQueryHandler(IBranchRateCodeExclusionService branchRateCodeExclusionService) {
            _branchRateCodeExclusionService = branchRateCodeExclusionService;
        }

        public ListOf<BranchRateCodeExclusionModel> Process(BranchRateCodeExclusionsGetQuery query) {
            return _branchRateCodeExclusionService.Get(query.Filter, query.Pagination);
        }
    }
}
