using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class RateRuleAddCommand : ICommand {
        public RateRuleModel Model { get; set; }
    }

    public class RateRuleAddCommandHandler : ICommandHandler<RateRuleAddCommand> {
        private readonly IRateRuleService _rateRuleService;
        public RateRuleAddCommandHandler(IRateRuleService rateRuleService) {
            _rateRuleService = rateRuleService;
        }
        public void Handle(RateRuleAddCommand command) {
            command.Model = _rateRuleService.Create(command.Model);
        }
    }
}
