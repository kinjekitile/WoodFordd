using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {

    public enum Setting {
        [SiteSettingDefaultValue("http://localhost")]
        Public_Website_Domain,
        [SiteSettingDefaultValue("http://localhost")]
        Admin_Website_Domain,
        [SiteSettingDefaultValue("webadmin@woodford.co.za")]
        Email_Admin_Bcc_Addresses,
        [SiteSettingDefaultValue("info@woodford.co.za")]
        Email_From_Address,
        [SiteSettingDefaultValue("Contact received via the Woodford website")]
        Contact_Subject,
        [SiteSettingDefaultValue("info@woodford.co.za")]
        Contact_Email_Address,
        [SiteSettingDefaultValue("Password reset request from the Woodford website")]
        Password_Reset_Subject,
        [SiteSettingDefaultValue("3")]
        Email_Notification_Try_Times,
        [SiteSettingDefaultValue("10")]
        Email_Notifications_Number_To_Process,
        [SiteSettingDefaultValue(".jpg,.jpeg,.png,.gif")]
        Accepted_Image_Types,
        [SiteSettingDefaultValue("90")]
        Resize_Image_Save_Quality,
        [SiteSettingDefaultValue("1024")]
        Image_Max_Length,
        [SiteSettingDefaultValue("7")]
        Booking_Lead_Time_Hours,
        [SiteSettingDefaultValue("6")]
        Cancel_Lead_Time_Hours,
        [SiteSettingDefaultValue("1")]
        Default_Location_Id,
        [SiteSettingDefaultValue("14")]
        Tax_Rate_Non_Airport,
        [SiteSettingDefaultValue("9")]
        Tax_Rate_Airport,
        [SiteSettingDefaultValue("50")]
        Booking_Admin_Fee,
        [SiteSettingDefaultValue("1000")]
        Rental_Deposit_Amount,
        [SiteSettingDefaultValue("reservations@woodford.co.za")]
        Email_Reservations_Admin_Cc_Addresses,
        [SiteSettingDefaultValue("")]
        Payment_Gateway_Merchant_Id,
        [SiteSettingDefaultValue("")]
        Payment_Gateway_Password,
        [SiteSettingDefaultValue("")]
        Payment_Gateway_Application_Id,
        [SiteSettingDefaultValue("")]
        Payment_Gateway_Gateway_Id,
        [SiteSettingDefaultValue("")]
        Payment_Gateway_Mode,
        [SiteSettingDefaultValue("")]
        Payment_Gateway_Currency,
        [SiteSettingDefaultValue("true")]
        Payment_Gateway_Use_3D_Secure,
        [SiteSettingDefaultValue("")]
        Payment_3D_Secure_Callback_URL,
        [SiteSettingDefaultValue("C:\\Work\\FakeFtp\\WoodfordDataExport\\")]
        DataExportLocalPath,
        [SiteSettingDefaultValue("1")]
        Countdown_Special_Cookie_Days,
        [SiteSettingDefaultValue("10")]
        Countdown_Special_Minutes,
        [SiteSettingDefaultValue("Woodford Reservation")]
        Reservation_Subject,
        [SiteSettingDefaultValue("Woodford Reservation - RESERVATION MODIFIED")]
        Reservation_Changed_Subject,
        [SiteSettingDefaultValue("Woodford Reservation - RESERVATION CANCELLED")]
        Reservation_Cancelled_Subject,
        [SiteSettingDefaultValue("Woodford Reservation Quote")]
        Reservation_Quote_Subject,
        [SiteSettingDefaultValue("Woodford Reservation Reminder")]
        ReservationReminder_Subject,
        [SiteSettingDefaultValue("Woodford Reservation Thank You")]
        ReservationThankYou_Subject,
        [SiteSettingDefaultValue("Woodford Advance Loyalty Report")]
        AdvanceLoyaltyReport_Subject,
        [SiteSettingDefaultValue("Woodford Account")]
        Account_Register_Subject,
        [SiteSettingDefaultValue("2")]
        ReservationReminderHoursBeforeEmail,
        [SiteSettingDefaultValue("1")]
        ReservationThanksDaysAfterDropoff,
        [SiteSettingDefaultValue("Woodford Advance Account Created")]
        Loyalty_Register_Subject,
        [SiteSettingDefaultValue("Woodford Advance Point Spent")]
        LoyaltyPointsSpent,
        [SiteSettingDefaultValue("Woodford Advance Point Earned")]
        LoyaltyPointsEarned,
        Voucher_Subject,
        [SiteSettingDefaultValue("4")]
        Reservation_Prefix_Length,
        [SiteSettingDefaultValue("C:\\Work\\FakeFtp\\WoodfordVouchers\\")]
        VoucherFileLocation,
        [SiteSettingDefaultValue("C:\\Work\\Projects\\Woodford2015\\Woodford.UI.Web.Admin\\Content\\images\\voucher-template.jpg")]
        VoucherTemplateLocation,
        [SiteSettingDefaultValue("http://api.clickatell.com/http/sendmsg?user=woodford2&amp;password=aSdOBIcWSRQQZa&amp;api_id=3409336&amp;to=##phonenumber##&amp;text=##message##")]
        SMSServiceUrl,
        [SiteSettingDefaultValue("True")]
        SendReservationSMS,
        [SiteSettingDefaultValue("True")]
        ProductionMode,
        [SiteSettingDefaultValue("cdebc6a0df36709e00e5ae236176f927")]
        WeatherApiAccountId,
        [SiteSettingDefaultValue("0b7cfc8b8f@invite.trustpilot.com")]
        Review_Invite_Bcc_Address,
        [SiteSettingDefaultValue("C:\\Sites\\www.woodford.co.za\\Content\\images\\email_signature")]
        EmailSignatureAssetsLocalPath,
        [SiteSettingDefaultValue("250")]
        ReservationCancelFee,
        [SiteSettingDefaultValue("6")]
        ReservationCancelNoticeHours,
        [SiteSettingDefaultValue("Woodford User Booking Claim")]
        ReservationBookingClaimSubject,
        [SiteSettingDefaultValue("https://api.trustpilot.com/v1/oauth/oauth-business-users-for-applications/accesstoken")]
        ReviewServiceAPIAuthUrl,
        [SiteSettingDefaultValue("https://invitations-api.trustpilot.com/v1/private/business-units/")]
        ReviewServiceAPIEndpoint,
        [SiteSettingDefaultValue("54620ca500006400057b77fe")]
        ReviewServiceBusinessUnitId,
        [SiteSettingDefaultValue("C:\\Sites\\admin.woodford.co.za\\EmailSignatures\\output\\")]
        EmailSignatureFolderOutputLocation,
        [SiteSettingDefaultValue("C:\\Sites\\admin.woodford.co.za\\EmailSignatures\\templates\\")]
        EmailSignatureTemplatesLocation
    }

    public class SettingModel {
        public Setting Type { get; set; }
        public string Value { get; set; }
    }

    public class SiteSettingDefaultValue : Attribute {
        private string _defaultValue;
        public string DefaultValue { get { return _defaultValue; } }
        public SiteSettingDefaultValue(string defaultValue) {
            _defaultValue = defaultValue;
        }
    }
}
