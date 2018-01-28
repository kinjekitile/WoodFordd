using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;
using System.Diagnostics;

using System.Text;
using System.Threading.Tasks;
using System.Timers;

using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Diagnostics;
using Quartz;
using Quartz.Impl;

using Quartz.Spi;

namespace Woodford.Infrastructure.TaskScheduler {
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
			throw new NotImplementedException();
		}
	}
}
