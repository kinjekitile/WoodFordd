using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class DynamicPageIdGetByUrlQuery : IQuery<int?> {
        public string Url { get; set; }
    }

    public class DynamicPageIdGetByUrlQueryHandler : IQueryHandler<DynamicPageIdGetByUrlQuery, int?> {
        private readonly IDynamicPageService _dynamicPageService;
        public DynamicPageIdGetByUrlQueryHandler(IDynamicPageService dynamicPageService) {
            _dynamicPageService = dynamicPageService;
        }

        public int? Process(DynamicPageIdGetByUrlQuery query) {
            return _dynamicPageService.GetIdByUrl(query.Url);
        }
    }
}
