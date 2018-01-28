using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class BranchRateCodeExclusionRepository : RepositoryBase, IBranchRateCodeExclusionRepository {
        public BranchRateCodeExclusionRepository(IDbConnectionConfig connection) : base(connection) { }

        private const string BranchRateCodeExclusionNotFound = "Branch Rate Code Exclusion could not be found";

        public BranchRateCodeExclusionModel Create(BranchRateCodeExclusionModel model) {
            BranchRateCodeExclusion e = new BranchRateCodeExclusion();
            e.RateCodeId = model.RateCodeId;
            e.BranchId = model.BranchId;
            e.StartDate = model.StartDate;
            e.EndDate = model.EndDate;
            _db.BranchRateCodeExclusions.Add(e);
            _db.SaveChanges();
            model.Id = e.Id;
            return model;
        }

        public void Delete(int id) {
            BranchRateCodeExclusion e = _db.BranchRateCodeExclusions.Where(x => x.Id == id).SingleOrDefault();
            if (e == null)
                throw new Exception(BranchRateCodeExclusionNotFound);
            _db.BranchRateCodeExclusions.Remove(e);
            _db.SaveChanges();
        }

        public List<BranchRateCodeExclusionModel> Get(BranchRateCodeExclusionFilterModel filter, ListPaginationModel pagination) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }
        }

        public BranchRateCodeExclusionModel GetById(int id) {
            BranchRateCodeExclusionModel e = GetAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (e == null)
                throw new Exception(BranchRateCodeExclusionNotFound);
            return e;
        }

        public int GetCount(BranchRateCodeExclusionFilterModel filter) {
            var list = GetAsIQueryable();
            list = applyFilter(list, filter);
            return list.Count();
        }

        public BranchRateCodeExclusionModel Update(BranchRateCodeExclusionModel model) {
            var e = _db.BranchRateCodeExclusions.Where(x => x.Id == model.Id).SingleOrDefault();
            if (e == null)
                throw new Exception(BranchRateCodeExclusionNotFound);
            e.StartDate = model.StartDate;
            e.EndDate = model.EndDate;
            _db.SaveChanges();
            return model;

        }

        private IQueryable<BranchRateCodeExclusionModel> applyFilter(IQueryable<BranchRateCodeExclusionModel> list, BranchRateCodeExclusionFilterModel filter) {
            if (filter != null) {
                if (!filter.ShowPastExclusions) {
                    list = list.Where(x => x.EndDate >= DateTime.Today);
                }
                if (filter.BranchId.HasValue)
                    list = list.Where(x => x.BranchId == filter.BranchId.Value);
                if (filter.RateCodeId.HasValue)
                    list = list.Where(x => x.RateCodeId == filter.RateCodeId.Value);
                if (filter.RateCodeIds != null)
                    list = list.Where(x => filter.RateCodeIds.Contains(x.Id));
                if (filter.SearchStart.HasValue)
                    list = list.Where(x => (x.StartDate <= filter.SearchStart && x.EndDate >= filter.SearchStart) || (x.StartDate <= filter.SearchEnd && x.EndDate >= filter.SearchStart));
            }
            return list;
        }

        private IQueryable<BranchRateCodeExclusionModel> GetAsIQueryable() {
            return _db.BranchRateCodeExclusions.Select(x => new BranchRateCodeExclusionModel {
                Id = x.Id,
                BranchId = x.BranchId,
                RateCodeId = x.RateCodeId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Branch = new BranchModel {
                    Id = x.Branch.Id,
                    Title = x.Branch.Title
                },
                RateCode = new RateCodeModel {
                    Id = x.RateCode.Id,
                    Title = x.RateCode.Title
                }
            });
        }
    }
}
