using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class WaiversGetQuery : IQuery<List<WaiverModel>> {
        public WaiverFilterModel Filter { get; set; }
    }

    public class WaiversGetQueryHandler : IQueryHandler<WaiversGetQuery, List<WaiverModel>> {
        private readonly IWaiverService _waiverService;
        public WaiversGetQueryHandler(IWaiverService waiverService) {
            _waiverService = waiverService;
        }

        public List<WaiverModel> Process(WaiversGetQuery query) {
            return _waiverService.Get(query.Filter);
        }
    }
}
