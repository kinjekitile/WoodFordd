using System;
using System.Collections;
using System.Linq;
using Quartz;
using Woodford.Core.Interfaces;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Public.Code.Helpers;
using Woodford.Core.Interfaces.Facades;

namespace Woodford.UI.Web.Public.TaskScheduler.Jobs {

    [DisallowConcurrentExecution]
    public class SendLoyaltyPointsEarnedNotificationJob : IJob {
        
        private IBookingHistoryService _bookingHistoryService;
        private INotify _notify;
        private INotificationBuilder _notificationBuilder;

        public SendLoyaltyPointsEarnedNotificationJob(IBookingHistoryService bookingHistoryService, INotify notify, INotificationBuilder notificationBuilder) {
            _bookingHistoryService = bookingHistoryService;
            _notify = notify;
            _notificationBuilder = notificationBuilder;
        }

        public void Execute(IJobExecutionContext context) {
            var bookings = _bookingHistoryService.Get(new BookingHistoryFilterModel { LoyaltyPointsEarnedEmailSent = false, SendLoyaltyPointsEmail = true }, null).Items;

            foreach (var booking in bookings) {
                if (booking.LoyaltyPointsEarned.HasValue) {
                    if (booking.LoyaltyPointsEarned.Value > 0) {
                        if (booking.UserId.HasValue) {
                           
                            LoyaltyPointsEarnedNotificationModel emailModel = new LoyaltyPointsEarnedNotificationModel();
                            emailModel = _notificationBuilder.BuildLoyaltyPointsEarnedModel(booking);

                            _bookingHistoryService.SetLoyaltyPointsEmailSent(booking.Id, true);

                            _notify.SendNotifyLoyaltyPointsEarned(emailModel, Setting.Public_Website_Domain);
                            _notify.SendNotifyLoyaltyPointsEarnedSMS(emailModel, Setting.Public_Website_Domain);

                        }
                    }
                }
            }
            
        }
    }
}