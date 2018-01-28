using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class DynamicPagesGetQuery : IQuery<ListOfDynamicPageModel> {
        public DynamicPageFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class DynamicPagesGetQueryHandler : IQueryHandler<DynamicPagesGetQuery, ListOfDynamicPageModel> {
        private readonly IDynamicPageService _dynamicPageService;
        public DynamicPagesGetQueryHandler(IDynamicPageService dynamicPageService) {
            _dynamicPageService = dynamicPageService;
        }

        public ListOfDynamicPageModel Process(DynamicPagesGetQuery query) {
            return _dynamicPageService.Get(query.Filter, query.Pagination);
        }
    }
}
