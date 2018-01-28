using Quartz;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.TaskScheduler.Jobs {
    [DisallowConcurrentExecution]
    public class NotificationsQueueProcessorJob : IJob {

        private readonly INotificationQueueService _queue;

        public NotificationsQueueProcessorJob(INotificationQueueService queue) {
            _queue = queue;
        }

        public void Execute(IJobExecutionContext context) {

            _queue.ProcessNotificationQueue();
            
        }
    }
}
