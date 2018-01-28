using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class CountdownSpecialRepository : RepositoryBase, ICountdownSpecialRepository {

        private const string CountdownSpecialNotFound = "Countdown special could not be found";

        public CountdownSpecialRepository(IDbConnectionConfig connection) : base(connection) { }

        public CountdownSpecialModel Create(CountdownSpecialModel model) {
            CountdownSpecial s = new CountdownSpecial();
            s.Title = model.Title;
            s.OfferText = model.OfferText;
            s.OfferDiscount = model.OfferDiscount;
            s.OfferType = (int)model.SpecialType;
            s.IsActive = true;
            s.VehicleUpgradeId = model.VehicleUpgradeId;
            s.VehicleUpgradeAmountOverride = model.VehicleUpgradeAmountOverride;
            _db.CountdownSpecials.Add(s);
            _db.SaveChanges();
            model.Id = s.Id;
            return model;
        }

        public List<CountdownSpecialModel> Get(CountdownSpecialFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue) list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.IsActive.HasValue) list = list.Where(x => x.IsActive == filter.IsActive.Value);
                if (filter.SpecialType.HasValue) list = list.Where(x => x.SpecialType == filter.SpecialType.Value);
            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public int GetCount(CountdownSpecialFilterModel filter) {
            var list = GetAsIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue) list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.IsActive.HasValue) list = list.Where(x => x.IsActive == filter.IsActive.Value);
                if (filter.SpecialType.HasValue) list = list.Where(x => x.SpecialType == filter.SpecialType.Value);
            }
            return list.Count();
        }

        public CountdownSpecialModel GetById(int id) {
            CountdownSpecialModel s = GetAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (s == null) throw new Exception(CountdownSpecialNotFound);
            return s;
        }

        public CountdownSpecialModel Update(CountdownSpecialModel model) {
            CountdownSpecial s = _db.CountdownSpecials.Where(x => x.Id == model.Id).SingleOrDefault();
            if (s == null) throw new Exception(CountdownSpecialNotFound);
            s.Title = model.Title;
            s.OfferText = model.OfferText;
            s.OfferDiscount = model.OfferDiscount;
            s.OfferType = (int)model.SpecialType;
            s.IsActive = model.IsActive;
            s.VehicleUpgradeId = model.VehicleUpgradeId;
            s.VehicleUpgradeAmountOverride = model.VehicleUpgradeAmountOverride;
            _db.SaveChanges();
            return model;
        }

        private IQueryable<CountdownSpecialModel> GetAsIQueryable() {
            return _db.CountdownSpecials.Select(x => new CountdownSpecialModel {
                Id = x.Id,
                Title = x.Title,
                OfferText = x.OfferText,
                OfferDiscount = x.OfferDiscount,
                SpecialType = (CountdownSpecialType)x.OfferType,
                IsActive = x.IsActive,
                VehicleUpgradeId = x.VehicleUpgradeId,
                VehicleUpgradeAmountOverride = x.VehicleUpgradeAmountOverride
            });
        }
    }
}
