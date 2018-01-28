using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class VehicleRepository : RepositoryBase, IVehicleRepository {
        private const string VehicleNotFound = "Vehicle could not be found";
        public VehicleRepository(IDbConnectionConfig connection) : base(connection) { }
        public VehicleModel Create(VehicleModel model) {
            Vehicle v = new Vehicle();
            v.Title = model.Title;
            v.VehicleGroupId = model.VehicleGroupId;
            v.VehicleManufacturerId = model.VehicleManufacturerId;
            v.IsArchived = false;
            v.FileUploadId = model.FileUploadId;
            v.FileUploadId2 = model.FileUploadId2;
            v.PageUrl = model.PageUrl;
            v.ShortDescription = model.ShortDescription;
            v.NumberOfPassengers = model.NumberOfPassengers;
            v.NumberOfBaggage = model.NumberOfBaggage;
            v.IsPetrol = model.IsPetrol;
            v.HasAircon = model.HasAircon;
            v.HasRadio = model.HasRadio;
            v.HasPowerSteering = model.HasPowerSteering;
            v.IsAutomatic = model.IsAutomatic;
            v.ExcessAmount = model.ExcessAmount;
            v.DepositAmount = model.DepositAmount;

            _db.Vehicles.Add(v);
            _db.SaveChanges();
            model.Id = v.Id;
            return model;
        }

        public List<VehicleModel> Get(VehicleFilterModel filter, ListPaginationModel pagination) {
            var list = getByIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.IsArchived.HasValue)
                    list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
                if (filter.VehicleGroupId.HasValue)
                    list = list.Where(x => x.VehicleGroup.Id == filter.VehicleGroupId.Value);
                if (filter.VehicleManufacturerId.HasValue)
                    list = list.Where(x => x.VehicleManufacturerId == filter.VehicleManufacturerId.Value);
            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public VehicleModel GetById(int id) {
            VehicleModel v = getByIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (v == null)
                throw new Exception(VehicleNotFound);
            return v;
        }

        public List<VehicleModel> GetByIds(List<int> ids) {
            return getByIQueryable().Where(x => ids.Contains(x.Id)).ToList();
        }

        public VehicleModel GetByUrl(string url) {
            VehicleModel v = getByIQueryable().Where(x => x.PageUrl == url).SingleOrDefault();
            if (v == null)
                throw new Exception(VehicleNotFound);
            return v;
        }

        public int GetCount(VehicleFilterModel filter) {
            var list = getByIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.IsArchived.HasValue)
                    list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
                if (filter.VehicleGroupId.HasValue)
                    list = list.Where(x => x.VehicleGroup.Id == filter.VehicleGroupId.Value);
                if (filter.VehicleManufacturerId.HasValue)
                    list = list.Where(x => x.VehicleManufacturerId == filter.VehicleManufacturerId.Value);
            }
            return list.Count();
        }

        public VehicleModel Update(VehicleModel model) {
            Vehicle v = _db.Vehicles.Where(x => x.Id == model.Id).SingleOrDefault();
            if (v == null)
                throw new Exception(VehicleNotFound);
            v.Title = model.Title;
            v.VehicleGroupId = model.VehicleGroupId;
            v.VehicleManufacturerId = model.VehicleManufacturerId;
            v.IsArchived = model.IsArchived;
            v.FileUploadId = model.FileUploadId;
            v.FileUploadId2 = model.FileUploadId2;
            v.PageUrl = model.PageUrl;
            v.ShortDescription = model.ShortDescription;
            v.NumberOfPassengers = model.NumberOfPassengers;
            v.NumberOfBaggage = model.NumberOfBaggage;
            v.IsPetrol = model.IsPetrol;
            v.HasAircon = model.HasAircon;
            v.HasRadio = model.HasRadio;
            v.HasPowerSteering = model.HasPowerSteering;
            v.IsAutomatic = model.IsAutomatic;
            v.ExcessAmount = model.ExcessAmount;
            v.DepositAmount = model.DepositAmount;
            _db.SaveChanges();
            return model;
        }

        private IQueryable<VehicleModel> getByIQueryable() {

            return _db.Vehicles.Select(x => new VehicleModel {
                Id = x.Id,
                Title = x.Title,
                VehicleGroupId = x.VehicleGroupId,
                VehicleGroup = new VehicleGroupModel {
                    Id = x.VehicleGroup.Id,
                    GroupType = (VehicleGroupType)x.VehicleGroup.VehicleGroupTypeId,
                    Title = x.VehicleGroup.Title,
                    TitleDescription = x.VehicleGroup.TitleDescription,
                    DateCreated = x.VehicleGroup.DateCreated,
                    IsArchived = x.VehicleGroup.IsArchived,
                    FileUploadId = x.VehicleGroup.FileUploadId,
                    PageUrl = x.VehicleGroup.PageUrl,
                    SortOrder = x.VehicleGroup.SortOrder
                },
                VehicleManufacturerId = x.VehicleManufacturerId,
                VehicleManufacturer = x.VehicleManufacturerId.HasValue ? new VehicleManufacturerModel {
                    Id = x.VehicleManufacturer.Id,
                    Title = x.VehicleManufacturer.Title,
                    SortOrder = x.VehicleManufacturer.SortOrder
                } : null,
                IsArchived = x.IsArchived,
                PageUrl = x.PageUrl,
                FileUploadId = x.FileUploadId,
                FileUploadId2 = x.FileUploadId2,
                VehicleImage = x.FileUploadId.HasValue ? (new FileUploadModel {
                    Id = x.FileUpload.Id,
                    Title = x.FileUpload.Title,
                    FileExtension = x.FileUpload.FileExtension,
                    DateUploaded = x.FileUpload.DateUploaded
                }) : null,
                VehicleImage2 = x.FileUploadId2.HasValue ? (new FileUploadModel {
                    Id = x.FileUpload1.Id,
                    Title = x.FileUpload1.Title,
                    FileExtension = x.FileUpload1.FileExtension,
                    DateUploaded = x.FileUpload1.DateUploaded
                }) : null,
                ShortDescription = x.ShortDescription,
                NumberOfPassengers = x.NumberOfPassengers,
                NumberOfBaggage = x.NumberOfBaggage,
                IsPetrol = x.IsPetrol,
                HasAircon = x.HasAircon,
                HasRadio = x.HasRadio,
                HasPowerSteering = x.HasPowerSteering,
                IsAutomatic = x.IsAutomatic,
                ExcessAmount = x.ExcessAmount,
                DepositAmount = x.DepositAmount,
                SortOrder = x.SortOrder
            });
        }
    }
}
