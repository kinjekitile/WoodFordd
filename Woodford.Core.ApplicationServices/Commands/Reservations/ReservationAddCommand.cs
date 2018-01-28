using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationAddCommand : ICommand {
        //public ReservationModel Reservation { get; set; }
        public int VehicleId { get; set; }
        public int RateId { get; set; }
        public SearchCriteriaModel Criteria { get; set; }
        public ReservationModel ReservationOut { get; set; }
    }

    public class ReservationAddCommandHandler : ICommandHandler<ReservationAddCommand> {
        private readonly IReservationBuilder _reservationBuilder;
        private readonly IReservationService _reservationService;
        private readonly ISettingService _settings;
        

        public ReservationAddCommandHandler(IReservationBuilder reservationBuilder, IReservationService reservationService, ISettingService settings) {
            _reservationBuilder = reservationBuilder;
            _reservationService = reservationService;
            _settings = settings;


        }
        public void Handle(ReservationAddCommand command) {
            int referenceCodeLength = _settings.GetValue<int>(Setting.Reservation_Prefix_Length);
            ReservationModel r = _reservationBuilder.InitializeFromSearch(command.Criteria, command.VehicleId, command.RateId);
            
            
                     
            command.ReservationOut = _reservationService.Create(r);

            r.QuoteReference = GenerateRandomReferenceCode(r.Id, referenceCodeLength);

            _reservationService.Update(r);
        }

        private string GenerateRandomReferenceCode(int id, int length) {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--) {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
           
            return res.ToString();
        }
    }
}
