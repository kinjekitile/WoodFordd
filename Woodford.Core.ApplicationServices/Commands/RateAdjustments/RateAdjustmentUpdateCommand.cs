using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands.RateAdjustments {
    public class RateAdjustmentUpdateCommand : ICommand {
        public RateAdjustmentModel Model { get; set; }
    }

    public class RateAdjustmentUpdateCommandHandler : ICommandHandler<RateAdjustmentUpdateCommand> {
        private readonly IRateAdjustmentService _rateAdjustService;
        public RateAdjustmentUpdateCommandHandler(IRateAdjustmentService rateAdjustService) {
            _rateAdjustService = rateAdjustService;
        }
        public void Handle(RateAdjustmentUpdateCommand command) {
            command.Model = _rateAdjustService.Update(command.Model);
        }
    }
}
