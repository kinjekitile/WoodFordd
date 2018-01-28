using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VehicleGroupEditCommand : ICommand {
        public VehicleGroupModel Model { get; set; }
    }

    public class VehicleGroupEditCommandHandler : ICommandHandler<VehicleGroupEditCommand> {
        private readonly IVehicleGroupService _vehicleGroupService;
        private readonly IPageContentService _pageContentService;
        public VehicleGroupEditCommandHandler(IVehicleGroupService vehicleGroupService, IPageContentService pageContentService) {
            _vehicleGroupService = vehicleGroupService;
            _pageContentService = pageContentService;
        }

        public void Handle(VehicleGroupEditCommand command) {            
            _vehicleGroupService.Update(command.Model);
            _pageContentService.Update(command.Model.PageContent);
        }
    }
}
