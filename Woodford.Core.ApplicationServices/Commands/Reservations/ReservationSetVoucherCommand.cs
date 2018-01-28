using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
namespace Woodford.Core.ApplicationServices.Commands {
    public class ReservationSetVoucherCommand : ICommand {
        public int ReservationId { get; set; }
        public int VoucherId { get; set; }
        public ReservationModel ReservationOut { get; set; }
    }

    public class RReservationSetVoucherCommandHandler : ICommandHandler<ReservationSetVoucherCommand> {
        private readonly IReservationService _reservationService;
        private readonly IVoucherService _voucherService;
        public RReservationSetVoucherCommandHandler(IReservationService reservationService, IVoucherService voucherService) {
            _reservationService = reservationService;
            _voucherService = voucherService;
        }
        public void Handle(ReservationSetVoucherCommand command) {
            var voucher = _voucherService.GetById(command.VoucherId);

            command.ReservationOut = _reservationService.SetVoucher(command.ReservationId, voucher);
            //Code below must only be run once reservation is completed
            //if (!voucher.IsMultiUse) {
            //    voucher.DateRedeemed = DateTime.Now;
            //    _voucherService.Update(voucher);
            //}
        }
    }
}
