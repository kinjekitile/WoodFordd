using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationMarkAsPaidCommand : ICommand {
        //public ReservationModel Reservation { get; set; }
        public int ReservationId { get; set; }
        public bool IsMobileDevice { get; set; }
        public bool IsCorporate { get; set; }
        public decimal AmountPaid { get; set; }
        public PaymentTransactionModel PaymentTransaction { get; set; }
        public InvoiceModel InvoiceOut { get; set; }

        public bool NoTransaction { get; set; }
    }

    public class ReservationMarkAsPaidCommandHandler : ICommandHandler<ReservationMarkAsPaidCommand> {
        private readonly IInvoiceService _invoiceService;
        private readonly IReservationService _reservationService;
        private readonly INotify _notify;
        private readonly IUserService _userService;
        private readonly IVehicleService _vehicleService;
        private readonly INotificationBuilder _notificationBuilder;
        private readonly IVoucherService _voucherService;
        private readonly IPaymentTransactionService _paymentTransactionService;

        public ReservationMarkAsPaidCommandHandler(INotificationBuilder notificationBuilder, IReservationService reservationService, IInvoiceService invoiceService, INotify notify, IUserService userService, IVehicleService vehicleService, IVoucherService voucherService, IPaymentTransactionService paymentTransactionService) {
            _notificationBuilder = notificationBuilder;
            _invoiceService = invoiceService;
            _reservationService = reservationService;
            _notify = notify;
            _userService = userService;
            _vehicleService = vehicleService;
            _voucherService = voucherService;
            _paymentTransactionService = paymentTransactionService;
        }
        public void Handle(ReservationMarkAsPaidCommand command) {
            InvoiceModel i = new InvoiceModel { ReservationId = command.ReservationId, IsMobileCheckout = command.IsMobileDevice, IsCorporateCheckout = command.IsCorporate, AmountPaid = command.AmountPaid };
            if (command.PaymentTransaction != null) {
                i.PaymentTransaction = command.PaymentTransaction;
            }
            i.IsCompleted = true;
            command.InvoiceOut = _invoiceService.Upsert(i);

            if (command.NoTransaction) {

            } else {
                var transaction = _paymentTransactionService.GetByInvoiceId(i.Id);
                var transactionId = "";

                //this might be null if the card wasn't enrolled in 3d Secure
                if (transaction != null)
                    transactionId = transaction.TransactionId.ToString();
                else
                    transactionId = command.PaymentTransaction.TransactionId;

                if (transaction == null) {
                    transaction = new PaymentTransactionModel();
                    transaction.InvoiceId = i.Id;
                    transaction.TransactionId = command.PaymentTransaction.TransactionId;
                    //REmoved below as not used for PayGate
                    //transaction.AuthorisationIdAuth = command.PayResponse.AuthorisationIDForAuth;
                    //transaction.AuthorisationIdCapt = command.PayResponse.AuthorisationIDForCapt;
                }
                _paymentTransactionService.Upsert(transaction);

            }






            ReservationModel r = _reservationService.GetById(command.ReservationId);
            r.Vehicle = _vehicleService.GetById(r.VehicleId, includePageContent: false);
            r.IsQuote = false;
            r.ConfirmedDate = DateTime.Now;
            if (r.VehicleUpgradeId.HasValue) {
                r.VehicleUpgrade = _vehicleService.GetById(r.VehicleUpgradeId.Value, includePageContent: false);
            }
            if (r.UserId.HasValue) {
                UserModel user = _userService.GetById(r.UserId.Value);

                if (command.IsCorporate) {
                    r.CorporateId = user.CorporateId;
                }

                
            } 

            if (r.VoucherId.HasValue) {
                //Voucher applied
                var voucher = _voucherService.GetById(r.VoucherId.Value);
                voucher.DateRedeemed = DateTime.Now;
                _voucherService.Update(voucher);
            }
            _reservationService.Update(r);

            var invoiceEmailModel = _notificationBuilder.BuildReservationInvoiceModel(r.Id);

            _notify.SendNotifyReservationInvoice(invoiceEmailModel, Setting.Public_Website_Domain);
            _notify.SendNotifyReservationInvoiceSMS(new ReservationInvoiceNotificationModel { Reservation = r, Invoice = command.InvoiceOut });

        }
    }
}
