using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
   public class UrlRedirectGetQuery : IQuery<ListOf<UrlRedirectModel>> {
        public UrlRedirectFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class UrlRedirectGetQueryHandler : IQueryHandler<UrlRedirectGetQuery, ListOf<UrlRedirectModel>> {
        private readonly IUrlRedirectService _redirectService;
        public UrlRedirectGetQueryHandler(IUrlRedirectService redirectService) {
            _redirectService = redirectService;
        }
        public ListOf<UrlRedirectModel> Process(UrlRedirectGetQuery query) {
            return _redirectService.Get(query.Filter, query.Pagination);
        }
    }
}
