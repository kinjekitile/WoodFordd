using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class RefundRepository : RepositoryBase, IRefundRepository {
        public RefundRepository(IDbConnectionConfig connection) : base(connection) { }

        public RefundModel Create(RefundModel model) {
            Refund r = new Refund();
            r.InvoiceId = model.InvoiceId;
            r.ReservationId = model.ReservationId;
            r.Amount = model.Amount;
            r.RefundedDate = model.RefundDate;

            _db.Refunds.Add(r);
            _db.SaveChanges();
            model.Id = r.Id;
            return model;
        }

        

        public List<RefundModel> Get(RefundFilterModel filter, ListPaginationModel pagination) {
            var list = getAsQueryable();
            list = applyFilter(list, filter);
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
            
        }

        private IQueryable<RefundModel> applyFilter(IQueryable<RefundModel> list, RefundFilterModel filter) {
            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);

                if (filter.InvoiceId.HasValue)
                    list = list.Where(x => x.InvoiceId == filter.InvoiceId.Value);

                if (filter.ReservationId.HasValue)
                    list = list.Where(x => x.ReservationId == filter.ReservationId.Value);

                if (filter.StartDate.HasValue && filter.EndDate.HasValue)
                    list = list.Where(x => x.RefundDate >= filter.StartDate.Value && x.RefundDate <= filter.EndDate.Value);
            }

            return list;
        }

        public RefundModel GetById(int id) {
            RefundModel r = getAsQueryable().SingleOrDefault(x => x.Id == id);
            if (r == null)
                throw new Exception("Refund could not be found");
            return r;
        }



        public int GetCount(RefundFilterModel filter) {
            var list = getAsQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public RefundModel Update(RefundModel model) {
            //Refund r = _db.Refunds.SingleOrDefault(x => x.Id == model.Id);
            //if (r == null)
            //    throw new Exception("Refund not found");

            throw new NotImplementedException();
        }

        private IQueryable<RefundModel> getAsQueryable() {
            return _db.Refunds.Select(x => new RefundModel {
                Id = x.Id,
                ReservationId = x.ReservationId,
                InvoiceId = x.InvoiceId,
                Amount = x.Amount,
                RefundDate = x.RefundedDate
            });
        }
    }
}
