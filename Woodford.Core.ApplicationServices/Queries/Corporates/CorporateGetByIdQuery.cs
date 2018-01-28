using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class CorporateGetByIdQuery : IQuery<CorporateModel> {
        public int Id { get; set; }
    }

    public class CorporateGetByIdQueryHandler : IQueryHandler<CorporateGetByIdQuery, CorporateModel> {
        private readonly ICorporateService _corpService;
        public CorporateGetByIdQueryHandler(ICorporateService corpService) {
            _corpService = corpService;
        }
        public CorporateModel Process(CorporateGetByIdQuery query) {
            return _corpService.GetById(query.Id);
        }
    }


}
