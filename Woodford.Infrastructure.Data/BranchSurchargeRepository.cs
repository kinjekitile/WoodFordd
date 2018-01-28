using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class BranchSurchargeRepository : RepositoryBase, IBranchSurchageRepository {

        private const string NotFound = "Surcharge not found";

        public BranchSurchargeRepository(IDbConnectionConfig connection) : base(connection) {

        }
        public BranchSurchargeModel Create(BranchSurchargeModel model) {
            BranchSurcharge b = new BranchSurcharge();
            b.Title = model.Title;
            b.BranchId = model.BranchId;
            b.SurchargeAmount = model.SurchargeAmount;
            b.IsOnceOff = model.IsOnceOff;
            b.MaximumCharge = model.MaximumCharge;
            _db.BranchSurcharges.Add(b);
            _db.SaveChanges();

            model.Id = b.Id;
            return model;
        }

        public List<BranchSurchargeModel> Get(BranchSurchargeFilterModel filter, ListPaginationModel pagination) {

            var list = getAsIQueryable();

            list = applyFilter(list, filter);

            return list.ToList();
        }

        public BranchSurchargeModel GetById(int id) {
            BranchSurchargeModel b = getAsIQueryable().SingleOrDefault(x => x.Id == id);

            if (b == null)
                throw new Exception(NotFound);
            return b;
        }

        public int GetCount(BranchSurchargeFilterModel filter) {
            throw new NotImplementedException();
        }

        public BranchSurchargeModel Update(BranchSurchargeModel model) {
            BranchSurcharge b = _db.BranchSurcharges.SingleOrDefault(x => x.Id == model.Id);
            if (b == null)
                throw new Exception(NotFound);


            b.Title = model.Title;
            b.BranchId = model.BranchId;
            b.SurchargeAmount = model.SurchargeAmount;
            b.IsOnceOff = model.IsOnceOff;
            b.MaximumCharge = model.MaximumCharge;
            _db.SaveChanges();

            return model;
        }

        IQueryable<BranchSurchargeModel> applyFilter(IQueryable<BranchSurchargeModel> list, BranchSurchargeFilterModel filter) {

            if (filter != null) {
                if (filter.BranchId.HasValue) {
                    list = list.Where(x => x.BranchId == filter.BranchId);
                }
            }

            return list;
        }

        private IQueryable<BranchSurchargeModel> getAsIQueryable() {
            return _db.BranchSurcharges.Select(x => new BranchSurchargeModel {
                Id = x.Id,
                Title = x.Title,
                BranchId = x.BranchId,
                SurchargeAmount = x.SurchargeAmount,
                IsOnceOff = x.IsOnceOff,
                MaximumCharge = x.MaximumCharge
            });
        }
    }
}
