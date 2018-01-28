using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IFileUploadRepository {
        FileUploadModel Create(FileUploadModel model);
        FileUploadModel Update(FileUploadModel model);
        FileUploadModel GetById(int id, bool includeFileContents = false);
        List<FileUploadModel> Get(FileUploadFilterModel filter, ListPaginationModel pagination);
        int GetCount(FileUploadFilterModel filter);
    }
}
