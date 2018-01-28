using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.ApplicationServices.Utilities;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;

namespace Woodford.Core.ApplicationServices.Commands {
    public class PaymentProcessCommand : ICommand {
        public PaymentRequestModel PayRequest { get; set; }
        public InvoiceModel Invoice { get; set; }
        //public PaymentResponseModel PayResponse { get; set; }
        public PaymentPortalResponseModel PayResponse { get; set; }
        public string TransactionId { get; set; }

    }

    public class PaymentProcessCommandHandler : ICommandHandler<PaymentProcessCommand> {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly ISettingService _settings;
        private readonly IInvoiceService _invoiceService;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IReservationService _reservationService;
        private readonly INotify _notify;
        private readonly INotificationBuilder _notificationBuilder;
        private readonly IVoucherService _voucherService;

        public PaymentProcessCommandHandler(INotificationBuilder notificationBuilder, IPaymentProcessor paymentProcessor, ISettingService settings, IInvoiceService invoiceService, IPaymentTransactionService paymentTransactionService, IReservationService reservationService, INotify notify, IVoucherService voucherService) {
            _notificationBuilder = notificationBuilder;
            _paymentProcessor = paymentProcessor;
            _settings = settings;
            _invoiceService = invoiceService;
            _paymentTransactionService = paymentTransactionService;
            _reservationService = reservationService;
            _notify = notify;
            _voucherService = voucherService;
        }

        public void Handle(PaymentProcessCommand command) {
            //PayGate foreces 3d secure so no need for an options anymore
            //bool use3DSecure = (_settings.GetValue<bool>(Setting.Payment_Gateway_Use_3D_Secure));

            //var transaction = _paymentTransactionService.GetByInvoiceId(command.Invoice.Id);


            //var transactionId = "";

            ////this might be null if the card wasn't enrolled in 3d Secure
            //if (transaction != null)
            //    transactionId = transaction.TransactionId.ToString();
            //else
            //    transactionId = command.TransactionId;


            command.PayResponse = _paymentProcessor.MakePayment(command.PayRequest, "");


            if (command.PayResponse.Processed) {


                PaymentTransactionModel transaction = new PaymentTransactionModel();
                transaction.InvoiceId = command.Invoice.Id;

                string fauxGuid = "00000000-0000-0000-0000-000000000000";

                fauxGuid = fauxGuid.Substring(0, fauxGuid.Length - command.PayResponse.TransactionId.Length) + command.PayResponse.TransactionId;

                transaction.TransactionId = fauxGuid;
                //REmoved below as not used for PayGate
                //transaction.AuthorisationIdAuth = command.PayResponse.AuthorisationIDForAuth;
                //transaction.AuthorisationIdCapt = command.PayResponse.AuthorisationIDForCapt;

                _paymentTransactionService.Upsert(transaction);

                InvoiceModel i = _invoiceService.GetById(command.Invoice.Id);
                i.IsCompleted = true;
                _invoiceService.Upsert(i);

                ReservationModel r = _reservationService.GetById(command.PayRequest.ReservationId);

                r.ReservationState = ReservationState.Completed;
                r.IsQuote = false;
                r.ConfirmedDate = DateTime.Now;
                _reservationService.Update(r);

                if (r.VoucherId.HasValue) {
                    //Voucher applied
                    var voucher = _voucherService.GetById(r.VoucherId.Value);
                    voucher.DateRedeemed = DateTime.Now;
                    _voucherService.Update(voucher);
                }

                var invoiceEmailModel = _notificationBuilder.BuildReservationInvoiceModel(r.Id);


                _notify.SendNotifyReservationInvoice(invoiceEmailModel, Setting.Public_Website_Domain);
                _notify.SendNotifyReservationInvoiceSMS(new ReservationInvoiceNotificationModel { Reservation = r, Invoice = command.Invoice });

                if (r.LoyaltyPointsSpent.HasValue) {
                    if (r.LoyaltyPointsSpent.Value > 0) {


                        var emailModel = _notificationBuilder.BuildLoyaltyPointsSpentModel(r.Id);


                        _notify.SendNotifyLoyaltyPointsSpent(emailModel, Setting.Public_Website_Domain);
                        _notify.SendNotifyLoyaltyPointsSpentSMS(emailModel, Setting.Public_Website_Domain);
                    }
                }
            }


        }
    }
}
