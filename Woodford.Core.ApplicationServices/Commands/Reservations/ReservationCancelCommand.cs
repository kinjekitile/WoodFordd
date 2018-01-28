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
    public class ReservationCancelCommand : ICommand {
        public bool IgnoreLeadTime { get; set; }
        public int ReservationId { get; set; }
        public bool IsCancelled { get; set; }
        public bool OutStatus { get; set; }
        public ReservationCancelResponseState OutCancelStatus { get; set; }
        
    }

    public class ReservationCancelCommandHandler : ICommandHandler<ReservationCancelCommand> {
        
        private readonly IReservationService _reservationService;
        private readonly IPaymentProcessor _payment;
        private readonly INotify _notify;
        private readonly INotificationBuilder _notifyBuilder;
        private readonly ISettingService _settings;
        private readonly IPaymentTransactionService _transactions;
        private readonly IInvoiceService _invoices;

        public ReservationCancelCommandHandler(IReservationService reservationService, IPaymentProcessor payment, INotify notify, INotificationBuilder notifyBuilder, ISettingService settings, IPaymentTransactionService transactions, IInvoiceService invoices) {
            _reservationService = reservationService;
            _payment = payment;
            _notify = notify;
            _notifyBuilder = notifyBuilder;
            _settings = settings;
            _transactions = transactions;
            _invoices = invoices;
        }

        public void Handle(ReservationCancelCommand command) {
            command.OutCancelStatus = ReservationCancelResponseState.Failed;
            bool cancelled = false;
            int cancelLeadTime = _settings.GetValue<int>(Setting.Cancel_Lead_Time_Hours);
            decimal cancellationFee = _settings.GetValue<int>(Setting.ReservationCancelFee);
            var reservation = _reservationService.GetById(command.ReservationId);
            var invoice = _invoices.GetByReservationId(reservation.Id);
            var transaction = _transactions.GetByInvoiceId(invoice.Id);

            if (!command.IgnoreLeadTime) {
                if (reservation.PickupDate <= DateTime.Now.AddHours(cancelLeadTime)) {
                    command.OutCancelStatus = ReservationCancelResponseState.NoLeadTime;
                    return;
                    //throw new Exception(string.Format("Less than {0} lead time", cancelLeadTime));
                }
            }
            if (reservation.IsCancelled) {
                command.OutCancelStatus = ReservationCancelResponseState.AlreadyCancelled;
                return;
                //throw new Exception("Reservation has already been cancelled");
            }
            decimal refundedAmount = 0m;

            //if (reservation.Invoice.AmountPaid > 0) {
            //    RefundRequestModel refundRequest = new RefundRequestModel();
            //    refundRequest.ReservationId = reservation.Id;
            //    refundRequest.InvoiceId = reservation.Invoice.Id;
            //    refundRequest.TransactionId = transaction.TransactionId;

            //    bool processRefund = false;
            //    if (reservation.Invoice.AmountPaid < cancellationFee) {
            //        //Keep whatever has been paid in lieu of full cancellation fee
            //        cancellationFee = reservation.Invoice.AmountPaid;
            //        processRefund = false;
            //    } else {
            //        refundRequest.Amount = reservation.Invoice.AmountPaid - cancellationFee;
            //        refundedAmount = refundRequest.Amount;
            //        processRefund = true;
            //    }
            //    //TODO - PayGate Implementation
            //    processRefund = false;
            //    if (processRefund) {
            //        var refundResult = _payment.ProcessRefund(refundRequest);

            //        if (refundResult.Processed) {
            //            _reservationService.Cancel(command.ReservationId, command.IsCancelled, cancellationFee, refundedAmount);
                        
            //            cancelled = true;
            //        }
            //    } else {
            //        _reservationService.Cancel(command.ReservationId, command.IsCancelled, cancellationFee, refundedAmount);
                    
            //        cancelled = true;
            //    }
                
            //} else {
                //Can't take fee or give refund so just cancel
                cancellationFee = 0m;
                refundedAmount = 0m;
                _reservationService.Cancel(command.ReservationId, command.IsCancelled, cancellationFee, refundedAmount);
                
                cancelled = true;
            //}
            
            if (cancelled) {
                command.OutCancelStatus = ReservationCancelResponseState.Success;
                command.OutStatus = true;
                //Notify user
                ReservationInvoiceNotificationModel notifyModel = new ReservationInvoiceNotificationModel();
                notifyModel.Reservation = reservation;
                notifyModel.CancellationFee = cancellationFee;
                notifyModel.RefundedAmount = refundedAmount;

                _notify.SendNotifyUserOfReservationCancel(notifyModel, Setting.Public_Website_Domain);

                //Notify admin
                _notify.SendNotifyAdminOfReservationCancel(notifyModel, Setting.Public_Website_Domain);


                //Notify user via SMS
                _notify.SendNotifyReservationCancelledSMS(notifyModel);

            }
            
        }
    }
}
