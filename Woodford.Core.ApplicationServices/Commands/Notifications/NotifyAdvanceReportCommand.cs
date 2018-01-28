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
    public class NotifyAdvanceReportCommand : ICommand {
        public int ReservationId { get; set; }
        public int CheckoutStep { get; set; }
    }

    public class NotifyAdvanceReportCommandHandler : ICommandHandler<NotifyAdvanceReportCommand> {
        private readonly INotify _notify;
        private readonly ISettingService _settings;
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;

        private readonly INotificationBuilder _notificationBuilder;

        public NotifyAdvanceReportCommandHandler(INotify notify, ISettingService settingService, IReservationService reservationService, INotificationBuilder notificationBuilder) {
            _notify = notify;
            _settings = settingService;
            _reservationService = reservationService;
 
            _notificationBuilder = notificationBuilder;
        }

        public void Handle(NotifyAdvanceReportCommand command) {
            ////get all the user records
            //var users = _userService.Get(new UserFilterModel { IsLoyaltyMember = true }, null).Items;

            ////get all reservations for period in memory
            //var reservations = _reservationService.Get(new ReservationFilterModel {  DateFilterType = ReservationDateFilterTypes.PickupDate })

            //foreach (var user in users) {

            //}
            throw new NotImplementedException();
        }
    }
}
