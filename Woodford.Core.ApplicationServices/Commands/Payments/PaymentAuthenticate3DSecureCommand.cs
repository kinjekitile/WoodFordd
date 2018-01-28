using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class PaymentAuthenticate3DSecureCommand : ICommand {
        public string TransactionId { get; set; }
        public string ParEq3DSecure { get; set; }

        public bool Authenticated { get; set; }

    }

    public class PaymentAuthenticate3DSecureCommandHandler : ICommandHandler<PaymentAuthenticate3DSecureCommand> {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IPaymentTransactionService _paymentTransactionService;
        public PaymentAuthenticate3DSecureCommandHandler(IPaymentProcessor paymentProcessor, IPaymentTransactionService paymentTransactionService) {
            _paymentProcessor = paymentProcessor;
            _paymentTransactionService = paymentTransactionService;
        }
        public void Handle(PaymentAuthenticate3DSecureCommand command) {

            try {
                var transaction = _paymentTransactionService.GetByTransactionId(command.TransactionId);
                transaction.Transaction3DParEq = command.ParEq3DSecure;

                _paymentTransactionService.Upsert(transaction);

                bool sucess = _paymentProcessor.Authenticate3DSecure(command.TransactionId, command.ParEq3DSecure);
                command.Authenticated = sucess;
            } catch (Exception ex) {
                command.Authenticated = false;
            }
            
            
        }
    }
}
