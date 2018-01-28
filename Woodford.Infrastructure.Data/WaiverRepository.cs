using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class WaiverRepository : RepositoryBase, IWaiverRepository {

        private const string ItemNotFound = "Waiver could not be found";

        public WaiverRepository(IDbConnectionConfig connection) : base(connection) { }

        public WaiverModel Create(WaiverModel model) {
            Waiver w = new Waiver();
            w.WaiverType = Convert.ToInt32(model.WaiverType);
            w.VehicleGroupId = model.VehicleGroupId;
            w.CostPerDay = model.CostPerDay;
            w.Liability = model.Liability;
            _db.Waivers.Add(w);
            _db.SaveChanges();
            model.Id = w.Id;
            return model;
        }

        public WaiverModel Update(WaiverModel model) {

            Waiver w = _db.Waivers.Where(x => x.Id == model.Id).SingleOrDefault();
            if (w == null) throw new Exception(ItemNotFound);
            w.WaiverType = Convert.ToInt32(model.WaiverType);
            //w.VehicleGroupId = model.VehicleGroupId;
            w.CostPerDay = model.CostPerDay;
            w.Liability = model.Liability;
            _db.SaveChanges();

            return model;
        }

        public List<WaiverModel> Get(WaiverFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.VehicleGroupId.HasValue) list = list.Where(x => x.VehicleGroupId == filter.VehicleGroupId.Value);
            }
            return list.ToList();

        }

        public WaiverModel GetById(int id) {
            WaiverModel w = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (w == null) throw new Exception(ItemNotFound);
            return w;
        }

        private IQueryable<WaiverModel> getAsIQueryable() {
            return _db.Waivers.Select(x => new WaiverModel {
                Id = x.Id,
                VehicleGroupId = x.VehicleGroupId,
                VehicleGroup = new VehicleGroupModel {
                    Id = x.VehicleGroup.Id,
                    Title = x.VehicleGroup.Title,
                    TitleDescription = x.VehicleGroup.TitleDescription
                },
                WaiverType = (WaiverType)x.WaiverType,
                CostPerDay = x.CostPerDay,
                Liability = x.Liability
            });
        }

        public void Delete(int id) {
            Waiver w = _db.Waivers.Where(x => x.Id == id).SingleOrDefault();
            if (w == null) throw new Exception(ItemNotFound);
            _db.Waivers.Remove(w);
            _db.SaveChanges();
        }
    }
}
