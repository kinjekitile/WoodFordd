using System;
using Quartz;
using Woodford.Core.Interfaces;
using Woodford.Core.DomainModel.Models;
using System.Linq;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.Interfaces.Facades;
using System.Collections.Generic;

namespace Woodford.UI.Web.Public.TaskScheduler.Jobs {
    public class SendLoyaltyReportJob : IJob {

        private readonly IUserService _userService;
        
        
        private readonly INotify _notify;
        private readonly ISettingService _settings;
        private readonly INotificationBuilder _notificationBuilder; 
        public SendLoyaltyReportJob(IUserService userService, ILoyaltyService loyaltyService, INotify notify, ISettingService settings, INotificationBuilder notificationBuilder) {
            _userService = userService;
            
            _notify = notify;
            _settings = settings;
            _notificationBuilder = notificationBuilder;
        }

        //LoyaltyOverviewByUserIdQuery
        public void Execute(IJobExecutionContext context) {
            string siteDomain = _settings.GetValue<string>(Setting.Public_Website_Domain);

            var newsAndCampaigns = _notificationBuilder.BuildNewsAndCampaignItems();
            
            int lastDayOfMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);

            if (DateTime.Today.Day == lastDayOfMonth) {
                //we send to all Advance Users
                var users = _userService.Get(new Core.DomainModel.Models.UserFilterModel { IsLoyaltyMember = true }, null).Items;
                //var users = _userService.Get(new Core.DomainModel.Models.UserFilterModel { Id = 15 }, null).Items;

                foreach (var user in users) {

                    var emailModel = _notificationBuilder.BuildLoyaltyReport(user, newsAndCampaigns);
                    _notify.SendNotifyLoyaltyReport(emailModel, Setting.Public_Website_Domain);

                }
            }

            if (DateTime.Today.Day == (lastDayOfMonth - 3)) {
                //we send to just Essa and Oliver

                var users = _userService.Get(new Core.DomainModel.Models.UserFilterModel { Id = 15 }, null).Items;
                foreach (var user in users) {

                    var emailModel = _notificationBuilder.BuildLoyaltyReport(user, newsAndCampaigns);
                    _notify.SendNotifyLoyaltyReport(emailModel, Setting.Public_Website_Domain);

                }

                users = _userService.Get(new Core.DomainModel.Models.UserFilterModel { Id = 16 }, null).Items;
                foreach (var user in users) {

                    var emailModel = _notificationBuilder.BuildLoyaltyReport(user, newsAndCampaigns);
                    _notify.SendNotifyLoyaltyReport(emailModel, Setting.Public_Website_Domain);

                }
            }


        }
    }
}