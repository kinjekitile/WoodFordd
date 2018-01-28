using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class BranchVehicleExclusionRepository : RepositoryBase, IBranchVehicleExclusionRepository {
        public BranchVehicleExclusionRepository(IDbConnectionConfig connection) : base(connection) { }

        private const string BranchVehicleExclusionNotFound = "Branch Vehicle Exclusion could not be found";

        public BranchVehicleExclusionModel Create(BranchVehicleExclusionModel model) {
            BranchVehicleExclusion e = new BranchVehicleExclusion();
            e.BranchVehicleId = model.BranchVehicleId;
            e.StartDate = model.StartDate;
            e.EndDate = model.EndDate;
            _db.BranchVehicleExclusions.Add(e);
            _db.SaveChanges();
            model.Id = e.Id;
            return model;
        }

        public void Delete(int id) {
            BranchVehicleExclusion e = _db.BranchVehicleExclusions.Where(x => x.Id == id).SingleOrDefault();
            if (e == null)
                throw new Exception(BranchVehicleExclusionNotFound);
            _db.BranchVehicleExclusions.Remove(e);
            _db.SaveChanges();
        }

        public List<BranchVehicleExclusionModel> Get(BranchVehicleExclusionFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public BranchVehicleExclusionModel GetById(int id) {
            BranchVehicleExclusionModel e = GetAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (e == null)
                throw new Exception(BranchVehicleExclusionNotFound);
            return e;
        }

        public int GetCount(BranchVehicleExclusionFilterModel filter) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public BranchVehicleExclusionModel Update(BranchVehicleExclusionModel model) {
            var e = _db.BranchVehicleExclusions.Where(x => x.Id == model.Id).SingleOrDefault();
            if (e == null)
                throw new Exception(BranchVehicleExclusionNotFound);
            e.StartDate = model.StartDate;
            e.EndDate = model.EndDate;
            _db.SaveChanges();
            return model;

        }

        private IQueryable<BranchVehicleExclusionModel> applyFilter(IQueryable<BranchVehicleExclusionModel> list, BranchVehicleExclusionFilterModel filter) {

            if (filter != null) {
                if (filter.BranchVehicleId.HasValue)
                    list = list.Where(x => x.BranchVehicleId == filter.BranchVehicleId.Value);
                if (filter.BranchId.HasValue)
                    list = list.Where(x => x.BranchVehicle.BranchId == filter.BranchId.Value);
                if (filter.SearchStart.HasValue)
                    list = list.Where(x => (x.StartDate <= filter.SearchStart && x.EndDate >= filter.SearchStart) || (x.StartDate <= filter.SearchEnd && x.EndDate >= filter.SearchStart));
            }

            return list;
        }

        private IQueryable<BranchVehicleExclusionModel> GetAsIQueryable() {
            return _db.BranchVehicleExclusions.Select(x => new BranchVehicleExclusionModel {
                Id = x.Id,
                BranchVehicleId = x.BranchVehicleId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                BranchVehicle = new BranchVehicleModel {
                    Id = x.BranchVehicleId,
                    BranchId = x.BranchVehicle.BranchId,
                    VehicleId = x.BranchVehicle.VehicleId
                }

            });
        }
    }
}
