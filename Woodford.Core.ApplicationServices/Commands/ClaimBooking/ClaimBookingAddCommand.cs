using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ClaimBookingAddCommand : ICommand {
        public BookingClaimModel BookingClaim { get; set; }
    }

    public class ClaimBookingAddCommandHandler : ICommandHandler<ClaimBookingAddCommand> {
        private readonly IClaimBookingRepository _claimBookingRepo;
        private readonly INotify _notify;
        private readonly IUserService _userService;
        private readonly IBranchService _branchService;
        private readonly ISettingService _settingService;
        public ClaimBookingAddCommandHandler(IClaimBookingRepository claimBookingRepo, INotify notify, IUserService userService, IBranchService branchService, ISettingService settingService) {
            _claimBookingRepo = claimBookingRepo;
            _notify = notify;
            _userService = userService;
            _branchService = branchService;
            _settingService = settingService;
        }
        public void Handle(ClaimBookingAddCommand command) {
            command.BookingClaim.State = BookingClaimState.Pending;
            command.BookingClaim = _claimBookingRepo.Create(command.BookingClaim);

            //Notify Admin
            BookingClaimNotificationModel emailModel = new BookingClaimNotificationModel();
            var user = _userService.GetById(command.BookingClaim.UserId);
            var branch = _branchService.GetById(command.BookingClaim.BookingPickupBranchId);
            emailModel.Claim = command.BookingClaim;
            emailModel.User = user;
            emailModel.Claim.PickupBranch = branch;
            emailModel.AdminSiteDomain = _settingService.GetValue<string>(Setting.Admin_Website_Domain);


            _notify.SendNotifyAdminBookingClaim(emailModel, Setting.Public_Website_Domain);
        }
    }
}
