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
    public class VehicleGroupService : IVehicleGroupService {
        private readonly IVehicleGroupRepository _repo;
        private readonly IVehicleService _vehicleService;
        private readonly IFileUploadService _fileUploads;
        private readonly IPageContentService _pageContentService;
        public VehicleGroupService(IVehicleGroupRepository repo, IFileUploadService fileUploads, IPageContentService pageContentService, IVehicleService vehicleService) {
            _repo = repo;
            _fileUploads = fileUploads;
            _pageContentService = pageContentService;
            _vehicleService = vehicleService;
        }
        public VehicleGroupModel Create(VehicleGroupModel model) {
            if (model.VehicleGroupImage != null) {
                FileUploadModel f = _fileUploads.Create(model.VehicleGroupImage);
                model.FileUploadId = f.Id;
            }

            VehicleGroupModel vg = _repo.Create(model);
            vg.PageContent.VehicleGroupId = vg.Id;
            vg.PageContent = _pageContentService.Create(vg.PageContent);

            return vg;
        }

        public ListOf<VehicleGroupModel> Get(VehicleGroupFilterModel filter, ListPaginationModel pagination) {

            ListOf<VehicleGroupModel> res = new ListOf<VehicleGroupModel>();

            res.Pagination = pagination;

            if (pagination == null) {
                res.Items = _repo.Get(filter, pagination);
            } else {
                res.Pagination.TotalItems = _repo.GetCount(filter);
                res.Items = _repo.Get(filter, res.Pagination);
            }

            return res;

        }

        public VehicleGroupModel GetById(int id, bool includePageContent) {
            VehicleGroupModel vg = _repo.GetById(id);
            if (includePageContent) {
                vg.PageContent = _pageContentService.GetByForeignKey(vg.Id, PageContentForeignKey.VehicleGroupId);
            }
            vg.Vehicles = _vehicleService.Get(new VehicleFilterModel { VehicleGroupId = id }, null).Items;
            return vg;
        }

        public VehicleGroupModel Update(VehicleGroupModel model) {

            if (model.VehicleGroupImage != null) {
                if (model.VehicleGroupImage.FileContents != null) {
                    if (model.FileUploadId.HasValue) {
                        model.VehicleGroupImage.Id = model.FileUploadId.Value;
                        model.FileUploadId = _fileUploads.Update(model.VehicleGroupImage).Id;
                    } else {
                        FileUploadModel f = _fileUploads.Create(model.VehicleGroupImage);
                        model.FileUploadId = f.Id;
                    }
                }
            }
            VehicleGroupModel vg = _repo.Update(model);
            _pageContentService.Update(model.PageContent);

            return vg;

        }

        public VehicleGroupModel GetByUrl(string url, bool includePageContent = false) {
            VehicleGroupModel vg = _repo.GetByUrl(url);
            if (includePageContent) {
                vg.PageContent = _pageContentService.GetByForeignKey(vg.Id, PageContentForeignKey.VehicleGroupId);
            }
            vg.Vehicles = _vehicleService.Get(new VehicleFilterModel { VehicleGroupId = vg.Id }, null).Items;
            return vg;
        }

        public List<VehicleGroupModel> GetGroupsForVehicleIds(List<int> vehicleIds) {
            return _repo.GetGroupsForVehicleIds(vehicleIds);
        }
    }
}
