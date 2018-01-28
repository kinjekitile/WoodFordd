using System;
using Quartz;
using Woodford.Core.Interfaces;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.Interfaces.Providers;
using System.Linq;

namespace Woodford.UI.Web.Public.TaskScheduler.Jobs {
    [DisallowConcurrentExecution]
    public class SendReservationReminderSMSJob : IJob {

        private IReservationService _reservationService;
        private INotify _notify;
        private IUserService _userService;
        private ISettingService _settings;


        public SendReservationReminderSMSJob(IReservationService reservationService, INotify notify, ISettingService settings, IUserService userService) {

            _reservationService = reservationService;
            _notify = notify;
            _userService = userService;
            _settings = settings;

        }

        public void Execute(IJobExecutionContext context) {
            int hoursBeforePickupTime = _settings.GetValue<int>(Setting.ReservationReminderHoursBeforeEmail);

            var reservations = _reservationService.Get(new ReservationFilterModel { DateFilterType = ReservationDateFilterTypes.PickupDate, DateSearchStart = DateTime.Today, DateSearchEnd = DateTime.Today.AddDays(1), IsCompletedInvoice = true, ReminderSMSSent = false, IsCancelled = false }, null).Items;

            foreach (var item in reservations) {

                //Only send if pickup not yet past
                if (item.PickupDate > DateTime.Now) {

                    //Check if reservation pickup time is less than or equal to Now add 2 hours
                    if (item.PickupDate <= DateTime.Now.AddHours(hoursBeforePickupTime)) {
                        //It is now time to send the reminder

                        ReservationInvoiceNotificationModel model = new ReservationInvoiceNotificationModel();
                        model.Reservation = item;
                        

                        _notify.SendNotifyReservationReminderSMS(model);
                        _reservationService.SetReminderSMSSent(item.Id, true);


                    }
                    else {
                        //The reservation will be re-processed in the next job run
                    }
                }



            }


        }
    }
}