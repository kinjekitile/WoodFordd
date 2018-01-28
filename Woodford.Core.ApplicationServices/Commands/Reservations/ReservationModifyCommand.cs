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
    public class ReservationModifyCommand : ICommand {
        public ReservationModel Reservation { get; set; }
    }

    public class ReservationModifyCommandHandler : ICommandHandler<ReservationModifyCommand> {
        private readonly IReservationService _reservationService;
        private readonly ISearchService _searchService;
        private readonly INotify _notify;
        private readonly INotificationBuilder _notificationBuilder;
        public ReservationModifyCommandHandler(IReservationService reservationService, ISearchService searchService, INotify notify, INotificationBuilder notificationBuilder) {
            _reservationService = reservationService;
            _searchService = searchService;
            _notify = notify;
            _notificationBuilder = notificationBuilder;
        }

        public void Handle(ReservationModifyCommand command) {
            command.Reservation = _reservationService.Update(command.Reservation);

            var emailBody = _notificationBuilder.BuildReservationInvoiceModel(command.Reservation.Id);
            emailBody.NotifyAdminOfModification = true;
            _notify.SendNotifyReservationInvoice(emailBody, Setting.Public_Website_Domain);
        }
    }
}
