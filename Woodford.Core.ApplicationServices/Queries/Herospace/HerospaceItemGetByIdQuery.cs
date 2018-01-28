using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class HerospaceItemGetByIdQuery : IQuery<HerospaceItemModel> {
        public int Id { get; set; }        
    }

    public class HerospaceItemGetByIdQueryHandler : IQueryHandler<HerospaceItemGetByIdQuery, HerospaceItemModel> {
        private readonly IHerospaceService _herospaceService;
        public HerospaceItemGetByIdQueryHandler(IHerospaceService herospaceService) {
            _herospaceService = herospaceService;
        }

        public HerospaceItemModel Process(HerospaceItemGetByIdQuery query) {
            return _herospaceService.GetById(query.Id);
        }
    }
}
