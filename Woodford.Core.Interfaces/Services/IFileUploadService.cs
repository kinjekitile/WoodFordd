using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IFileUploadService {
        FileUploadModel Create(FileUploadModel model, bool resize = true);
        FileUploadModel Update(FileUploadModel model, bool resize = true);
        FileUploadModel GetById(int id, int? width, int? height, int? crop, bool includeFileContents = false);
        ListOf<FileUploadModel> Get(FileUploadFilterModel filter, ListPaginationModel pagination);
    }
}
