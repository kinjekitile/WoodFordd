using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RateRuleGetByIdQuery : IQuery<RateRuleModel> {
        public int Id { get; set; }        
    }

    public class RateRuleGetByIdQueryHandler : IQueryHandler<RateRuleGetByIdQuery, RateRuleModel> {
        private readonly IRateRuleService _rateRuleService;
        public RateRuleGetByIdQueryHandler(IRateRuleService rateRuleService) {
            _rateRuleService = rateRuleService;
        }
        public RateRuleModel Process(RateRuleGetByIdQuery query) {
            return _rateRuleService.GetById(query.Id);
        }
    }
}
