using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BranchGetByIdQuery : IQuery<BranchModel> {
        public int Id { get; set; }
        public bool IncludePageContent { get; set; }
    }

    public class BranchGetByIdQueryHandler : IQueryHandler<BranchGetByIdQuery, BranchModel> {
        private readonly IBranchService _branchService;
        public BranchGetByIdQueryHandler(IBranchService branchService) {
            _branchService = branchService;
        }

        public BranchModel Process(BranchGetByIdQuery query) {
            return _branchService.GetById(query.Id, query.IncludePageContent);
        }
    }
}
