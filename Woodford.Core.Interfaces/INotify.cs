using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
	public interface INotify {
		
        //void SendMail(string to, string from, string subject, object model, NotificationResources notificationResource, Setting domainSetting, string keyFormat = "##{0}##", string cc = "", string bcc = "");

        void SendMail(string to, string subject, string emailBody, string cc = "", string bcc = "");
        void SendSMS(string toMobileNumber, string textBody);
        void SendNotifyReservationInvoiceSMS(ReservationInvoiceNotificationModel model);
        void SendNotifyReservationReminderSMS(ReservationInvoiceNotificationModel model);
        void SendNotifyReservationCancelledSMS(ReservationInvoiceNotificationModel model);

        void SendNotifyContactUs(ContactUsNotificationModel model, Setting domainSetting);

        void SendNotifyRequestCallback(RequestCallbackNotificationModel model, Setting domainSetting);

        void SendNotifyUserPasswordReset(ForgotPasswordNotificationModel model, Setting domainSetting);

        void SendNotifyReservationInvoice(ReservationInvoiceNotificationModel model, Setting domainSetting);

        void SendNotifyAdminOfReservationChange(ReservationInvoiceNotificationModel model, Setting domainSetting);
        void SendNotifyAdminOfReservationCancel(ReservationInvoiceNotificationModel model, Setting domainSetting);
        void SendNotifyUserOfReservationCancel(ReservationInvoiceNotificationModel model, Setting domainSetting);
        void SendNotifyAccountCreated(UserRegistrationNotifcationModel model, Setting domainSetting);

        void SendNotifyReservationReminder(ReservationInvoiceNotificationModel model, Setting domainSetting);
        void SendNotifyReservationThanks(ReservationInvoiceNotificationModel model, Setting domainSetting);

        void SendNotifyLoyaltyAccountCreatedByAdmin(UserRegistrationNotifcationModel model, Setting domainSetting);
        void SendNotifyLoyaltyPointsEarned(LoyaltyPointsEarnedNotificationModel model, Setting domainSetting);
        void SendNotifyLoyaltyPointsEarnedSMS(LoyaltyPointsEarnedNotificationModel model, Setting domainSetting);
        void SendNotifyLoyaltyPointsSpent(LoyaltyPointsSpentNotificationModel model, Setting domainSetting);
        void SendNotifyLoyaltyPointsSpentSMS(LoyaltyPointsSpentNotificationModel model, Setting domainSetting);
        void SendNotifyReservationQuote(ReservationInvoiceNotificationModel model, Setting domainSetting);

        void SendNotifyLoyaltyReport(AdvanceReportNotificationModel model, Setting domainSetting);

        void SendNotifyUserVoucher(UserVoucherNotificationModel model, Setting domainSetting);
        void SendNotifyAccountCreatedSMS(UserRegistrationNotifcationModel model, Setting domainSetting);
        void SendNotifyAdminBookingClaim(BookingClaimNotificationModel model, Setting domainSetting);

        void SendNotifyEmployeeOfSignature(EmailSignatureToUserNotificationModel model, Setting domainSetting);

        void SendNotifyLoyaltyMontlyReport(AdvanceReportNotificationModel model, Setting domainSetting);
    }
}
