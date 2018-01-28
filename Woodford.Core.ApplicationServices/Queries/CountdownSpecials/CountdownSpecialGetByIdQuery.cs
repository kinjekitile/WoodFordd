using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class CountdownSpecialGetByIdQuery : IQuery<CountdownSpecialModel> {
        public int Id { get; set; }        
    }

    public class CountdownSpecialGetByIdQueryHandler : IQueryHandler<CountdownSpecialGetByIdQuery, CountdownSpecialModel> {
        private readonly ICountdownSpecialService _countdownSpecialService;
        public CountdownSpecialGetByIdQueryHandler(ICountdownSpecialService countdownSpecialService) {
            _countdownSpecialService = countdownSpecialService;
        }

        public CountdownSpecialModel Process(CountdownSpecialGetByIdQuery query) {
            return _countdownSpecialService.GetById(query.Id);
        }
    }
}
