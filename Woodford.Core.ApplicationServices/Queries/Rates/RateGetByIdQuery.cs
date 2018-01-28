using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RateGetByIdQuery : IQuery<RateModel> {

    }

    public class RateGetByIdQueryHandler : IQueryHandler<RateGetByIdQuery, RateModel> {
        private readonly IRateService _rateService;
        public RateGetByIdQueryHandler(IRateService rateService) {
            _rateService = rateService;
        }
        public RateModel Process(RateGetByIdQuery query) {

            throw new NotImplementedException();
        }
    }
}
