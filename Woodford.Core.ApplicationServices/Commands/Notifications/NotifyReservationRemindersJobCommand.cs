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
    public class NotifyReservationRemindersJobCommand : ICommand {
        public int ReservationId { get; set; }
        public int CheckoutStep { get; set; }
    }

    public class NotifyReservationRemindersJobCommandHandler : ICommandHandler<NotifyReservationRemindersJobCommand> {
        private readonly INotify _notify;
        private readonly ISettingService _settings;
        private readonly IReservationService _reservationService;
   
        private readonly INotificationBuilder _notificationBuilder;

        public NotifyReservationRemindersJobCommandHandler(INotify notify, ISettingService settingService, IReservationService reservationService, INotificationBuilder notificationBuilder) {
            _notify = notify;
            _settings = settingService;
            _reservationService = reservationService;
 
            _notificationBuilder = notificationBuilder;
        }

        public void Handle(NotifyReservationRemindersJobCommand command) {

            int hoursBeforePickupTime = _settings.GetValue<int>(Setting.ReservationReminderHoursBeforeEmail);

            var reservations = _reservationService.Get(new ReservationFilterModel { DateFilterType = ReservationDateFilterTypes.PickupDate, DateSearchStart = DateTime.Today, DateSearchEnd = DateTime.Today.AddDays(1), IsCompletedInvoice = true, ReminderEmailSent = false }, null).Items;

            foreach (var item in reservations) {

                ReservationInvoiceNotificationModel model = _notificationBuilder.BuildReservationReminderModel(item.Id);

                _notify.SendNotifyReservationReminder(model, Setting.Public_Website_Domain);
                _reservationService.SetReminderEmailSent(item.Id, reminderSent: true);

            }
        }
    }
}
