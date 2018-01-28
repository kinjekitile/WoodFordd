using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class UrlRedirectGetByIdQuery : IQuery<UrlRedirectModel> {

        public int Id { get; set; }
    }

    public class UrlRedirectGetByIdQueryHandler : IQueryHandler<UrlRedirectGetByIdQuery, UrlRedirectModel> {
        private readonly IUrlRedirectService _redirectService;
        public UrlRedirectGetByIdQueryHandler(IUrlRedirectService redirectService) {
            _redirectService = redirectService;
        }
        public UrlRedirectModel Process(UrlRedirectGetByIdQuery query) {
            return _redirectService.GetById(query.Id);
        }
    }
}
