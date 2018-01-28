using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class VehicleGroupRepository : RepositoryBase, IVehicleGroupRepository {

        private const string VehicleGroupNotFound = "Vehicle Group could not be found";

        public VehicleGroupRepository(IDbConnectionConfig connection) : base(connection) { }

        public VehicleGroupModel Create(VehicleGroupModel model) {
            VehicleGroup vg = new VehicleGroup();
            vg.VehicleGroupTypeId = (int)model.GroupType;
            vg.Title = model.Title;
            vg.TitleDescription = model.TitleDescription;
            vg.DateCreated = DateTime.Now;
            vg.IsArchived = false;
            vg.PageUrl = model.PageUrl;
            vg.FileUploadId = model.FileUploadId;
            
            _db.VehicleGroups.Add(vg);
            _db.SaveChanges();
            model.Id = vg.Id;
            return model;
        }

        public List<VehicleGroupModel> Get(VehicleGroupFilterModel filter, ListPaginationModel pagination) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue) list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.Ids.Count > 0)
                    list = list.Where(x => filter.Ids.Contains(x.Id));
                if (filter.GroupType.HasValue) {
                    list = list.Where(x => x.GroupType == filter.GroupType.Value);
                }
            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }            
        }

        public VehicleGroupModel GetById(int id) {
            VehicleGroupModel vg = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (vg == null)
                throw new Exception(VehicleGroupNotFound);
            return vg;
        }

        public VehicleGroupModel GetByUrl(string url) {
            VehicleGroupModel vg = getAsIQueryable().Where(x => x.PageUrl == url).SingleOrDefault();
            if (vg == null) throw new Exception(VehicleGroupNotFound);
            return vg;
        }

        public int GetCount(VehicleGroupFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue) list = list.Where(x => x.Id == filter.Id.Value);
            }
            return list.Count();
        }

        public List<VehicleGroupModel> GetGroupsForVehicleIds(List<int> vehicleIds) {
            throw new NotImplementedException();
        }

        public VehicleGroupModel Update(VehicleGroupModel model) {
            VehicleGroup vg = _db.VehicleGroups.Where(x => x.Id == model.Id).SingleOrDefault();
            if (vg == null)
                throw new Exception(VehicleGroupNotFound);
            vg.VehicleGroupTypeId = (int)model.GroupType;
            vg.Title = model.Title;
            vg.TitleDescription = model.TitleDescription;
            vg.PageUrl = model.PageUrl;
            vg.FileUploadId = model.FileUploadId;
            _db.SaveChanges();
            return model;
        }

        private IQueryable<VehicleGroupModel> getAsIQueryable() {
            return _db.VehicleGroups.Select(x => new VehicleGroupModel {
                Id = x.Id,
                GroupType = (VehicleGroupType)x.VehicleGroupTypeId,
                Title = x.Title,
                TitleDescription = x.TitleDescription,
                DateCreated = x.DateCreated,
                IsArchived = x.IsArchived,
                FileUploadId = x.FileUploadId,
                PageUrl = x.PageUrl,
                SortOrder = x.SortOrder,
                VehicleGroupImage = x.FileUploadId.HasValue ? (new FileUploadModel {
                    Id = x.FileUpload.Id,
                    Title = x.FileUpload.Title,
                    FileExtension = x.FileUpload.FileExtension,
                    DateUploaded = x.FileUpload.DateUploaded
                }) : null
            });
        }

    }
}
