using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RateRuleGetQuery : IQuery<ListOf<RateRuleModel>> {
        public RateRuleFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }

    }

    public class RateRuleGetQueryHandler : IQueryHandler<RateRuleGetQuery, ListOf<RateRuleModel>> {
        private readonly IRateRuleService _rateRuleService;
        public RateRuleGetQueryHandler(IRateRuleService rateRuleService) {
            _rateRuleService = rateRuleService;
        }
        public ListOf<RateRuleModel> Process(RateRuleGetQuery query) {
            return _rateRuleService.Get(query.Filter, query.Pagination);
        }
    }
}
