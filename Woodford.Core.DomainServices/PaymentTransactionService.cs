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
    public class PaymentTransactionService : IPaymentTransactionService {
        private readonly IPaymentTransactionRepository _repo;
        public PaymentTransactionService(IPaymentTransactionRepository repo) {
            _repo = repo;
        }

        public PaymentTransactionModel Upsert(PaymentTransactionModel model) {
            var transaction = GetByInvoiceId(model.InvoiceId);
            if (transaction == null) {
                model = _repo.Create(model);
            } else {
                model.Id = transaction.Id;
                model = _repo.Update(model);
            }
            return model;
        }

        public ListOf<PaymentTransactionModel> Get(PaymentTransactionFilterModel filter, ListPaginationModel pagination) {
            ListOf<PaymentTransactionModel> res = new ListOf<PaymentTransactionModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, pagination);
            }

            return res;
        }

        public PaymentTransactionModel GetById(int id) {
            return _repo.GetById(id);
        }

        public PaymentTransactionModel GetByInvoiceId(int invoiceId) {            
            return _repo.GetByInvoiceId(invoiceId);
        }

        public PaymentTransactionModel GetByTransactionId(string transactionId) {
            return _repo.GetByTransactionId(transactionId);
        }

        public PaymentTransactionModel Update(PaymentTransactionModel model) {
            return _repo.Update(model);
        }
    }
}
