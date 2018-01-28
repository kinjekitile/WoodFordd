using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Repositories;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VehicleManufacturerAddCommand : ICommand {
        public VehicleManufacturerModel Manufacturer { get; set; }
    }

    public class VehicleManufacturerAddCommandHandler : ICommandHandler<VehicleManufacturerAddCommand> {
        private readonly IVehicleManufacturerRepository _repo;
        private readonly IPageContentService _pageContentService;
        private readonly IFileUploadService _fileUploadService;
        public VehicleManufacturerAddCommandHandler(IVehicleManufacturerRepository repo, IPageContentService pageContentService, IFileUploadService fileUploadService) {
            _repo = repo;
            _pageContentService = pageContentService;
            _fileUploadService = fileUploadService;
        }
        public void Handle(VehicleManufacturerAddCommand command) {
            if (command.Manufacturer.ManufacturerImage != null) {
                var f = _fileUploadService.Create(command.Manufacturer.ManufacturerImage);
                command.Manufacturer.FileUploadId = f.Id;
            }

            command.Manufacturer = _repo.Create(command.Manufacturer);
            command.Manufacturer.PageContent.VehicleManufacturerId = command.Manufacturer.Id;
            command.Manufacturer.PageContent = _pageContentService.Create(command.Manufacturer.PageContent);
        }
    }
}
