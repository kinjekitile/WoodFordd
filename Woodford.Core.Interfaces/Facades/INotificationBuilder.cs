using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces.Facades {
    public interface INotificationBuilder {
        ReservationInvoiceNotificationModel BuildReservationInvoiceModel(int reservationId);
        LoyaltyPointsSpentNotificationModel BuildLoyaltyPointsSpentModel(int reservationId);
        LoyaltyPointsEarnedNotificationModel BuildLoyaltyPointsEarnedModel(BookingHistoryModel booking);
        ReservationInvoiceNotificationModel BuildReservationThankYouModel(int reservationId);

        ReservationInvoiceNotificationModel BuildReservationReminderModel(int reservationId);
        List<AdvanceReportNewsAndCampaignItem> BuildNewsAndCampaignItems();
        AdvanceReportNotificationModel BuildLoyaltyReport(UserModel user, List<AdvanceReportNewsAndCampaignItem> newsAndCampaigns);
    }
}
