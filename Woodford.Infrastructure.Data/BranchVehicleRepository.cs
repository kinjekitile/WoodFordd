using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class BranchVehicleRepository : RepositoryBase, IBranchVehicleRepository {
        public BranchVehicleRepository(IDbConnectionConfig connection) : base(connection) { }

        private const string BranchVehicleNotFound = "BranchVehicle could not be found";

        public BranchVehicleModel Create(BranchVehicleModel model) {
            BranchVehicle b = new BranchVehicle();
            b.BranchId = model.BranchId;
            b.VehicleId = model.VehicleId;
            _db.BranchVehicles.Add(b);
            _db.SaveChanges();
            model.Id = b.Id;
            return model;
        }

        public void Delete(int id) {
            BranchVehicle bv = _db.BranchVehicles.Where(x => x.Id == id).SingleOrDefault();
            if (bv == null)
                throw new Exception(BranchVehicleNotFound);
            _db.BranchVehicles.Remove(bv);
            _db.SaveChanges();
        }

        public List<BranchVehicleModel> Get(BranchVehicleFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsIQueryable();
            if (filter != null) {
                //Cant filter exclusions here, do it in the service layer
                //if (!filter.ShowPastExclusions) {
                //    list = list.Where(x=>x.Exclusions.Any)
                //}
                if (filter.BranchId.HasValue)
                    list = list.Where(x => x.BranchId == filter.BranchId.Value);
                if (filter.VehicleId.HasValue)
                    list = list.Where(x => x.VehicleId == filter.VehicleId);
            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public BranchVehicleModel GetById(int id) {
            BranchVehicleModel b = GetAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (b == null)
                throw new Exception(BranchVehicleNotFound);
            return b;
        }

        public int GetCount(BranchVehicleFilterModel filter) {
            var list = GetAsIQueryable();
            if (filter != null) {
                if (filter.BranchId.HasValue)
                    list = list.Where(x => x.BranchId == filter.BranchId.Value);
                if (filter.VehicleId.HasValue)
                    list = list.Where(x => x.VehicleId == filter.VehicleId);
            }
            return list.Count();
        }

        public BranchVehicleModel Update(BranchVehicleModel model) {
            throw new NotImplementedException();
            
        }

        private IQueryable<BranchVehicleModel> GetAsIQueryable() {
            return _db.BranchVehicles.Select(x => new BranchVehicleModel {
                Id = x.Id,
                BranchId = x.BranchId,
                VehicleId = x.VehicleId,
                Vehicle = new VehicleModel {
                    Id = x.Vehicle.Id,
                    Title = x.Vehicle.Title
                },
                Exclusions = x.BranchVehicleExclusions.Select(y=> new BranchVehicleExclusionModel {
                    Id = y.Id,
                    BranchVehicleId = y.BranchVehicleId,
                    StartDate = y.StartDate,
                    EndDate = y.EndDate
                })
            });
        }

        public List<int> GetAvailableVehicleIds(int branchId, DateTime pickupDate) {

            List<int> branchVehicleIds = _db.BranchVehicles.Where(x => x.BranchId == branchId).Select(x => x.VehicleId).ToList();

            List<int> excludedVehicleIds = _db.BranchVehicleExclusions
                .Where(x => x.BranchVehicle.BranchId == branchId)
                .Where(x => x.StartDate <= pickupDate && x.EndDate >= pickupDate)
                .Select(x => x.BranchVehicle.VehicleId).ToList();

            List<int> availableVehicleIds = branchVehicleIds.Where(x => !excludedVehicleIds.Contains(x)).ToList();
            return availableVehicleIds;
        }
    }
}
