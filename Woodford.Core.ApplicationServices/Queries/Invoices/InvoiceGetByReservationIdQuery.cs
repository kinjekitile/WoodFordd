using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class InvoiceGetByReservationIdQuery : IQuery<InvoiceModel> {
        public int ReservationId { get; set; }
    }

    public class InvoiceGetByReservationIdQueryHandler : IQueryHandler<InvoiceGetByReservationIdQuery, InvoiceModel> {
        private readonly IInvoiceService _invoiceService;
        public InvoiceGetByReservationIdQueryHandler(IInvoiceService invoiceService) {
            _invoiceService = invoiceService;
        }
        public InvoiceModel Process(InvoiceGetByReservationIdQuery query) {
            return _invoiceService.GetByReservationId(query.ReservationId);
        }
    }
}
