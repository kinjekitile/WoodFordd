using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IInvoiceRepository {
        InvoiceModel Create(InvoiceModel model);
        InvoiceModel Update(InvoiceModel model);
        List<InvoiceModel> Get(InvoiceFilterModel filter, ListPaginationModel pagination);
        int GetCount(InvoiceFilterModel filter);
        InvoiceModel GetById(int id);

        InvoiceModel GetByReservationId(int reservationId);
    }
}
