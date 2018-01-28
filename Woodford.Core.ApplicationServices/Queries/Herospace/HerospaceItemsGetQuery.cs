using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class HerospaceItemsGetQuery : IQuery<List<HerospaceItemModel>> {
        public HerospaceItemFilterModel Filter { get; set; }        
    }

    public class HerospaceItemsGetQueryHandler : IQueryHandler<HerospaceItemsGetQuery, List<HerospaceItemModel>> {
        private readonly IHerospaceService _herospaceService;
        public HerospaceItemsGetQueryHandler(IHerospaceService herospaceService) {
            _herospaceService = herospaceService;
        }

        public List<HerospaceItemModel> Process(HerospaceItemsGetQuery query) {
            return _herospaceService.Get(query.Filter);
        }
    }
}
