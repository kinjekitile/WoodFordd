using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class CorporateUpdateRateCodesCommand : ICommand {
        public int CorporateId { get; set; }

        public List<int> AddedRateCodeIds { get; set; }
        public List<int> RemovedRateCodeIds { get; set; }
    }

    public class CorporateUpdateRateCodesCommandHandler : ICommandHandler<CorporateUpdateRateCodesCommand> {
        private readonly ICorporateService _corpService;
        public CorporateUpdateRateCodesCommandHandler(ICorporateService corpService) {
            _corpService = corpService;
        }
        public void Handle(CorporateUpdateRateCodesCommand command) {
            foreach (int rateCodeId in command.AddedRateCodeIds) {
                _corpService.AddRateCodeToCorporate(command.CorporateId, rateCodeId);
            }
            foreach (int rateCodeId in command.RemovedRateCodeIds) {
                _corpService.RemoveRateCodeFromCorporate(command.CorporateId, rateCodeId);
            }
        }
    }
}
