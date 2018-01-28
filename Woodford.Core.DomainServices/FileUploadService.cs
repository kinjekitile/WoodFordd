using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class FileUploadService : IFileUploadService {
        private readonly IFileUploadRepository _repo;
        private readonly IImageSizer _sizer;
        private readonly ISettingService _settings;
        public FileUploadService(IFileUploadRepository repo, IImageSizer sizer, ISettingService settings) {
            _repo = repo;
            _sizer = sizer;
            _settings = settings;
        }
        public FileUploadModel Create(FileUploadModel model, bool resize = false) {
            if (resize) {
                model.FileContents = resizeImageOnUpload(model.FileExtension, model.FileContents);
            }
            model.MimeType = MimeTypes.Get(model.FileExtension.Replace(".", ""));
            return _repo.Create(model);
        }

        public ListOf<FileUploadModel> Get(FileUploadFilterModel filter, ListPaginationModel pagination) {
            ListOf<FileUploadModel> res = new ListOf<FileUploadModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;
        }

        public FileUploadModel GetById(int id, int? width, int? height, int? crop, bool includeFileContents = false) {
            FileUploadModel f = _repo.GetById(id, includeFileContents);
            if (width.HasValue || height.HasValue || crop.HasValue) {
                f.FileContents = _sizer.ImageResize(f.FileContents, width, height, crop, null);
            }
            return f;
        }

        public FileUploadModel Update(FileUploadModel model, bool resize = true) {
            if (model.FileContents != null) { 
                if (resize) {
                    model.FileContents = resizeImageOnUpload(model.FileExtension, model.FileContents);
                }
                
                model.MimeType = MimeTypes.Get(model.FileExtension.Replace(".", ""));
            }
            return _repo.Update(model);
        }

        private byte[] resizeImageOnUpload(string fileExtension, byte[] fileContents) {
            if (fileContents != null) {
                if (_sizer.IsImage(fileExtension)) {
                    int maxImageLength = _settings.GetValue<int>(Setting.Image_Max_Length);
                    fileContents = _sizer.ImageResize(fileContents, maxImageLength, maxImageLength, null, null);
                }
            }
            return fileContents;
        }
    }
}
