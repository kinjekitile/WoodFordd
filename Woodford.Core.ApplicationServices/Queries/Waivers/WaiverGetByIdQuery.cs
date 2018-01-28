using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class WaiverGetByIdQuery : IQuery<WaiverModel> {
        public int Id { get; set; }
    }

    public class WaiverGetByIdQueryHandler : IQueryHandler<WaiverGetByIdQuery, WaiverModel> {
        private readonly IWaiverService _waiverService;
        public WaiverGetByIdQueryHandler(IWaiverService waiverService) {
            _waiverService = waiverService;
        }

        public WaiverModel Process(WaiverGetByIdQuery query) {
            return _waiverService.GetById(query.Id);
        }
    }
}
