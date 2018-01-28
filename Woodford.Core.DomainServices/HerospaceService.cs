using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class HerospaceService : IHerospaceService {
        private readonly IHerospaceRepository _repo;
        private readonly IFileUploadService _fileUploads;

        public HerospaceService(IHerospaceRepository repo, IFileUploadService fileUploads) {
            _repo = repo;
            _fileUploads = fileUploads;
        }
        public HerospaceItemModel Create(HerospaceItemModel model) {
            FileUploadModel f = _fileUploads.Create(model.HerospaceImage);
            model.FileUploadId = f.Id;
            HerospaceItemModel i = _repo.Create(model);
            return i;
        }

        public List<HerospaceItemModel> Get(HerospaceItemFilterModel filter) {
            return _repo.Get(filter);            
        }

        public HerospaceItemModel GetById(int id) {
            return _repo.GetById(id);            
        }

        public HerospaceItemModel Update(HerospaceItemModel model) {
            if (model.HerospaceImage != null) {
                model.HerospaceImage.Id = model.FileUploadId;
                model.FileUploadId = _fileUploads.Update(model.HerospaceImage).Id;
            }
            
            return _repo.Update(model);            
        }
    }
}
