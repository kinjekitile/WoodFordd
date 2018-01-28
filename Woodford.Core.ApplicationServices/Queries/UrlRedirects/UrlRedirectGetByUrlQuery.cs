using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class UrlRedirectGetByUrlQuery : IQuery<UrlRedirectModel> {
        public string Url { get; set; }
    }

    public class UrlRedirectGetByUrlQueryHandler : IQueryHandler<UrlRedirectGetByUrlQuery, UrlRedirectModel> {
        private readonly IUrlRedirectService _redirectService;
        public UrlRedirectGetByUrlQueryHandler(IUrlRedirectService redirectService) {
            _redirectService = redirectService;
        }
        public UrlRedirectModel Process(UrlRedirectGetByUrlQuery query) {
            return _redirectService.GetByUrl(query.Url);
        }
    }
}
