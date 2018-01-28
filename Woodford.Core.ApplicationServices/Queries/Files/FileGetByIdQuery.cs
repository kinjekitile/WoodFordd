using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class FileGetByIdQuery : IQuery<FileUploadModel> {
        public int Id { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Crop { get; set; }
    }

    public class FileGetByIdQueryHandler : IQueryHandler<FileGetByIdQuery, FileUploadModel> {
        private readonly IFileUploadService _fileService;

        public FileGetByIdQueryHandler(IFileUploadService fileService) {
            _fileService = fileService;
        }

        public FileUploadModel Process(FileGetByIdQuery query) {
            return _fileService.GetById(query.Id, query.Width, query.Height, query.Crop, true);
        }
    }
}
