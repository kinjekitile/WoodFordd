using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IPaymentTransactionRepository {
        PaymentTransactionModel Create(PaymentTransactionModel model);
        PaymentTransactionModel Update(PaymentTransactionModel model);
        PaymentTransactionModel GetById(int id);
        PaymentTransactionModel GetByInvoiceId(int invoiceId);
        PaymentTransactionModel GetByTransactionId(string transactionId);
        List<PaymentTransactionModel> Get(PaymentTransactionFilterModel filter, ListPaginationModel pagination);
        int GetCount(PaymentTransactionFilterModel filter);
    }
}
