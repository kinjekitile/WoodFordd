using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.TaskScheduler.Jobs {
    public class NotificationsQueueProcessorJob : IJob {

        private readonly INotificationQueueService _queue;

        public NotificationsQueueProcessorJob(INotificationQueueService queue) {
            _queue = queue;
        }

        public void Execute(IJobExecutionContext context) {

            _queue.ProcessNotificationQueue();

            

            //INotificationQueueService serv = new INotificationQueueService();
            //serv.ProcessNotificationQueue();

        }
    }
}
