using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IPaymentTransactionService {
        PaymentTransactionModel Upsert(PaymentTransactionModel model);
        
        PaymentTransactionModel GetById(int id);
        ListOf<PaymentTransactionModel> Get(PaymentTransactionFilterModel filter, ListPaginationModel pagination);
        PaymentTransactionModel GetByInvoiceId(int invoiceId);

        PaymentTransactionModel GetByTransactionId(string transactionId);
    }
}
