using System;
using Quartz;
using Woodford.Core.Interfaces;

namespace Woodford.UI.Web.Public.TaskScheduler.Jobs {
    [DisallowConcurrentExecution]
    public class AssignEarnedLoyaltyPointsJob : IJob {
        private IUserService _userService;
        private ILoyaltyService _loyaltyService;
        public AssignEarnedLoyaltyPointsJob(IUserService userService, ILoyaltyService loyaltyService) {
            _userService = userService;
            _loyaltyService = loyaltyService;
        }
        public void Execute(IJobExecutionContext context) {
            
            throw new NotImplementedException();
        }
    }
}