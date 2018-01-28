using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VoucherEditCommand : ICommand {
        public VoucherModel Model { get; set; }        
    }

    public class VoucherEditCommandHandler : ICommandHandler<VoucherEditCommand> {
        private readonly IVoucherService _voucherService;
        public VoucherEditCommandHandler(IVoucherService voucherService) {
            _voucherService = voucherService;            
        }

        public void Handle(VoucherEditCommand command) {
            command.Model = _voucherService.Update(command.Model);
        }
    }
}
