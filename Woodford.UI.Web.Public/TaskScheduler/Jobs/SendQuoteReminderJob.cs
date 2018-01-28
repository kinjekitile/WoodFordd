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
    public class SendQuoteReminderJob : IJob {
       
        private IReservationService _reservationService;
        private INotify _notify;
        private ISettingService _settings;
        private INotificationBuilder _notificationBuilder;

        public SendQuoteReminderJob(IReservationService reservationService, INotify notify, ISettingService settings, INotificationBuilder notificationBuilder) {
            
            _reservationService = reservationService;
            _notify = notify;
            _settings = settings;
            _notificationBuilder = notificationBuilder;
        }

        public void Execute(IJobExecutionContext context) {


            var items = _reservationService.Get(new ReservationFilterModel { QuoteHasBeenEmailed = true, DateCreated = DateTime.Today.AddDays(-3), QuoteReminderSent = false, IsQuote = true }, null).Items;

            foreach (var item in items) {
                if (item.PickupDate > DateTime.Today) {
                    //if quote pickup date already passed then do not send
                    if (item.DateCreated.AddDays(7) > DateTime.Today) {
                        //if quote has expired by being older than 7 days then do not send
                        ReservationInvoiceNotificationModel model = _notificationBuilder.BuildReservationInvoiceModel(item.Id);
                        model.IsQuoteReminder = true;
                        _notify.SendNotifyReservationQuote(model, Setting.Public_Website_Domain);
                        _reservationService.SetQuoteReminderSent(item.Id, reminderSent: true);
                    }
                    
                }
                

            }


        }
    }
}