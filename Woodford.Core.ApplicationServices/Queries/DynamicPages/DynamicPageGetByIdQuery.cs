using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class DynamicPageGetByIdQuery : IQuery<DynamicPageModel> {
        public int Id { get; set; }
        public bool IncludePageContent { get; set; }
    }

    public class DynamicPageGetByIdQueryHandler : IQueryHandler<DynamicPageGetByIdQuery, DynamicPageModel> {
        private readonly IDynamicPageService _dynamicPageService;
        public DynamicPageGetByIdQueryHandler(IDynamicPageService dynamicPageService) {
            _dynamicPageService = dynamicPageService;
        }

        public DynamicPageModel Process(DynamicPageGetByIdQuery query) {
            return _dynamicPageService.GetById(query.Id, query.IncludePageContent);
        }
    }
}
