using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class FileUploadEditCommand : ICommand {
        public FileUploadModel Model { get; set; }               
    }

    public class FileUploadEditCommandHandler : ICommandHandler<FileUploadEditCommand> {
        private readonly IFileUploadService _fileService;
        public FileUploadEditCommandHandler(IFileUploadService fileService) {
            _fileService = fileService;
        }

        public void Handle(FileUploadEditCommand command) {
            _fileService.Update(command.Model);
        }
    }
}
