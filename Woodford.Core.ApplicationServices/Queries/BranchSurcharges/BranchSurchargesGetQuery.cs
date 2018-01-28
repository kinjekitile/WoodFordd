using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BranchSurchargesGetQuery : IQuery<ListOf<BranchSurchargeModel>> {
        public BranchSurchargeFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class BranchSurchargesGetQueryHandler : IQueryHandler<BranchSurchargesGetQuery, ListOf<BranchSurchargeModel>> {
        private readonly IBranchSurchargeService _surchargeService;
        public BranchSurchargesGetQueryHandler(IBranchSurchargeService surchargeService) {
            _surchargeService = surchargeService;
        }

        public ListOf<BranchSurchargeModel> Process(BranchSurchargesGetQuery query) {
            return _surchargeService.Get(query.Filter, query.Pagination);
        }
    }
}
