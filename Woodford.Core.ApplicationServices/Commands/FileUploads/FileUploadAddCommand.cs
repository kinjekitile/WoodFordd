using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Commands {
    public class FileUploadAddCommand : ICommand {
        public FileUploadModel Model { get; set; }
    }

    public class FileUploadAddCommandHandler : ICommandHandler<FileUploadAddCommand> {
        private readonly IFileUploadService _fileService;
        public FileUploadAddCommandHandler(IFileUploadService fileService) {
            _fileService = fileService;
        }

        public void Handle(FileUploadAddCommand command) {
            command.Model = _fileService.Create(command.Model);
        }
    }
}
