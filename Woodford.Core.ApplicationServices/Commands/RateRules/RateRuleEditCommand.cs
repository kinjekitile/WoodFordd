using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class RateRuleEditCommand : ICommand {
        public RateRuleModel Model { get; set; }
    }

    public class RateRuleEditCommandHandler : ICommandHandler<RateRuleEditCommand> {
        private readonly IRateRuleService _rateRuleService;
        public RateRuleEditCommandHandler(IRateRuleService rateRuleService) {
            _rateRuleService = rateRuleService;
        }
        public void Handle(RateRuleEditCommand command) {
            command.Model = _rateRuleService.Update(command.Model);
        }
    }
}
