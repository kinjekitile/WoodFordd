using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BranchesGetQuery : IQuery<ListOf<BranchModel>> {
        public BranchFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class BranchesGetQueryHandler : IQueryHandler<BranchesGetQuery, ListOf<BranchModel>> {
        private readonly IBranchService _branchService;
        public BranchesGetQueryHandler(IBranchService branchService) {
            _branchService = branchService;
        }

        public ListOf<BranchModel> Process(BranchesGetQuery query) {
            return _branchService.Get(query.Filter, query.Pagination);
        }
    }
}
