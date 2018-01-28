using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class InterBranchDropOffFeeGetByIdQuery : IQuery<InterBranchDropOffFeeModel> {
        public int Id { get; set; }
    }

    public class InterBranchDropOffFeeGetByIdQueryHandler : IQueryHandler<InterBranchDropOffFeeGetByIdQuery, InterBranchDropOffFeeModel> {
        private readonly IInterBranchDropOffFeeService _interBranchDropOffFeeService;
        public InterBranchDropOffFeeGetByIdQueryHandler(IInterBranchDropOffFeeService interBranchDropOffFeeService) {
            _interBranchDropOffFeeService = interBranchDropOffFeeService;
        }

        public InterBranchDropOffFeeModel Process(InterBranchDropOffFeeGetByIdQuery query) {
            return _interBranchDropOffFeeService.GetById(query.Id);
        }
    }
}
