using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class RateCodeGetByIdQuery : IQuery<RateCodeModel> {
        public int Id { get; set; }        
    }

    public class RateCodeGetByIdQueryHandler : IQueryHandler<RateCodeGetByIdQuery, RateCodeModel> {
        private readonly IRateCodeService _rateCodeService;
        public RateCodeGetByIdQueryHandler(IRateCodeService rateCodeService) {
            _rateCodeService = rateCodeService;
        }
        public RateCodeModel Process(RateCodeGetByIdQuery query) {
            return _rateCodeService.GetById(query.Id);
        }
    }
}
