using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class RateAdjustmentRepository : RepositoryBase, IRateAdjustmentRepository {
        private const string AdjustmentNotFound = "Rate Adjustment could not be found";

        public RateAdjustmentRepository(IDbConnectionConfig connection) : base(connection) { }

        public RateAdjustmentModel Create(RateAdjustmentModel model) {
            RateAdjustment r = new RateAdjustment();
            r.AdjustmentTypeEnum = (int)model.AdjustmentType;
            r.StartDate = model.StartDate;
            r.EndDate = model.EndDate;
            r.BranchId = model.BranchId;
            r.VehicleGroupId = model.VehicleGroupId;
            r.RateCodeId = model.RateCodeId;
            r.NumberOfBookings = model.NumberOfBookings;
            r.AdjustmentPercentage = model.AdjustmentPercentage;
            r.IsArchived = model.IsArchived;
            //not sure why the default didn't work, but it breaks without setting the datecreated
            r.DateCreated = DateTime.Now;

            _db.RateAdjustments.Add(r);
            _db.SaveChanges();

            model.Id = r.Id;


            return model;
        }

        public RateAdjustmentModel Delete(int id) {
            throw new NotImplementedException();
        }

        public List<RateAdjustmentModel> Get(RateAdjustmentFilterModel filter, ListPaginationModel pagination) {

            ListOf<RateAdjustmentModel> results = new ListOf<RateAdjustmentModel>();

            var list = getAsIQueryable();
            if (filter != null) {
                if (!filter.ShowPastAdjustments) {
                    list = list.Where(x => x.EndDate >= DateTime.Today);
                }
                if (filter.AdjustmentType.HasValue)
                    list = list.Where(x => x.AdjustmentType == filter.AdjustmentType.Value);
                if (filter.StartDate.HasValue)
                    list = list.Where(x => x.StartDate == filter.StartDate.Value);
                if (filter.EndDate.HasValue)
                    list = list.Where(x => x.EndDate == filter.EndDate);
                if (filter.BranchId.HasValue)
                    list = list.Where(x => x.BranchId == filter.BranchId.Value);
                if (filter.VehicleGroupId.HasValue)
                    list = list.Where(x => x.VehicleGroupId == filter.VehicleGroupId.Value);
                if (filter.RateCodeId.HasValue)
                    list = list.Where(x => x.RateCodeId == filter.RateCodeId.Value);
            }

            if (pagination == null) {
                return list.ToList();
            } else {
                return list.OrderBy(x => x.Id).Skip(pagination.SkipItems).Take(pagination.ItemsPerPage).ToList();
            }

        }

        public List<RateAdjustmentModel> GetAdjustmentsForSearchCriteria(SearchCriteriaModel criteria) {

            var list = getAsIQueryable();

            var applicableAdjustments = list
                .Where(ra => ra.BranchId == criteria.PickUpLocationId)
                .Where(ra => ra.StartDate <= criteria.PickupDateTime && ra.EndDate >= criteria.PickupDateTime)
                .Where(ra => ra.IsArchived == false)
                .ToList();

            return applicableAdjustments;
        }

        public RateAdjustmentModel GetById(int id) {
            RateAdjustmentModel ra = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (ra == null) {
                throw new Exception(AdjustmentNotFound);
            }
            return ra;
        }

        public int GetCount(RateAdjustmentFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {
                if (filter.AdjustmentType.HasValue)
                    list = list.Where(x => x.AdjustmentType == filter.AdjustmentType.Value);
                if (filter.StartDate.HasValue)
                    list = list.Where(x => x.StartDate == filter.StartDate.Value);
                if (filter.EndDate.HasValue)
                    list = list.Where(x => x.EndDate == filter.EndDate);
                if (filter.BranchId.HasValue)
                    list = list.Where(x => x.BranchId == filter.BranchId.Value);
                if (filter.VehicleGroupId.HasValue)
                    list = list.Where(x => x.VehicleGroupId == filter.VehicleGroupId.Value);
                if (filter.RateCodeId.HasValue)
                    list = list.Where(x => x.RateCodeId == filter.RateCodeId.Value);
                if (filter.IsArchived.HasValue)
                    list = list.Where(x => x.IsArchived == filter.IsArchived.Value);
            }
            return list.Count();
        }

        public RateAdjustmentModel Update(RateAdjustmentModel model) {
            RateAdjustment r = _db.RateAdjustments.Where(x => x.Id == model.Id).SingleOrDefault();
            if (r == null)
                throw new Exception(AdjustmentNotFound);

            r.StartDate = model.StartDate;
            r.EndDate = model.EndDate;
            r.BranchId = model.BranchId;
            r.VehicleGroupId = model.VehicleGroupId;
            r.RateCodeId = model.RateCodeId;
            r.NumberOfBookings = model.NumberOfBookings;
            r.AdjustmentPercentage = model.AdjustmentPercentage;
            r.IsArchived = model.IsArchived;

            _db.SaveChanges();

            return model;

        }

        private IQueryable<RateAdjustmentModel> getAsIQueryable() {
            return _db.RateAdjustments.Select(x => new RateAdjustmentModel {
                Id = x.Id,
                AdjustmentType = (RateAdjustmentType)x.AdjustmentTypeEnum,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                BranchId = x.BranchId,
                Branch = new BranchModel {
                    Id = x.Branch.Id,
                    Title = x.Branch.Title
                },
                VehicleGroupId = x.VehicleGroupId,
                VehicleGroup = x.VehicleGroupId.HasValue ? (new VehicleGroupModel {
                    Id = x.VehicleGroup.Id,
                    Title = x.VehicleGroup.Title
                }) : null,
                RateCodeId = x.RateCodeId,
                RateCode = x.RateCodeId.HasValue ? (new RateCodeModel {
                    Id = x.RateCode.Id,
                    Title = x.RateCode.Title
                }) : null,
                NumberOfBookings = x.NumberOfBookings,
                AdjustmentPercentage = x.AdjustmentPercentage,
                DateCreated = x.DateCreated,
                IsArchived = x.IsArchived
            });
        }
    }
}
