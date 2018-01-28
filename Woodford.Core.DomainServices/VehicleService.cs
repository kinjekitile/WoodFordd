using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.DomainServices {
    public class VehicleService : IVehicleService {
        private readonly IVehicleRepository _repo;
        private readonly IFileUploadService _fileUploads;
        private readonly IPageContentService _pageContentService;
        

        public VehicleService(IVehicleRepository repo, IFileUploadService fileUploads, IPageContentService pageContentService) {
            _repo = repo;
            _fileUploads = fileUploads;
            _pageContentService = pageContentService;
            
        }

        public VehicleModel Create(VehicleModel model) {
            if (model.VehicleImage != null) {
                FileUploadModel f = _fileUploads.Create(model.VehicleImage);
                model.FileUploadId = f.Id;
            }

            if (model.VehicleImage2 != null) {
                FileUploadModel f2 = _fileUploads.Create(model.VehicleImage2);
                model.FileUploadId2 = f2.Id;
            }

            VehicleModel v = _repo.Create(model);
            model.PageContent.VehicleId = v.Id;
            v.PageContent = _pageContentService.Create(model.PageContent);

            return v;
        }

        public VehicleModel Update(VehicleModel model) {
            if (model.VehicleImage != null) {
                if (model.VehicleImage.FileContents != null) {
                    if (model.FileUploadId.HasValue) {
                        model.VehicleImage.Id = model.FileUploadId.Value;
                        model.FileUploadId = _fileUploads.Update(model.VehicleImage).Id;                        
                    } else {
                        FileUploadModel f = _fileUploads.Create(model.VehicleImage);
                        model.FileUploadId = f.Id;
                    }
                }
            }

            if (model.VehicleImage2 != null) {
                if (model.VehicleImage2.FileContents != null) {
                    if (model.FileUploadId2.HasValue) {
                        model.VehicleImage2.Id = model.FileUploadId2.Value;
                        model.FileUploadId2 = _fileUploads.Update(model.VehicleImage2).Id;                        
                    } else {
                        FileUploadModel f2 = _fileUploads.Create(model.VehicleImage2);
                        model.FileUploadId2 = f2.Id;
                    }
                }
            }

            VehicleModel v = _repo.Update(model);
            _pageContentService.Update(model.PageContent);

            return v;

        }

        public VehicleModel GetById(int id, bool includePageContent) {
            VehicleModel v = _repo.GetById(id);
            
            if (includePageContent) {
                v.PageContent = _pageContentService.GetByForeignKey(v.Id, PageContentForeignKey.VehicleId);
            }
            return v;
        }

        public VehicleModel GetByUrl(string url, bool includePageContent = false) {
            VehicleModel v = _repo.GetByUrl(url);
            if (includePageContent) {
                v.PageContent = _pageContentService.GetByForeignKey(v.Id, PageContentForeignKey.VehicleId);
            }
            return v;
        }

        public ListOf<VehicleModel> Get(VehicleFilterModel filter, ListPaginationModel pagination) {
            ListOf<VehicleModel> res = new ListOf<VehicleModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;
        }

        public List<VehicleModel> GetByIds(List<int> ids, bool includePageContent) {
            List<VehicleModel> vehicles = _repo.GetByIds(ids);
            if (includePageContent) {
                foreach (var v in vehicles) {
                    v.PageContent = _pageContentService.GetByForeignKey(v.Id, PageContentForeignKey.VehicleId);
                }
            }
            return vehicles;
        }
    }
}
