using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RateAdjustmentGetByIdQuery : IQuery<RateAdjustmentModel> {
        public int Id { get; set; }
    }

    public class RateAdjustmentGetByIdQueryHandler : IQueryHandler<RateAdjustmentGetByIdQuery, RateAdjustmentModel> {
        private readonly IRateAdjustmentService _rateAdjustService;
        public RateAdjustmentGetByIdQueryHandler(IRateAdjustmentService rateAdjustService) {
            _rateAdjustService = rateAdjustService;
        }
        public RateAdjustmentModel Process(RateAdjustmentGetByIdQuery query) {
            return _rateAdjustService.GetById(query.Id);
        }
    }
}
