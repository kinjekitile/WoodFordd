using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Repositories;

namespace Woodford.Core.ApplicationServices.Commands {
    public class VehicleManufacturerEditCommand : ICommand {
        public VehicleManufacturerModel Manufacturer { get; set; }
    }

    public class VehicleManufacturerEditCommandHandler : ICommandHandler<VehicleManufacturerEditCommand> {
        private readonly IVehicleManufacturerRepository _repo;
        private readonly IPageContentService _pageContentService;
        private readonly IFileUploadService _fileUploadService;
        public VehicleManufacturerEditCommandHandler(IVehicleManufacturerRepository repo, IPageContentService pageContentService, IFileUploadService fileUploadService) {
            _repo = repo;
            _pageContentService = pageContentService;
            _fileUploadService = fileUploadService;
        }
        public void Handle(VehicleManufacturerEditCommand command) {
            if (command.Manufacturer.ManufacturerImage != null) {
                if (command.Manufacturer.ManufacturerImage.FileContents != null) {
                    if (command.Manufacturer.FileUploadId.HasValue) {
                        command.Manufacturer.ManufacturerImage.Id = command.Manufacturer.FileUploadId.Value;
                        command.Manufacturer.FileUploadId = _fileUploadService.Update(command.Manufacturer.ManufacturerImage).Id;
                    } else {
                        FileUploadModel f = _fileUploadService.Create(command.Manufacturer.ManufacturerImage);
                        command.Manufacturer.FileUploadId = f.Id;
                    }
                }
            }
            command.Manufacturer = _repo.Update(command.Manufacturer);
            _pageContentService.Update(command.Manufacturer.PageContent);
        }
    }
}
