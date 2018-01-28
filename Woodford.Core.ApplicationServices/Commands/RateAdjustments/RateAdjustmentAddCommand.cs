using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands.RateAdjustments {
    public class RateAdjustmentAddCommand : ICommand {

        public RateAdjustmentModel Model { get; set; }
    }

    public class RateAdjustmentAddCommandHandler : ICommandHandler<RateAdjustmentAddCommand> {
        private readonly IRateAdjustmentService _rateAdjustService;
        public RateAdjustmentAddCommandHandler(IRateAdjustmentService rateAdjustService) {
            _rateAdjustService = rateAdjustService;
        }
        public void Handle(RateAdjustmentAddCommand command) {
            command.Model = _rateAdjustService.Create(command.Model);
        }
    }
}
