using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class FileUploadGetByIdQuery : IQuery<FileUploadModel> {
        public int Id { get; set; }       
    }

    public class FileUploadGetByIdQueryHandler : IQueryHandler<FileUploadGetByIdQuery, FileUploadModel> {
        private readonly IFileUploadService _fileService;

        public FileUploadGetByIdQueryHandler(IFileUploadService fileService) {
            _fileService = fileService;
        }

        public FileUploadModel Process(FileUploadGetByIdQuery query) {
            return _fileService.GetById(query.Id, null, null, null, false);
        }
    }
}
