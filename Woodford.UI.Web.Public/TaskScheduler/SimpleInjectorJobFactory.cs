using System;
using SimpleInjector;
using Quartz;
using Quartz.Spi;

namespace Woodford.UI.Web.Public.TaskScheduler {
    public class SimpleInjectorJobFactory : IJobFactory {
		private readonly Container _container;
		public SimpleInjectorJobFactory(Container container) {
			_container = container;
		}

		public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) {
			try {
				IJobDetail jobDetail = bundle.JobDetail;
				Type jobType = jobDetail.JobType;

				return (IJob)_container.GetInstance(jobType);
			} catch (Exception ex) {
				throw new SchedulerException("Problem instantiating class", ex);
			}
		}

		public void ReturnJob(IJob job) {
			//throw new NotImplementedException();
		}
	}
}
