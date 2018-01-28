using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Infrastructure.Data {
    public class MyGateRefundTransactionRepository : RepositoryBase, IRefundTransactionRepository {

        public MyGateRefundTransactionRepository(IDbConnectionConfig connection) : base(connection) { }

        public RefundTransactionModel Create(RefundTransactionModel model) {
            MyGateRefundTransaction r = new MyGateRefundTransaction();
            r.InvoiceId = model.InvoiceId;
            r.MyGateTransactionID = model.MyGateTransactionID;
            r.FSPMessage = model.Message;

            _db.MyGateRefundTransactions.Add(r);
            _db.SaveChanges();
            model.Id = r.Id;
            return model;
        }

        public List<RefundTransactionModel> Get(RefundTransactionFilterModel filter, ListPaginationModel pagination) {
            var list = getAsQueryable();
            list = applyFilter(list, filter);
            return list.ToList();
        }

        public RefundTransactionModel GetById(int id) {
            var r = getAsQueryable().SingleOrDefault(x => x.Id == id);
            if (r == null) {
                throw new Exception("Refund transaction could not be found");
            }
            return r;
        }

        public RefundTransactionModel GetByInvoiceId(int invoiceId) {
            var r = getAsQueryable().SingleOrDefault(x => x.InvoiceId == invoiceId);
            if (r == null) {
                throw new Exception("Refund transaction could not be found");
            }
            return r;
        }

        public RefundTransactionModel GetByReservationId(int reservationId) {
            throw new NotImplementedException();
        }

        public RefundTransactionModel GetByTransactionId(Guid transactionId) {
            var r = getAsQueryable().SingleOrDefault(x => x.MyGateTransactionID == transactionId);
            if (r == null) {
                throw new Exception("Refund transaction could not be found");
            }
            return r;
        }

        public int GetCount(RefundTransactionFilterModel filter) {
            var list = getAsQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public RefundTransactionModel Update(RefundTransactionModel model) {
            throw new NotImplementedException();
        }

        private IQueryable<RefundTransactionModel> applyFilter(IQueryable<RefundTransactionModel> list, RefundTransactionFilterModel filter) {
            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.InvoiceId.HasValue)
                    list = list.Where(x => x.InvoiceId == filter.InvoiceId.Value);
                if (filter.TransactionId.HasValue)
                    list = list.Where(x => x.MyGateTransactionID == filter.TransactionId.Value);
            }
            return list;
        }

        private IQueryable<RefundTransactionModel> getAsQueryable() {
            return _db.MyGateRefundTransactions.Select(x => new RefundTransactionModel {
                Id = x.Id,
                InvoiceId = x.InvoiceId,
                MyGateTransactionID = x.MyGateTransactionID.Value,
                Message = x.FSPMessage
            });
        }
    }
}
