using System;
using Quartz;
using Woodford.Core.Interfaces;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.Interfaces.Providers;
using System.Linq;
using Woodford.UI.Web.Public.Code.Helpers;
using Woodford.Core.Interfaces.Facades;

namespace Woodford.UI.Web.Public.TaskScheduler.Jobs {
    [DisallowConcurrentExecution]
    public class SendReservationReminderJob : IJob {

        private IReservationService _reservationService;
        private INotify _notify;
        private ISettingService _settings;
        private INotificationBuilder _notificationBuilder;

        public SendReservationReminderJob(IReservationService reservationService, INotify notify, ISettingService settings, INotificationBuilder notificationBuilder) {

            _reservationService = reservationService;
            _notify = notify;
            _settings = settings;
            _notificationBuilder = notificationBuilder;
        }
        
      
        public void Execute(IJobExecutionContext context) {
            int hoursBeforePickupTime = _settings.GetValue<int>(Setting.ReservationReminderHoursBeforeEmail);

            var reservations = _reservationService.Get(new ReservationFilterModel { DateFilterType = ReservationDateFilterTypes.PickupDate, DateSearchStart = DateTime.Today, DateSearchEnd = DateTime.Today.AddDays(1), IsCompletedInvoice = true, ReminderEmailSent = false, IsCancelled = false }, null).Items;

            foreach (var item in reservations) {

                if (item.PickupDate > DateTime.Now) {
                    ReservationInvoiceNotificationModel model = _notificationBuilder.BuildReservationReminderModel(item.Id);

                    _notify.SendNotifyReservationReminder(model, Setting.Public_Website_Domain);
                    _reservationService.SetReminderEmailSent(item.Id, reminderSent: true);

                }


            }


        }
    }
}