using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Infrastructure.Data {
    public class MyGatePaymentTransactionRepository : RepositoryBase, IPaymentTransactionRepository {
        private const string TransactionNotFound = "Transaction could not be found";
        public MyGatePaymentTransactionRepository(IDbConnectionConfig connection) : base(connection) {

        }

        public PaymentTransactionModel Create(PaymentTransactionModel model) {
            MyGateTransaction t = new MyGateTransaction();
            t.InvoiceId = model.InvoiceId;
            t.MyGateAuthorisationIDAuth = model.AuthorisationIdAuth;
            t.MyGateAuthorisationIDCapt = model.AuthorisationIdCapt;
            t.MyGateTransactionID = new Guid(model.TransactionId);
            t.MyGate3DParEq = model.Transaction3DParEq;
            _db.MyGateTransactions.Add(t);
            _db.SaveChanges();
            model.Id = t.Id;
            return model;
        }

        public List<PaymentTransactionModel> Get(PaymentTransactionFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);
            return list.ToList();
        }

        public PaymentTransactionModel GetById(int id) {
            PaymentTransactionModel model = getAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (model == null)
                throw new Exception(TransactionNotFound);
            return model;
        }

        public PaymentTransactionModel GetByInvoiceId(int invoiceId) {
            PaymentTransactionModel model = getAsIQueryable().SingleOrDefault(x => x.InvoiceId == invoiceId);
            return model;
        }

        public PaymentTransactionModel GetByTransactionId(string transactionId) {
            //Guid transId = new Guid(transactionId);
            PaymentTransactionModel model = getAsIQueryable().SingleOrDefault(x => x.TransactionId == transactionId);
            return model;
        }

        public int GetCount(PaymentTransactionFilterModel filter) {
            var list = getAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public PaymentTransactionModel Update(PaymentTransactionModel model) {
            MyGateTransaction p = _db.MyGateTransactions.SingleOrDefault(x => x.Id == model.Id);
            if (p == null)
                throw new Exception(TransactionNotFound);
            p.MyGateAuthorisationIDAuth = model.AuthorisationIdAuth;
            p.MyGateAuthorisationIDCapt = model.AuthorisationIdCapt;
            p.MyGateTransactionID =new Guid(model.TransactionId);
            p.MyGate3DParEq = model.Transaction3DParEq;
            _db.SaveChanges();
            return model;
        }

        private IQueryable<PaymentTransactionModel> applyFilter(IQueryable<PaymentTransactionModel> list, PaymentTransactionFilterModel filter) {
            if (filter != null) {
                list = list.Where(x => x.InvoiceId == filter.InvoiceId.Value);
            }
            return list;
        }

        private IQueryable<PaymentTransactionModel> getAsIQueryable() {
            return _db.MyGateTransactions.Select(x => new PaymentTransactionModel {
                Id = x.Id,
                InvoiceId = x.InvoiceId,
                AuthorisationIdAuth = x.MyGateAuthorisationIDAuth,
                AuthorisationIdCapt = x.MyGateAuthorisationIDCapt,
                TransactionId = x.MyGateTransactionID.ToString(),
                Transaction3DParEq = x.MyGate3DParEq
            });
        }
    }
}
