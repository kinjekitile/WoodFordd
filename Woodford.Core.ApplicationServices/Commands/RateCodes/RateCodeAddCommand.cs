using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class RateCodeAddCommand : ICommand {
        public RateCodeModel Model { get; set; }

    }

    public class RateCodeAddCommandHandler : ICommandHandler<RateCodeAddCommand> {
        private readonly IRateCodeService _rateCodeService;
        public RateCodeAddCommandHandler(IRateCodeService rateCodeService) {
            _rateCodeService = rateCodeService;
        }
        public void Handle(RateCodeAddCommand command) {
           command.Model = _rateCodeService.Create(command.Model);
        }
    }
}
