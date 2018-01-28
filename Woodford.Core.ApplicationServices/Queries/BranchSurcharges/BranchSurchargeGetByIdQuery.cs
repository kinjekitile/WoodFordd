using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class BranchSurchargeGetByIdQuery : IQuery<BranchSurchargeModel> {
        public int Id { get; set; }
    }

    public class BranchSurchargeGetByIdQueryHandler : IQueryHandler<BranchSurchargeGetByIdQuery, BranchSurchargeModel> {
        private readonly IBranchSurchargeService _surchargeService;
        public BranchSurchargeGetByIdQueryHandler(IBranchSurchargeService surchargeService) {
            _surchargeService = surchargeService;
        }

        public BranchSurchargeModel Process(BranchSurchargeGetByIdQuery query) {
            return _surchargeService.GetById(query.Id);
        }
    }
}
