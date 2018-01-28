using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Notifications {
    public class EmailNotificationService : INotify {

        private ISettingService _settings;
        private IFilePath _filePath;
        private string _resourceFolder;
        private INotificationQueueService _queue;

        public EmailNotificationService(ISettingService settings, INotificationQueueService queue, IFilePath filePath) {
            _settings = settings;
            _filePath = filePath;
            //_resourceFolder =  HttpContext.Current.Server.MapPath(_filePath.GetEmailResourcesPath()); //.GetFilePath();

            _resourceFolder = _filePath.GetEmailResourcesPath();
            _queue = queue;
        }

        private string getEmailResource(NotificationResources resource, string resourceExtension = ".txt") {
            resourceExtension = resourceExtension.StartsWith(".") ? resourceExtension : "." + resourceExtension;
            string filePath = Path.Combine(_resourceFolder, resource.ToString() + resourceExtension);
            if (Directory.Exists(_resourceFolder) && File.Exists(filePath)) {
                using (StreamReader reader = new StreamReader(filePath)) {
                    string header = reader.ReadLine();

                    string result = "";
                    if (!string.IsNullOrEmpty(header)) {
                        if (!header.Contains("@model")) {
                            result = header;
                        }
                    }
                    string line = "";
                    while ((line = reader.ReadLine()) != null) {
                        result = result + line;
                    }
                    return result;
                }
            }
            else {
                throw new Exception("Resource Location could not be found: " + filePath);
            }

            throw new NotImplementedException();
        }

        private string getSMSBody(NotificationResources resource, object model) {

            string bodyHtml = getEmailResource(resource, ".cshtml");

            string htmlTemplate = RazorEngine.Razor.Parse(bodyHtml, model);

            return htmlTemplate;
        }
        private string getEmailBody(NotificationResources resource, Setting domainSetting, object model) {
            string containerHtml = getEmailResource(NotificationResources.Container, ".html");
            string bodyHtml = getEmailResource(resource, ".cshtml");

            string htmlTemplate = RazorEngine.Razor.Parse(bodyHtml, model);

            string siteDomain = _settings.GetValue(domainSetting);
            containerHtml = containerHtml.Replace("##sitedomain##", siteDomain);

            bodyHtml = containerHtml.Replace(string.Format("##{0}##", "emailcontent"), htmlTemplate);

            return bodyHtml;
        }

        public void SendNotifyContactUs(ContactUsNotificationModel model, Setting domainSetting) {
            string emailBody = getEmailBody(NotificationResources.ContactUs, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = _settings.GetValue(Setting.Contact_Email_Address);
            string subject = _settings.GetValue(Setting.Contact_Subject);

            string adminBccEmailAddresses = _settings.Get(Setting.Email_Admin_Bcc_Addresses).Value;
            string bcc = adminBccEmailAddresses;

            _queue.Create(to, from, subject, emailBody, "Contact Us", string.Empty, bcc);
        }

        public void SendNotifyUserPasswordReset(ForgotPasswordNotificationModel model, Setting domainSetting) {

            string emailBody = getEmailBody(NotificationResources.ForgotPassword, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.Email;
            string subject = _settings.GetValue(Setting.Password_Reset_Subject);

            string adminBccEmailAddresses = _settings.Get(Setting.Email_Admin_Bcc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "User Password Reset", string.Empty, adminBccEmailAddresses);
        }
        
        public void SendMail(string to, string subject, string emailBody, string cc = "", string bcc = "") {
            Mailer.SendMail(to, subject, emailBody, cc, bcc);
        }

        public void SendNotifyReservationInvoice(ReservationInvoiceNotificationModel model, Setting domainSetting) {

            string emailBody = getEmailBody(NotificationResources.ReservationInvoice, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.Reservation.Email;
            string subject = _settings.GetValue(Setting.Reservation_Subject);
            

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;
            string senderProcess = "Reservation Checkout";
            if (model.NotifyAdminOfModification) {
                subject = _settings.GetValue(Setting.Reservation_Changed_Subject);
                senderProcess = "Reservation Modified";
            }
            _queue.Create(to, from, subject, emailBody, senderProcess, adminCC, String.Empty);

        }

        public void SendNotifyAdminOfReservationChange(ReservationInvoiceNotificationModel model, Setting domainSetting) {

            string emailBody = getEmailBody(NotificationResources.ReservationInvoice, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = _settings.GetValue(Setting.Email_Reservations_Admin_Cc_Addresses);
            string subject = _settings.GetValue(Setting.Reservation_Changed_Subject);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "Reservation Changed", adminCC, string.Empty);

        }

        public void SendNotifyReservationQuote(ReservationInvoiceNotificationModel model, Setting domainSetting) {

            string emailBody = getEmailBody(NotificationResources.ReservationQuote, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.Reservation.Email;
            string subject = _settings.GetValue(Setting.Reservation_Quote_Subject);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "Reservation Quote", adminCC, string.Empty);

        }

        public void SendNotifyReservationReminder(ReservationInvoiceNotificationModel model, Setting domainSetting) {
            string emailBody = getEmailBody(NotificationResources.ReservationReminder, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.Reservation.Email;
            string subject = _settings.GetValue(Setting.ReservationReminder_Subject);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            //This allows trust pilot to send an email to the users giving them the opportunity to create a review
            string reviewBcc = _settings.GetValue<string>(Setting.Review_Invite_Bcc_Address);
            //We now use the api to create a review link which we add to the thank you email
            reviewBcc = String.Empty;


            _queue.Create(to, from, subject, emailBody, "Reservation Reminder", adminCC, bcc: reviewBcc);
        }

        public void SendNotifyReservationThanks(ReservationInvoiceNotificationModel model, Setting domainSetting) {
            string emailBody = getEmailBody(NotificationResources.ReservationThankYou, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.Reservation.Email;
            string subject = _settings.GetValue(Setting.ReservationThankYou_Subject);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "Thank you", adminCC, string.Empty);
        }

        public void SendNotifyLoyaltyMontlyReport(AdvanceReportNotificationModel model, Setting domainSetting) {
            string emailBody = getEmailBody(NotificationResources.AdvanceReport, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.User.Email;
            string subject = _settings.GetValue(Setting.AdvanceLoyaltyReport_Subject);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "Advance Report", adminCC, string.Empty);
        }


        public void SendNotifyRequestCallback(RequestCallbackNotificationModel model, Setting domainSetting) {
            string emailBody = getEmailBody(NotificationResources.RequestCallback, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = _settings.GetValue(Setting.Contact_Email_Address);
            string subject = _settings.GetValue(Setting.Contact_Subject);

            string adminBccEmailAddresses = _settings.Get(Setting.Email_Admin_Bcc_Addresses).Value;
            string bcc = adminBccEmailAddresses;

            _queue.Create(to, from, subject, emailBody, "Request Callback", string.Empty, bcc);
        }

        public void SendNotifyAccountCreated(UserRegistrationNotifcationModel model, Setting domainSetting) {
            string emailBody = "";

            if (model.IsAdminGenerated) {
                emailBody = getEmailBody(NotificationResources.AdminUserRegistration, domainSetting, model);
            }
            else {
                emailBody = getEmailBody(NotificationResources.UserRegistration, domainSetting, model);
            }
            model.SiteDomain = _settings.GetValue(domainSetting);

            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.User.Email;
            string subject = _settings.GetValue(Setting.Account_Register_Subject);

            string adminBccEmailAddresses = _settings.Get(Setting.Email_Admin_Bcc_Addresses).Value;
            string bcc = adminBccEmailAddresses;

            _queue.Create(to, from, subject, emailBody, "Account Registration", string.Empty, bcc);
        }

        public void SendNotifyAccountCreatedSMS(UserRegistrationNotifcationModel model, Setting domainSetting) {
            string smsBody = "";


            smsBody = getSMSBody(NotificationResources.SMSUserRegistration, model);
            SendSMS(model.User.MobileNumber, smsBody);
        }

        public void SendNotifyLoyaltyAccountCreatedByAdmin(UserRegistrationNotifcationModel model, Setting domainSetting) {
            string emailBody = "";

            model.SiteDomain = _settings.GetValue(domainSetting);

            emailBody = getEmailBody(NotificationResources.LoyaltyUserRegistration, domainSetting, model);
            
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.User.Email;
            string subject = _settings.GetValue(Setting.Loyalty_Register_Subject);

            string adminBccEmailAddresses = _settings.Get(Setting.Email_Admin_Bcc_Addresses).Value;
            string bcc = adminBccEmailAddresses;

            _queue.Create(to, from, subject, emailBody, "Advance Account Registration", string.Empty, bcc);

        }


        public void SendNotifyLoyaltyPointsSpent(LoyaltyPointsSpentNotificationModel model, Setting domainSetting) {

            string emailBody = getEmailBody(NotificationResources.LoyaltyPointsSpent, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.User.Email;
            string subject = _settings.GetValue(Setting.LoyaltyPointsSpent);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "Loyalty Points Spent", adminCC, string.Empty);

        }

        public void SendNotifyLoyaltyPointsSpentSMS(LoyaltyPointsSpentNotificationModel model, Setting domainSetting) {

            string smsBody = "";


            smsBody = getSMSBody(NotificationResources.SMSLoyaltyPointsSpent, model);
            SendSMS(model.MobileNumber, smsBody);

        }

        public void SendNotifyLoyaltyPointsEarned(LoyaltyPointsEarnedNotificationModel model, Setting domainSetting) {

            string emailBody = getEmailBody(NotificationResources.LoyaltyPointsEarned, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.User.Email;
            string subject = _settings.GetValue(Setting.LoyaltyPointsEarned);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "Loyalty Points Earned", adminCC, string.Empty);

        }

        public void SendNotifyLoyaltyPointsEarnedSMS(LoyaltyPointsEarnedNotificationModel model, Setting domainSetting) {

            string smsBody = "";


            smsBody = getSMSBody(NotificationResources.SMSLoyaltyPointsEarned, model);
            SendSMS(model.MobileNumber, smsBody);

        }

        public void SendNotifyUserVoucher(UserVoucherNotificationModel model, Setting domainSetting) {
            string emailBody = "";


            emailBody = getEmailBody(NotificationResources.UserVoucher, domainSetting, model);

            model.SiteDomain = _settings.GetValue(domainSetting);

            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.User.Email;
            string subject = _settings.GetValue(Setting.Voucher_Subject);

            string adminBccEmailAddresses = _settings.Get(Setting.Email_Admin_Bcc_Addresses).Value;
            string bcc = adminBccEmailAddresses;

            _queue.Create(to, from, subject, emailBody, "Woodford Voucher", string.Empty, bcc, attachmentPath: model.AttachmentPath);

        }

        private string formatMobileNumber(string mobile) {

            mobile = mobile.Replace(" ", "").Trim().Replace("(", "").Replace(")", "");

            List<string> prefixes = "083,082,0710,0711,0712,0713,0714,0715,0716,0717,0718,0719,072,073,074,0741,076,078,079,0810,0811,0812,0813,0814,0815,0817,0818,084".Split(',').ToArray().ToList();

            foreach (var prefix in prefixes) {
                if (mobile.StartsWith(prefix)) {
                    mobile = "27" + mobile.Substring(1);
                }
            }

            if (mobile.StartsWith("+27")) {
                mobile = mobile.Replace("+27", "27");
            }



            return mobile;
        }

        public void SendSMS(string toMobileNumber, string textBody) {
            if (!string.IsNullOrEmpty(toMobileNumber)) {
                string smsPostUrl = _settings.GetValue<string>(Setting.SMSServiceUrl);
                bool sendSMS = _settings.GetValue<bool>(Setting.SendReservationSMS);
                try {
                    toMobileNumber = formatMobileNumber(toMobileNumber);

                    if ((!string.IsNullOrEmpty(smsPostUrl))) {
                        smsPostUrl = smsPostUrl.Replace("##phonenumber##", toMobileNumber);
                        smsPostUrl = smsPostUrl.Replace("##message##", textBody);

                        if (sendSMS) {
                            using (WebClient wc = new WebClient()) {
                                Stream resp = wc.OpenRead(smsPostUrl);
                                using (var reader = new StreamReader(resp, Encoding.UTF8)) {
                                    string value = reader.ReadToEnd();
                                    // Do something with the value
                                }
                            }
                        }
                    }

                }
                catch (Exception) {

                }
            }


        }

        public void SendNotifyReservationInvoiceSMS(ReservationInvoiceNotificationModel model) {
            string smsBody = "";


            smsBody = getSMSBody(NotificationResources.SMSReservation, model);
            SendSMS(model.Reservation.MobileNumber, smsBody);
        }

        public void SendNotifyReservationReminderSMS(ReservationInvoiceNotificationModel model) {
            string smsBody = "";


            smsBody = getSMSBody(NotificationResources.SMSReservationReminder, model);
            SendSMS(model.Reservation.MobileNumber, smsBody);
        }

        public void SendNotifyReservationCancelledSMS(ReservationInvoiceNotificationModel model) {
            string smsBody = "";


            smsBody = getSMSBody(NotificationResources.SMSReservationCancelled, model);
            SendSMS(model.Reservation.MobileNumber, smsBody);
        }

        public void SendNotifyAdminOfReservationCancel(ReservationInvoiceNotificationModel model, Setting domainSetting) {
            string emailBody = getEmailBody(NotificationResources.ReservationCancelled, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = _settings.GetValue(Setting.Email_Reservations_Admin_Cc_Addresses);
            string subject = _settings.GetValue(Setting.Reservation_Cancelled_Subject);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "Reservation CANCELLED", adminCC, string.Empty);
        }

        public void SendNotifyUserOfReservationCancel(ReservationInvoiceNotificationModel model, Setting domainSetting) {
            string emailBody = getEmailBody(NotificationResources.ReservationCancelled, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.Reservation.Email;
            string subject = _settings.GetValue(Setting.Reservation_Cancelled_Subject);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "Reservation CANCELLED", adminCC, string.Empty);
        }

        public void SendNotifyAdminBookingClaim(BookingClaimNotificationModel model, Setting domainSetting) {
            string emailBody = getEmailBody(NotificationResources.BookingClaim, domainSetting, model);
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = _settings.GetValue(Setting.Email_Reservations_Admin_Cc_Addresses);
            string subject = _settings.GetValue(Setting.ReservationBookingClaimSubject);

            string adminCC = _settings.Get(Setting.Email_Reservations_Admin_Cc_Addresses).Value;

            _queue.Create(to, from, subject, emailBody, "Reservation BOOKING CLAIM", adminCC, string.Empty);
        }

        public void SendNotifyEmployeeOfSignature(EmailSignatureToUserNotificationModel model, Setting domainSetting) {
            string emailBody = "";


            emailBody = getEmailBody(NotificationResources.EmployeeSignature, domainSetting, model);

            model.SiteDomain = _settings.GetValue(domainSetting);

            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.Signature.Email;
            string subject = "Woodford: New Email Signature";

            
            string bcc = "";

            _queue.Create(to, from, subject, emailBody, "Woodford Voucher", string.Empty, bcc, attachmentPath: model.AttachmentPath);

        }

        public void SendNotifyLoyaltyReport(AdvanceReportNotificationModel model, Setting domainSetting) {

            string emailBody = "";


            emailBody = getEmailBody(NotificationResources.AdvanceReport, domainSetting, model);

            model.SiteDomain = _settings.GetValue(domainSetting);
            model.SiteDomain = "https://www.woodford.co.za/";
            string from = _settings.GetValue(Setting.Email_From_Address);
            string to = model.User.Email;
            string subject = "Woodford: Advance Loyalty Program";


            string bcc = "";

            _queue.Create(to, from, subject, emailBody, "Woodford Advance Report", string.Empty, bcc, attachmentPath: null);
        }
    }
}


