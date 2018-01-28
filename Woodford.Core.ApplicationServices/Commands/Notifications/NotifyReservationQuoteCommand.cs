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
    public class NotifyReservationQuoteCommand : ICommand {
        public int ReservationId { get; set; }
        public int CheckoutStep { get; set; }
    }

    public class NotifyReservationQuoteCommandHandler : ICommandHandler<NotifyReservationQuoteCommand> {
        private readonly INotify _notify;
        private readonly ISettingService _settingService;
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleUpgradeService _upgradeService;
        private readonly INotificationBuilder _notificationBuilder;

        public NotifyReservationQuoteCommandHandler(INotify notify, ISettingService settingService, IReservationService reservationService, IUserService userService, IVehicleService vehicleService, IVehicleUpgradeService upgradeService, INotificationBuilder notificationBuilder) {
            _notify = notify;
            _settingService = settingService;
            _reservationService = reservationService;
            _userService = userService;
            _vehicleService = vehicleService;
            _upgradeService = upgradeService;
            _notificationBuilder = notificationBuilder;
        }

        public void Handle(NotifyReservationQuoteCommand command) {

            var reservation = _reservationService.GetById(command.ReservationId);
            reservation.QuoteSentDate = DateTime.Today;
            _reservationService.Update(reservation);

            //reservation.Vehicle = _vehicleService.GetById(reservation.VehicleId, includePageContent: false);
            //if (reservation.VehicleUpgradeId.HasValue) {
            //    var upgrade = _upgradeService.GetById(reservation.VehicleUpgradeId.Value);
            //    reservation.VehicleUpgrade = _vehicleService.GetById(upgrade.ToVehicleId, includePageContent: false);
            //}

            ReservationInvoiceNotificationModel emailModel = new ReservationInvoiceNotificationModel();
            emailModel = _notificationBuilder.BuildReservationInvoiceModel(reservation.Id);
            //emailModel.Reservation = reservation;
            emailModel.SiteDomain = _settingService.GetValue<string>(Setting.Public_Website_Domain);
            if (command.CheckoutStep == 2) {
                emailModel.QuoteSentStep = 2;
            }
            _notify.SendNotifyReservationQuote(emailModel, Setting.Public_Website_Domain);
        }
    }
}
