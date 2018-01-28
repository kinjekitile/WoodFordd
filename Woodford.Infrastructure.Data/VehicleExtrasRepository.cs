using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class VehicleExtrasRepository : RepositoryBase, IVehicleExtrasRepository {

        private const string ExtraNotFound = "Vehicle extra could not be found";
        public VehicleExtrasRepository(IDbConnectionConfig connection) : base(connection) { }

        public VehicleExtrasModel Edit(VehicleExtrasModel model) {
            VehicleExtra v = _db.VehicleExtras.SingleOrDefault(x => x.Id == model.Id);
            if (v == null)
                throw new Exception(ExtraNotFound);
            v.Title = model.Title;
            v.Price = model.Price;
            _db.SaveChanges();
            return model;

        }

        public List<VehicleExtrasModel> Get() {
            return getAsIQueryable().ToList();
        }

        public VehicleExtrasModel GetById(int id) {
            VehicleExtrasModel v = getAsIQueryable().SingleOrDefault(x => x.Id == id);
            if (v == null)
                throw new Exception(ExtraNotFound);
            return v;
        }

        public List<VehicleExtrasModel> GetByIds(List<int> ids) {
            return getAsIQueryable().Where(x => ids.Contains(x.Id)).ToList();
        }

        private IQueryable<VehicleExtrasModel> getAsIQueryable() {
            return _db.VehicleExtras.Select(x => new VehicleExtrasModel {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                ImageClass = x.ImageClass,
                OptionType = (VehicleExtraOption)x.Id
            });
        }
    }
}
