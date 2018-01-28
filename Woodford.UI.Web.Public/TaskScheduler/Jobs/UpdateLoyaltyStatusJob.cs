using System;
using Quartz;
using Woodford.Core.Interfaces;
using Woodford.Core.DomainModel.Models;

namespace Woodford.UI.Web.Public.TaskScheduler.Jobs {
    [DisallowConcurrentExecution]
    public class UpdateLoyaltyStatusJob : IJob {
        private IUserService _userService;
        private ILoyaltyService _loyaltyService;
        public UpdateLoyaltyStatusJob(IUserService userService, ILoyaltyService loyaltyService) {
            _userService = userService;
            _loyaltyService = loyaltyService;
        }
        public void Execute(IJobExecutionContext context) {
            //var users = _userService.Get(new UserFilterModel { IsLoyaltyMember = true }, null).Items;

            //foreach (var user in users) {
                
            //    if (user.LoyaltyPeriodEnd == DateTime.Today) {
            //        var loyaltyOverview = _userService.
            //    }
            //} 
            throw new NotImplementedException();
        }
    }
}