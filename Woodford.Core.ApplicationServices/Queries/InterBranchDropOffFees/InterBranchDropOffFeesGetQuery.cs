using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class InterBranchDropOffFeesGetQuery : IQuery<ListOf<InterBranchDropOffFeeModel>> {
        public InterBranchDropOffFeeFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class InterBranchDropOffFeesGetQueryHandler : IQueryHandler<InterBranchDropOffFeesGetQuery, ListOf<InterBranchDropOffFeeModel>> {
        private readonly IInterBranchDropOffFeeService _interBranchDropOffFeeService;
        public InterBranchDropOffFeesGetQueryHandler(IInterBranchDropOffFeeService interBranchDropOffFeeService) {
            _interBranchDropOffFeeService = interBranchDropOffFeeService;
        }

        public ListOf<InterBranchDropOffFeeModel> Process(InterBranchDropOffFeesGetQuery query) {
            return _interBranchDropOffFeeService.Get(query.Filter, query.Pagination);
        }
    }
}
