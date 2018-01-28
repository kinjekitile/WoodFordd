using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BranchGetByUrlQuery : IQuery<BranchModel> {
        public string Url { get; set; }
        public bool IncludePageContent { get; set; }
    }

    public class BranchGetByUrlQueryHandler : IQueryHandler<BranchGetByUrlQuery, BranchModel> {
        private readonly IBranchService _branchService;
        public BranchGetByUrlQueryHandler(IBranchService branchService) {
            _branchService = branchService;
        }

        public BranchModel Process(BranchGetByUrlQuery query) {
            return _branchService.GetByUrl(query.Url, query.IncludePageContent);
        }
    }
}
