using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class Payment3DSecureLookupCommand : ICommand {
        
        public PaymentRequestModel PayRequest { get; set; }
        public PaymentPortal3DSecureResponseModel LookupResponse { get; set; }
    }

    public class Payment3DSecureLookupCommandHandler : ICommandHandler<Payment3DSecureLookupCommand> {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IInvoiceService _invoiceService;

        public Payment3DSecureLookupCommandHandler(IPaymentProcessor paymentProcessor, IPaymentTransactionService paymentTransactionService, IInvoiceService invoiceService) {
            _paymentProcessor = paymentProcessor;
            _paymentTransactionService = paymentTransactionService;
            _invoiceService = invoiceService;
        }

        public void Handle(Payment3DSecureLookupCommand command) {
            throw new NotImplementedException();
            //var invoice = _invoiceService.GetByReservationId(command.PayRequest.ReservationId);


            //command.LookupResponse = _paymentProcessor.Secure3DLookup(command.PayRequest);

            //if (command.LookupResponse.Enrolled) {
            //    //Add transaction
            //    _paymentTransactionService.Upsert(new PaymentTransactionModel {
            //        InvoiceId = invoice.Id,
            //        TransactionId = command.LookupResponse.TransactionIndex,
            //        Transaction3DParEq = command.LookupResponse.ParEqMsg
            //    });
            //}            
        }
    }
}
