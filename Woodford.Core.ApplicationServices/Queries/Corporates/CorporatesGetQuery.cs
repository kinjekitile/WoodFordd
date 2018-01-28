using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class CorporatesGetQuery : IQuery<ListOf<CorporateModel>> {
        public CorporateFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }

    }

    public class CorporatesGetQueryHandler : IQueryHandler<CorporatesGetQuery, ListOf<CorporateModel>> {
        private readonly ICorporateService _corpService;
        public CorporatesGetQueryHandler(ICorporateService corpService) {
            _corpService = corpService;
        }

        public ListOf<CorporateModel> Process(CorporatesGetQuery query) {
            return _corpService.Get(query.Filter, query.Pagination);
        }
    }
}
