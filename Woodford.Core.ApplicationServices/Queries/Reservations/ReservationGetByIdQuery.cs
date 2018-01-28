using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class ReservationGetByIdQuery : IQuery<ReservationModel> {
        public int Id { get; set; }
        
    }

    public class ReservationGetByIdQueryHandler : IQueryHandler<ReservationGetByIdQuery, ReservationModel> {
        private readonly IReservationService _reservationService;
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleGroupService _groupService;
        private readonly IVehicleUpgradeService _upgradeService;
        private readonly IInvoiceService _invoiceService;


        public ReservationGetByIdQueryHandler(IReservationService reservationService, IVehicleService vehicleService, IVehicleGroupService groupService, IInvoiceService invoiceService, IVehicleUpgradeService upgradeService) {
            _reservationService = reservationService;
            _vehicleService = vehicleService;
            _groupService = groupService;
            _invoiceService = invoiceService;
            _upgradeService = upgradeService;
        }
        public ReservationModel Process(ReservationGetByIdQuery query) {
            var reservation = _reservationService.GetById(query.Id);
            reservation.Vehicle = _vehicleService.GetById(reservation.VehicleId, includePageContent: false);
            reservation.Vehicle.VehicleGroup = _groupService.GetById(reservation.Vehicle.VehicleGroupId, includePageContent: false);
            var invoice = _invoiceService.GetByReservationId(reservation.Id);
            reservation.Invoice = invoice;
            if (reservation.VehicleUpgradeId.HasValue) {
                var upgrade = _upgradeService.GetById(reservation.VehicleUpgradeId.Value);

                reservation.VehicleUpgrade = _vehicleService.GetById(upgrade.ToVehicleId, includePageContent: false);
                reservation.VehicleUpgrade.VehicleGroup = _groupService.GetById(reservation.VehicleUpgrade.VehicleGroupId, includePageContent: false);
            }
            return reservation;
        }
    }
}
