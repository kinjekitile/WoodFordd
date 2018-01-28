using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IInvoiceService {
        InvoiceModel Upsert(InvoiceModel model);
        //InvoiceModel Update(InvoiceModel model);
        //InvoiceModel Create(InvoiceModel model);
        ListOf<InvoiceModel> Get(InvoiceFilterModel filter, ListPaginationModel pagination);
        InvoiceModel GetById(int id);
        InvoiceModel GetByReservationId(int id);
        InvoiceModel CompleteInvoice(int id);
    }
}
