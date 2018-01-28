using Quartz;
using Quartz.Impl;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.TaskScheduler.Jobs;

namespace Woodford.UI.Web.Public.TaskScheduler {
    public class RepeatingTaskScheduler : ITaskScheduler {

        public void InitialiseTasks() {

            ISchedulerFactory schedFact = new StdSchedulerFactory();

            IScheduler sched = schedFact.GetScheduler();

            IJobDetail processEmailQueue = JobBuilder.Create<NotificationsQueueProcessorJob>()
                    .WithIdentity("processMailQueue", "notifications")
                    .Build();

            ITrigger processEmailQueueTrigger = TriggerBuilder.Create()
                .WithIdentity("processMailQueueTrigger", "notifications")
                .StartNow()
                .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(30)
                        .RepeatForever())
                .Build();

            sched.ScheduleJob(processEmailQueue, processEmailQueueTrigger);

            #region Reservation Reminders

            IJobDetail sendReservationReminders = JobBuilder.Create<SendReservationReminderJob>()
                   .WithIdentity("sendReservationReminders", "notifications")
                   .Build();

            ITrigger sendReservationRemindersTrigger = TriggerBuilder.Create()
                .WithIdentity("sendReservationRemindersTrigger", "notifications")
                .StartNow()
                .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(5)
                        .RepeatForever())
                .Build();

            sched.ScheduleJob(sendReservationReminders, sendReservationRemindersTrigger);


            IJobDetail sendReservationRemindersSMS = JobBuilder.Create<SendReservationReminderSMSJob>()
                   .WithIdentity("sendReservationRemindersSMS", "notifications")
                   .Build();

            ITrigger sendReservationRemindersSMSTrigger = TriggerBuilder.Create()
                .WithIdentity("sendReservationRemindersSMSTrigger", "notifications")
                .StartNow()
                .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(5)
                        .RepeatForever())
                .Build();

            sched.ScheduleJob(sendReservationRemindersSMS, sendReservationRemindersSMSTrigger);

            #endregion


            #region Quote Reminders

            IJobDetail sendQuoteReminders = JobBuilder.Create<SendQuoteReminderJob>()
                   .WithIdentity("sendQuoteReminders", "notifications")
                   .Build();

            ITrigger sendQuoteRemindersTrigger = TriggerBuilder.Create()
                .WithIdentity("sendQuoteRemindersTrigger", "notifications")
                .StartNow()
                .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(30)
                        .RepeatForever())
                .Build();

            sched.ScheduleJob(sendQuoteReminders, sendQuoteRemindersTrigger);


            #endregion


            #region Reservation Thanks

            IJobDetail sendReservationThanks = JobBuilder.Create<SendPostReservationThankYouJob>()
                   .WithIdentity("sendReservationThanks", "notifications")
                   .Build();

            ITrigger sendReservationThanksTrigger = TriggerBuilder.Create()
                .WithIdentity("sendReservationThanksTrigger", "notifications")
                .StartAt(DateBuilder.TodayAt(10, 00, 0))
                .WithSimpleSchedule(x => x
                        .WithIntervalInHours(24)
                        .RepeatForever())
                .Build();

            sched.ScheduleJob(sendReservationThanks, sendReservationThanksTrigger);

            #endregion

            #region Loyalty Points Earned Emailer

            IJobDetail sendLoyaltyPointsEarned = JobBuilder.Create<SendLoyaltyPointsEarnedNotificationJob>()
                   .WithIdentity("sendLoyaltyPointsEarned", "notifications")
                   .Build();

            ITrigger sendLoyaltyPointsEarnedTrigger = TriggerBuilder.Create()
                .WithIdentity("sendLoyaltyPointsEarnedTrigger", "notifications")
            .StartAt(DateBuilder.TodayAt(17, 00, 0))
            .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
            .Build();

            sched.ScheduleJob(sendLoyaltyPointsEarned, sendLoyaltyPointsEarnedTrigger);

            #endregion



            #region Loyalty Monthly Emailer

            IJobDetail sendLoyaltyReport = JobBuilder.Create<SendLoyaltyReportJob>()
                   .WithIdentity("sendLoyaltyReport", "notifications")
                   .Build();

            ITrigger sendLoyaltyReportTrigger = TriggerBuilder.Create()
                .WithIdentity("sendLoyaltyReportTrigger", "notifications")
            .StartAt(DateBuilder.TodayAt(10, 17, 0))
            .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
            .Build();

            sched.ScheduleJob(sendLoyaltyReport, sendLoyaltyReportTrigger);

            #endregion


            sched.Start();

        }


    }
}
