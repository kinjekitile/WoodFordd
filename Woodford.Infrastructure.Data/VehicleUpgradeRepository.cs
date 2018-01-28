using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class VehicleUpgradeRepository : RepositoryBase, IVehicleUpgradeRepository {
        private const string NotFound = "Vehicle Upgrade could not be found";
        public VehicleUpgradeRepository(IDbConnectionConfig connection) : base(connection) { }
        public VehicleUpgradeModel Create(VehicleUpgradeModel model) {
            VehicleUpgrade v = new VehicleUpgrade();
            v.FromVehicleId = model.FromVehicleId;
            v.ToVehicleId = model.ToVehicleId;
            v.UpdateAmount = model.UpgradeAmount;
            v.StartDate = model.StartDate;
            v.EndDate = model.EndDate;
            v.BranchId = model.BranchId;
            v.DateCreated = DateTime.Now;
            v.IsActive = true;
            _db.VehicleUpgrades.Add(v);
            _db.SaveChanges();
            model.Id = v.Id;
            return model;
        }

        public List<VehicleUpgradeModel> Get(VehicleUpgradeFilterModel filter, ListPaginationModel pagination) {
            var list = getByIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.FromVehicleId.HasValue)
                    list = list.Where(x => x.FromVehicleId == filter.FromVehicleId.Value);
                if (filter.IsActive.HasValue)
                    list = list.Where(x => x.IsActive == filter.IsActive.Value);
                if (filter.BranchId.HasValue)
                    list = list.Where(x => x.BranchId == filter.BranchId.Value);
                if (filter.PickupDate.HasValue)
                    list = list.Where(x => x.StartDate <= filter.PickupDate.Value && x.EndDate >= filter.PickupDate.Value);
              

            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public VehicleUpgradeModel GetById(int id) {
            VehicleUpgradeModel v = getByIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (v == null)
                throw new Exception(NotFound);
            return v;
        }


        public int GetCount(VehicleUpgradeFilterModel filter) {
            var list = getByIQueryable();
            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (filter.FromVehicleId.HasValue)
                    list = list.Where(x => x.FromVehicleId == filter.FromVehicleId.Value);
                if (filter.IsActive.HasValue)
                    list = list.Where(x => x.IsActive == filter.IsActive.Value);
                if (filter.BranchId.HasValue)
                    list = list.Where(x => x.BranchId == filter.BranchId.Value);
            }
            return list.Count();
        }

        public VehicleUpgradeModel Update(VehicleUpgradeModel model) {
            VehicleUpgrade v = _db.VehicleUpgrades.Where(x => x.Id == model.Id).SingleOrDefault();
            if (v == null)
                throw new Exception(NotFound);
            v.FromVehicleId = model.FromVehicleId;
            v.ToVehicleId = model.ToVehicleId;
            v.UpdateAmount = model.UpgradeAmount;
            v.StartDate = model.StartDate;
            v.EndDate = model.EndDate;
            v.BranchId = model.BranchId;
            v.IsActive = model.IsActive;
            _db.SaveChanges();
            return model;
        }

        private IQueryable<VehicleUpgradeModel> getByIQueryable() {

            return _db.VehicleUpgrades.Select(x => new VehicleUpgradeModel {
                Id = x.Id,
                FromVehicleId = x.FromVehicleId,
                FromVehicle = new VehicleModel {
                    Id = x.Vehicle.Id,
                    Title = x.Vehicle.Title,
                    FileUploadId = x.Vehicle.FileUploadId,
                    VehicleImage = x.Vehicle.FileUploadId.HasValue ? (new FileUploadModel {
                        Id = x.Vehicle.FileUpload.Id,
                        Title = x.Vehicle.FileUpload.Title,
                        FileExtension = x.Vehicle.FileUpload.FileExtension,
                        DateUploaded = x.Vehicle.FileUpload.DateUploaded
                    }) : null
                },
                ToVehicleId = x.ToVehicleId,
                ToVehicle = new VehicleModel {
                    Id = x.Vehicle1.Id,
                    Title = x.Vehicle1.Title,
                    FileUploadId = x.Vehicle1.FileUploadId,                    
                    VehicleImage = x.Vehicle1.FileUploadId.HasValue ? (new FileUploadModel {
                        Id = x.Vehicle1.FileUpload.Id,
                        Title = x.Vehicle1.FileUpload.Title,
                        FileExtension = x.Vehicle1.FileUpload.FileExtension,
                        DateUploaded = x.Vehicle1.FileUpload.DateUploaded
                    }) : null
                },
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                BranchId = x.BranchId,
                Branch = new BranchModel {
                    Id = x.Branch.Id,
                    Title = x.Branch.Title
                },
                UpgradeAmount = x.UpdateAmount,
                DateCreated = x.DateCreated,
                IsActive = x.IsActive            });
        }
    }
}
