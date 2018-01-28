using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RateAdjustmentGetQuery : IQuery<ListOf<RateAdjustmentModel>> {
        public RateAdjustmentFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }

        
    }

    public class RateAdjustmentGetQueryHandler : IQueryHandler<RateAdjustmentGetQuery, ListOf<RateAdjustmentModel>> {
        private readonly IRateAdjustmentService _rateAdjustService;
        public RateAdjustmentGetQueryHandler(IRateAdjustmentService rateAdjustService) {
            _rateAdjustService = rateAdjustService;
        }
        public ListOf<RateAdjustmentModel> Process(RateAdjustmentGetQuery query) {
            return _rateAdjustService.Get(query.Filter, query.Pagination);
        }
    }
}
