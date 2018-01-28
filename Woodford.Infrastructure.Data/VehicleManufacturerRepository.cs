using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.Interfaces;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces.Repositories;
using Woodford.Core.DomainModel.Common;

namespace Woodford.Infrastructure.Data {
    public class VehicleManufacturerRepository : RepositoryBase, IVehicleManufacturerRepository {

        public VehicleManufacturerRepository(IDbConnectionConfig connection) : base(connection) {

        }
        public VehicleManufacturerModel Create(VehicleManufacturerModel model) {
            VehicleManufacturer v = new VehicleManufacturer();
            v.Title = model.Title;
            v.SortOrder = model.SortOrder;
            v.PageUrl = model.PageUrl;
            v.FileUploadId = model.FileUploadId;
            _db.VehicleManufacturers.Add(v);
            _db.SaveChanges();
            model.Id = v.Id;
            return model;

        }

        public List<VehicleManufacturerModel> Get(VehicleManufacturerFilterModel filter, ListPaginationModel pagination) {

            var list = getAsQueryable();

            if (filter != null) {
                if (filter.Id.HasValue)
                    list = list.Where(x => x.Id == filter.Id.Value);
                if (string.IsNullOrEmpty(filter.PageUrl)) {
                    //exclamation key broken TODO - fix on working keyboard :(
                } else {
                    list = list.Where(x => filter.PageUrl == x.PageUrl);
                }
            }

            return list.ToList();
        }

        public VehicleManufacturerModel GetById(int id) {
            var v = getAsQueryable().SingleOrDefault(x => x.Id == id);

            if (v == null) {
                throw new Exception("Could not find Vehicle Manufacturer");
            }
            return v;
        }

        public VehicleManufacturerModel Update(VehicleManufacturerModel model) {
            VehicleManufacturer v = _db.VehicleManufacturers.SingleOrDefault(x => x.Id == model.Id);

            if (v == null) {
                throw new Exception("Could not find Vehicle Manufacturer");
            }

            v.Title = model.Title;
            v.SortOrder = model.SortOrder;
            v.PageUrl = model.PageUrl;
            v.FileUploadId = model.FileUploadId;

            _db.SaveChanges();

            return model;
        }

        private IQueryable<VehicleManufacturerModel> getAsQueryable() {
            return _db.VehicleManufacturers.Select(x => new VehicleManufacturerModel {
                Id = x.Id,
                Title = x.Title,
                SortOrder = x.SortOrder,
                PageUrl = x.PageUrl,
                FileUploadId = x.FileUploadId,
                ManufacturerImage = x.FileUploadId.HasValue ? new FileUploadModel {
                    Id = x.FileUpload.Id,
                    Title = x.FileUpload.Title,
                    FileExtension = x.FileUpload.FileExtension,
                    DateUploaded = x.FileUpload.DateUploaded
                } : null
            });
        }
    }
}
