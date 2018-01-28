using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class InterBranchDropOffFeeRepository : RepositoryBase, IInterBranchDropOffFeeRepository {

        private const string NotFound = "Drop Off Fee could not be found";

        public InterBranchDropOffFeeRepository(IDbConnectionConfig connection) : base(connection) { }

        public InterBranchDropOffFeeModel Create(InterBranchDropOffFeeModel model) {
            InterBranchDropOffFee f = new InterBranchDropOffFee();
            f.Branch1Id = model.Branch1Id;
            f.Branch2Id = model.Branch2Id;
            f.Price = model.Price;
            f.IsActive = true;
            _db.InterBranchDropOffFees.Add(f);
            _db.SaveChanges();
            model.Id = f.Id;
            return model;
        }

        public InterBranchDropOffFeeModel Update(InterBranchDropOffFeeModel model) {
            InterBranchDropOffFee f = _db.InterBranchDropOffFees.Where(x => x.Id == model.Id).SingleOrDefault();
            if (f == null)
                throw new Exception(NotFound);
            f.Branch1Id = model.Branch1Id;
            f.Branch2Id = model.Branch2Id;
            f.Price = model.Price;
            f.IsActive = model.IsActive;
            _db.SaveChanges();
            return model;
        }

        public List<InterBranchDropOffFeeModel> Get(InterBranchDropOffFeeFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsIQueryable();
            if (filter != null) {
                if (filter.Branch1Id.HasValue && filter.Branch2Id.HasValue) {
                    list = list.Where(x => (x.Branch1Id == filter.Branch1Id.Value && x.Branch2Id == filter.Branch2Id.Value) || (x.Branch1Id == filter.Branch2Id.Value && x.Branch2Id == filter.Branch1Id.Value));
                } else {
                    if (filter.Branch1Id.HasValue)
                        list = list.Where(x => x.Branch1Id == filter.Branch1Id.Value || x.Branch2Id == filter.Branch1Id.Value);
                    if (filter.Branch2Id.HasValue)
                        list = list.Where(x => x.Branch2Id == filter.Branch2Id.Value || x.Branch1Id == filter.Branch2Id.Value);
                }
                if (filter.IsActive.HasValue)
                    list = list.Where(x => x.IsActive == filter.IsActive.Value);
            }
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public int GetCount(InterBranchDropOffFeeFilterModel filter) {
            var list = GetAsIQueryable();
            if (filter != null) {
                if (filter.Branch1Id.HasValue && filter.Branch2Id.HasValue) {
                    list = list.Where(x => (x.Branch1Id == filter.Branch1Id.Value && x.Branch2Id == filter.Branch2Id.Value) || (x.Branch1Id == filter.Branch2Id.Value && x.Branch2Id == filter.Branch1Id.Value));
                } else {
                    if (filter.Branch1Id.HasValue)
                        list = list.Where(x => x.Branch1Id == filter.Branch1Id.Value || x.Branch2Id == filter.Branch1Id.Value);
                    if (filter.Branch2Id.HasValue)
                        list = list.Where(x => x.Branch2Id == filter.Branch2Id.Value || x.Branch1Id == filter.Branch2Id.Value);
                }
                if (filter.IsActive.HasValue)
                    list = list.Where(x => x.IsActive == filter.IsActive.Value);
            }
            return list.Count();
        }

        public InterBranchDropOffFeeModel GetById(int id) {
            InterBranchDropOffFeeModel f = GetAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (f == null)
                throw new Exception(NotFound);
            return f;
        }

        private IQueryable<InterBranchDropOffFeeModel> GetAsIQueryable() {
            return _db.InterBranchDropOffFees.Select(x => new InterBranchDropOffFeeModel {
                Id = x.Id,
                Branch1Id = x.Branch1Id,
                Branch1 = new BranchModel {
                    Id = x.Branch.Id,
                    Title = x.Branch.Title
                },
                Branch2Id = x.Branch2Id,
                Branch2 = new BranchModel {
                    Id = x.Branch1.Id,
                    Title = x.Branch1.Title
                },
                Price = x.Price,
                IsActive = x.IsActive
            });
        }
    }
}
