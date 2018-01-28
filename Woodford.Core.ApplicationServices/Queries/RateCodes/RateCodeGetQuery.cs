using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RateCodeGetQuery : IQuery<ListOf<RateCodeModel>> {
        public RateCodeFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }

    }

    public class RateCodeGetQueryHandler : IQueryHandler<RateCodeGetQuery, ListOf<RateCodeModel>> {
        private readonly IRateCodeService _rateCodeService;
        public RateCodeGetQueryHandler(IRateCodeService rateCodeService) {
            _rateCodeService = rateCodeService;
        }
        public ListOf<RateCodeModel> Process(RateCodeGetQuery query) {
            return _rateCodeService.Get(query.Filter, query.Pagination);
        }
    }
}
