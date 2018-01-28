using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class InvoiceUpsertCommand : ICommand {
        public InvoiceModel Invoice { get; set; }
    }

    public class InvoiceUpsertCommandHandler : ICommandHandler<InvoiceUpsertCommand> {
        private readonly IInvoiceService _invoiceService;
        public InvoiceUpsertCommandHandler(IInvoiceService invoiceService) {
            _invoiceService = invoiceService;
        }
        public void Handle(InvoiceUpsertCommand command) {
            command.Invoice = _invoiceService.Upsert(command.Invoice);
        }
    }
}
