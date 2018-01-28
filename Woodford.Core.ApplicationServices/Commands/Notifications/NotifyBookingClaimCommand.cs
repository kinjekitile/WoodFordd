using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Facades;

namespace Woodford.Core.ApplicationServices.Commands {
    public class NotifyBookingClaimCommand : ICommand {
        public BookingClaimModel Claim { get; set; }
        
    }

    public class NotifyBookingClaimCommandHandler : ICommandHandler<NotifyBookingClaimCommand> {
        private readonly INotify _notify;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;
        private readonly INotificationBuilder _notificationBuilder;
        private readonly IBranchService _branchService;

        public NotifyBookingClaimCommandHandler(INotify notify, ISettingService settingService, IUserService userService, IBranchService branchService, INotificationBuilder notificationBuilder) {
            _notify = notify;
            _settingService = settingService;
            _branchService = branchService;
            _userService = userService;
            _notificationBuilder = notificationBuilder;
        }

        public void Handle(NotifyBookingClaimCommand command) {

            BookingClaimNotificationModel emailModel = new BookingClaimNotificationModel();
            var user = _userService.GetById(command.Claim.UserId);
            var branch = _branchService.GetById(command.Claim.BookingPickupBranchId);
            emailModel.Claim = command.Claim;
            emailModel.User = user;
            emailModel.Claim.PickupBranch = branch;
            emailModel.AdminSiteDomain = _settingService.GetValue<string>(Setting.Admin_Website_Domain);


            _notify.SendNotifyAdminBookingClaim(emailModel, Setting.Public_Website_Domain);
        }
    }
}
