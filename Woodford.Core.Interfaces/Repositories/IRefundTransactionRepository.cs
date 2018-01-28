using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IRefundTransactionRepository {
        RefundTransactionModel Create(RefundTransactionModel model);
        RefundTransactionModel Update(RefundTransactionModel model);
        RefundTransactionModel GetById(int id);
        RefundTransactionModel GetByInvoiceId(int invoiceId);
        RefundTransactionModel GetByReservationId(int reservationId);
        RefundTransactionModel GetByTransactionId(Guid transactionId);
        List<RefundTransactionModel> Get(RefundTransactionFilterModel filter, ListPaginationModel pagination);
        int GetCount(RefundTransactionFilterModel filter);
    }
}
