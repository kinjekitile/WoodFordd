using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Queries {
    public class RateGetQuery : IQuery<ListOf<RateModel>> {
        public RateFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class RateGetQueryHandler : IQueryHandler<RateGetQuery, ListOf<RateModel>> {
        private readonly IRateService _rateService;
        public RateGetQueryHandler(IRateService rateService) {
            _rateService = rateService;
        }
        public ListOf<RateModel> Process(RateGetQuery query) {
            return _rateService.Get(query.Filter, query.Pagination);
            
        }
    }
}
