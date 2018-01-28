using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class CountdownSpecialsGetQuery : IQuery<ListOf<CountdownSpecialModel>> {
        public CountdownSpecialFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class CountdownSpecialsGetQueryHandler : IQueryHandler<CountdownSpecialsGetQuery, ListOf<CountdownSpecialModel>> {
        private readonly ICountdownSpecialService _countdownSpecialService;
        public CountdownSpecialsGetQueryHandler(ICountdownSpecialService countdownSpecialService) {
            _countdownSpecialService = countdownSpecialService;
        }

        public ListOf<CountdownSpecialModel> Process(CountdownSpecialsGetQuery query) {
            return _countdownSpecialService.Get(query.Filter, query.Pagination);
        }
    }
}
