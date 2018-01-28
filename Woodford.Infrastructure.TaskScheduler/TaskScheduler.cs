using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Infrastructure.TaskScheduler.Jobs;

namespace Woodford.Infrastructure.TaskScheduler
{
    public class TaskScheduler : ITaskScheduler {

        //private readonly INotify _notify;
        //private readonly ISettingService _settings;

        public TaskScheduler() {
            //_notify = notify;
            //_settings = settings;
        }

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

            sched.Start();


            //private static ISchedulerFactory schedFact = new StdSchedulerFactory();

            //private static IScheduler sched;
        }

        
    }
}
