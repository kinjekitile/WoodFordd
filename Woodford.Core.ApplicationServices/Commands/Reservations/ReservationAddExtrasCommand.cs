using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationAddExtrasCommand : ICommand {
        //public ReservationModel Reservation { get; set; }
        public int ReservationId { get; set; }
        public List<int> ExtraIds { get; set; }        
    }

    public class ReservationAddExtrasCommandHandler : ICommandHandler<ReservationAddExtrasCommand> {
        
        private readonly IReservationService _reservationService;
        private readonly IVehicleExtrasService _vehicleExtraService;

        public ReservationAddExtrasCommandHandler(IReservationService reservationService, IVehicleExtrasService vehicleExtraService) {
            _reservationService = reservationService;
            _vehicleExtraService = vehicleExtraService;
        }
        public void Handle(ReservationAddExtrasCommand command) {
            ReservationModel reservation = _reservationService.GetById(command.ReservationId);

            List<VehicleExtrasModel> extras = _vehicleExtraService.GetByIds(command.ExtraIds);

            List<ReservationVehicleExtraModel> reservationExtras = new List<ReservationVehicleExtraModel>();
            foreach (var e in extras) {
                decimal price = e.Price;
                int? reservationBenefitId = null;
                ReservationLoyaltyTierBenefitModel benefit = null;
                switch (e.OptionType) {
                    case VehicleExtraOption.GPSRental:
                        benefit = reservation.Benefits.FirstOrDefault(x => x.BenefitTypeId == BenefitType.FreeGPS);
                        break;
                    case VehicleExtraOption.AdditionalDrivers:
                        benefit = reservation.Benefits.FirstOrDefault(x => x.BenefitTypeId == BenefitType.FreeAdditionalDriver);
                        break;
                }
                if (benefit != null) {
                    reservationBenefitId = benefit.Id;
                }
                reservationExtras.Add(new ReservationVehicleExtraModel {
                    ReservationId = command.ReservationId,
                    VehicleExtraId = e.Id,
                    ExtraTitle = e.Title,
                    ExtraPrice = price,
                    ReservationLoyaltyBenefitId = reservationBenefitId
                
                });
            }

            _reservationService.AddVehicleExtraModels(command.ReservationId, reservationExtras);
        }
    }
}
