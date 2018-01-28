using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.DomainServices {
    public class RateCodeService : IRateCodeService {
        private readonly IRateCodeRepository _repo;
        public RateCodeService(IRateCodeRepository repo) {
            _repo = repo;
        }
        public RateCodeModel Create(RateCodeModel model) {
            return _repo.Create(model);
        }

        public ListOf<RateCodeModel> Get(RateCodeFilterModel filter, ListPaginationModel pagination) {
            var results = new ListOf<RateCodeModel>();
            results.Pagination = pagination;            
            if (pagination == null) {
                results.Items = _repo.Get(filter, pagination);
            } else {
                results.Pagination.TotalItems = _repo.GetCount(filter);
                results.Items = _repo.Get(filter, results.Pagination);
            }

            return results;
        }

        public RateCodeModel GetById(int id) {
            return _repo.GetById(id);
        }        

        public RateCodeModel Update(RateCodeModel model) {
            return _repo.Update(model);
        }
    }
}
