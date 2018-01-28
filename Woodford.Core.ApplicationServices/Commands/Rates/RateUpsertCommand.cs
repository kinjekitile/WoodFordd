using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class RateUpsertCommand : ICommand {

        public List<RateModel> NewRates { get; set; }
        public List<RateModel> ExistingRates { get; set; }
        public List<RateModel> RemoveRates { get; set; }

        public RateUpsertCommand() {
            NewRates = new List<RateModel>();
            ExistingRates = new List<RateModel>();
            RemoveRates = new List<RateModel>();

        }

    }

    public class RateUpsertCommandHandler : ICommandHandler<RateUpsertCommand> {

        private readonly IRateService _rateService;
        private readonly IRateCodeService _rateCodeService;
        private readonly IVehicleGroupService _vehicleGroupService;
        private readonly IBranchService _branchService;

        public RateUpsertCommandHandler(IRateService rateService, IRateCodeService rateCodeService, IVehicleGroupService vehicleGroupService, IBranchService branchService) {
            _rateService = rateService;
            _rateCodeService = rateCodeService;
            _vehicleGroupService = vehicleGroupService;
            _branchService = branchService;
        }
        public void Handle(RateUpsertCommand command) {

            foreach (var rate in command.NewRates) {
                _rateService.Create(rate);
            }
                        
            foreach (var rate in command.ExistingRates) {
                _rateService.Update(rate);
            }

            foreach (var rate in command.RemoveRates) {
                _rateService.MarkAsDeleted(rate.Id);
            }

        }
    }
}
