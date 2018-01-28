using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class BranchVehicleInsertDeleteCommand : ICommand {

        public List<int> VehicleIdsToAdd { get; set; }
        public List<int> BranchVehicleIdsToRemove { get; set; }
        public int BranchId { get; set; }

       
    }

    public class BranchVehicleInsertDeleteCommandHandler : ICommandHandler<BranchVehicleInsertDeleteCommand> {

        private readonly IBranchVehicleService _branchVehicleService;

        public BranchVehicleInsertDeleteCommandHandler(IBranchVehicleService branchVehicleService) {
            _branchVehicleService = branchVehicleService;
        }
        public void Handle(BranchVehicleInsertDeleteCommand command) {

            foreach (var vId in command.VehicleIdsToAdd) {
                _branchVehicleService.Create(new BranchVehicleModel { BranchId = command.BranchId, VehicleId = vId });
            }
                        
            foreach (var bvId in command.BranchVehicleIdsToRemove) {
                _branchVehicleService.Delete(bvId);
            }            

        }
    }
}
