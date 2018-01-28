using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class FilesGetQuery : IQuery<ListOf<FileUploadModel>> {
        public FileUploadFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class FileGetQueryHandler : IQueryHandler<FilesGetQuery, ListOf<FileUploadModel>> {
        private readonly IFileUploadService _fileService;

        public FileGetQueryHandler(IFileUploadService fileService) {
            _fileService = fileService;
        }

        public ListOf<FileUploadModel> Process(FilesGetQuery query) {
            return _fileService.Get(query.Filter, query.Pagination);            
        }
    }
}
