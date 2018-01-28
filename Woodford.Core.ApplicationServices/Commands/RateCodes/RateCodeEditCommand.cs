using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class RateCodeEditCommand : ICommand {
        public RateCodeModel Model { get; set; }
    }

    public class RateCodeEditCommandHandler : ICommandHandler<RateCodeEditCommand> {
        private readonly IRateCodeService _rateCodeService;
        public RateCodeEditCommandHandler(IRateCodeService rateCodeService) {
            _rateCodeService = rateCodeService;
        }

        public void Handle(RateCodeEditCommand command) {
            command.Model = _rateCodeService.Update(command.Model);
        }
    }
}
