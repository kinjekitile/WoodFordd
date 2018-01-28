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
    public class CorporateService : ICorporateService {
        private readonly ICorporateRepository _repo;
        public CorporateService(ICorporateRepository repo) {
            _repo = repo;
        }
        public CorporateModel Create(CorporateModel model) {
            return _repo.Create(model);
        }

        public ListOf<CorporateModel> Get(CorporateFilterModel filter, ListPaginationModel pagination) {
            ListOf<CorporateModel> res = new ListOf<CorporateModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;

            
        }

        public CorporateModel GetById(int id) {
            return _repo.GetById(id);
        }

        public CorporateModel Update(CorporateModel model) {
            return _repo.Update(model);
        }

        public void AddRateCodeToCorporate(int corporateId, int rateCodeId) {
            _repo.AddRateCodeToCorporate(corporateId, rateCodeId);
        }

        public void RemoveRateCodeFromCorporate(int corporateId, int rateCodeId) {
            _repo.RemoveRateCodeFromCorporate(corporateId, rateCodeId);
        }
    }
}
