using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class CorporateRepository : RepositoryBase, ICorporateRepository {

        private const string CorporateNotFound = "Corporate could not be found";

        public CorporateRepository(IDbConnectionConfig connection) : base(connection) { }
        public CorporateModel Create(CorporateModel model) {
            Corporate c = new Corporate();
            c.Title = model.Title;
            _db.Corporates.Add(c);
            _db.SaveChanges();
            model.Id = c.Id;
            return model;
        }

        public List<CorporateModel> Get(CorporateFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (!string.IsNullOrEmpty(filter.Title))
                    list = list.Where(x => x.Title.Contains(filter.Title));
            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public CorporateModel GetById(int id) {
            var c = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (c == null)
                throw new Exception(CorporateNotFound);

            return c;
        }

        public int GetCount(CorporateFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (!string.IsNullOrEmpty(filter.Title))
                    list = list.Where(x => x.Title.Contains(filter.Title));
            }
            return list.Count();
        }

        public CorporateModel Update(CorporateModel model) {
            Corporate c = _db.Corporates.SingleOrDefault(x => x.Id == model.Id);
            if (c == null)
                throw new Exception(CorporateNotFound);
            c.Title = model.Title;
            _db.SaveChanges();
            return model;
        }


        public void AddRateCodeToCorporate(int corporateId, int rateCodeId) {
            var corpRateCode = _db.CorporateRateCodes.SingleOrDefault(x => x.CorporateId == corporateId && x.RateCodeId == rateCodeId);
            if (corpRateCode == null) {
                CorporateRateCode c = new CorporateRateCode();
                c.CorporateId = corporateId;
                c.RateCodeId = rateCodeId;
                _db.CorporateRateCodes.Add(c);
                _db.SaveChanges();
            }
        }

        public void RemoveRateCodeFromCorporate(int corporateId, int rateCodeId) {
            var corpRateCode = _db.CorporateRateCodes.SingleOrDefault(x => x.CorporateId == corporateId && x.RateCodeId == rateCodeId);
            if (corpRateCode != null) {
                _db.CorporateRateCodes.Remove(corpRateCode);
                _db.SaveChanges();
            }
        }

        private IQueryable<CorporateModel> getAsIQueryable() {
            return _db.Corporates.Select(x => new CorporateModel {
                Id = x.Id,
                Title = x.Title,
                RateCodes = x.CorporateRateCodes.Select(y=> new RateCodeModel {
                    Id  = y.RateCode.Id,
                    Title = y.RateCode.Title
                })
            });
        }
    }
}
