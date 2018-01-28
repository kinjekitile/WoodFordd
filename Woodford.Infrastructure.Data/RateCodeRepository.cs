using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class RateCodeRepository : RepositoryBase, IRateCodeRepository {
        private static string RateCodeNotFound = "Rate code could not be found";

        public RateCodeRepository(IDbConnectionConfig connection) : base(connection) { }

        public RateCodeModel Create(RateCodeModel model) {

            RateCode r = new RateCode();
            r.IsNotAdjustable = model.IsNotAdjustable;
            r.Title = model.Title;
            r.RateRuleId = model.RateRuleId;
            r.AvailableToPublic = model.AvailableToPublic;
            r.AvailableToLoyalty = model.AvailableToLoyalty;
            r.AvailableToCorporate = model.AvailableToCorporate;
            r.CanHaveUpgradeApplied = model.CanHaveUpgradeApplied;
            r.IsSticky = model.IsSticky;
            r.ColorCode = model.ColorCode;

            _db.RateCodes.Add(r);
            _db.SaveChanges();

            model.Id = r.Id;

            return model;
        }

        public RateCodeModel Update(RateCodeModel model) {
            RateCode r = _db.RateCodes.SingleOrDefault(x => x.Id == model.Id);
            if (r == null)
                throw new Exception(RateCodeNotFound);

            r.IsNotAdjustable = model.IsNotAdjustable;
            r.Title = model.Title;
            r.RateRuleId = model.RateRuleId;
            r.AvailableToPublic = model.AvailableToPublic;
            r.AvailableToLoyalty = model.AvailableToLoyalty;
            r.AvailableToCorporate = model.AvailableToCorporate;
            r.CanHaveUpgradeApplied = model.CanHaveUpgradeApplied;
            r.IsSticky = model.IsSticky;
            r.ColorCode = model.ColorCode;

            _db.SaveChanges();

            return model;
        }

        public List<RateCodeModel> Get(RateCodeFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);

            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }

        }

        public int GetCount(RateCodeFilterModel filter) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public RateCodeModel GetById(int id) {
            RateCodeModel rateCode = getAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (rateCode == null)
                throw new Exception(RateCodeNotFound);
            return rateCode;
        }

       private IQueryable<RateCodeModel> applyFilter(IQueryable<RateCodeModel> list, RateCodeFilterModel filter) {
            if (filter != null) {
                if (filter.IsNotAdjustable.HasValue)
                    list = list.Where(x => x.IsNotAdjustable == filter.IsNotAdjustable);
                if (!string.IsNullOrEmpty(filter.Title))
                    list = list.Where(x => x.Title.Contains(filter.Title));
                if (filter.RateRuleId.HasValue)
                    list = list.Where(x => x.RateRuleId == filter.RateRuleId);
                if (filter.AvailableToPublic.HasValue)
                    list = list.Where(x => x.AvailableToPublic == filter.AvailableToPublic.Value);
                if (filter.AvailableToLoyalty.HasValue)
                    list = list.Where(x => x.AvailableToLoyalty == filter.AvailableToLoyalty.Value);
                if (filter.AvailableToCorporate.HasValue)
                    list = list.Where(x => x.AvailableToCorporate == filter.AvailableToCorporate.Value);
                if (filter.CanHaveUpgradeApplied.HasValue) {
                    list = list.Where(x => x.CanHaveUpgradeApplied == filter.CanHaveUpgradeApplied.Value);
                }
            }
            return list;
        }

        private IQueryable<RateCodeModel> getAsIQueryable() {
            return _db.RateCodes.Select(x => new RateCodeModel {
                Id = x.Id,
                IsNotAdjustable = x.IsNotAdjustable,
                Title = x.Title,
                RateRuleId = x.RateRuleId,
                AvailableToPublic = x.AvailableToPublic,
                AvailableToLoyalty = x.AvailableToLoyalty,
                AvailableToCorporate = x.AvailableToCorporate,
                CanHaveUpgradeApplied = x.CanHaveUpgradeApplied,
                IsSticky = x.IsSticky,
                ColorCode = x.ColorCode
            });
        }
    }
}
