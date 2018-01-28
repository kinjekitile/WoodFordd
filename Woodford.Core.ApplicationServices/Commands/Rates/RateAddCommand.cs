using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;


namespace Woodford.Core.ApplicationServices.Commands {
    public class RateAddCommand : ICommand {
        public int RateCodeId { get; set; }
        public bool IsOpenEnded { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Tuple<int, decimal>> RatesForGroups { get; set; }
        public List<int> BranchIds { get; set; }
        public List<int> VehicleGroupIds { get; set; }

        public RateAddCommand() {
            RatesForGroups = new List<Tuple<int, decimal>>();
            BranchIds = new List<int>();
            VehicleGroupIds = new List<int>();
        }

    }

    public class RateAddCommandHandler : ICommandHandler<RateAddCommand> {

        private readonly IRateService _rateService;
        private readonly IRateCodeService _rateCodeService;
        private readonly IVehicleGroupService _vehicleGroupService;
        private readonly IBranchService _branchService;

        public RateAddCommandHandler(IRateService rateService, IRateCodeService rateCodeService, IVehicleGroupService vehicleGroupService, IBranchService branchService) {
            _rateService = rateService;
            _rateCodeService = rateCodeService;
            _vehicleGroupService = vehicleGroupService;
            _branchService = branchService;
        }
        public void Handle(RateAddCommand command) {

            RateCodeModel rateCode = _rateCodeService.GetById(command.RateCodeId);


            var branches = _branchService.Get(new BranchFilterModel { Ids = command.BranchIds }, null).Items;
            var branchIds = branches.Select(x => x.Id);
            //Test to see if branch ids exists
            foreach (var branchId in command.BranchIds) {
                if (!branchIds.Contains(branchId)) {
                    throw new Exception("No such branch Id: " + branchId);
                }
            }


            var vehicleGroupIds = _vehicleGroupService.Get(new VehicleGroupFilterModel { Ids = command.VehicleGroupIds }, null).Items.Select(x => x.Id);

            //Test to see if vehicle groups exists
            foreach (var vehicleGroupId in command.VehicleGroupIds) {
                if (!vehicleGroupIds.Contains(vehicleGroupId)) {
                    throw new Exception("No such vehicle group id: " + vehicleGroupId);
                }
            }


            //Each branch selected must have a new rate created for each combination of branchId and vehicleGroupId
            foreach( var branchId in command.BranchIds) {
                foreach (var item in command.RatesForGroups) {
                    RateModel rate = new RateModel();
                    rate.BranchId = branchId;
                    rate.RateCodeId = rateCode.Id;
                    rate.VehicleGroupId = item.Item1;
                    rate.Price = item.Item2;
                    rate.IsOpenEnded = command.IsOpenEnded;
                    if (!rate.IsOpenEnded) {
                        rate.ValidStartDate = command.StartDate;
                        rate.ValidEndDate = command.EndDate;
                    }

                    rate = _rateService.Create(rate);
                }
            }
            
        }
    }
}
