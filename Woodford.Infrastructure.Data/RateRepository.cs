using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Infrastructure.Data {
    public class RateRepository : RepositoryBase, IRateRepository {

        private const string RateNotFound = "Rate could not be found";

        public RateRepository(IDbConnectionConfig connection) : base(connection) { }

        public RateModel Create(RateModel model) {
            Rate r = new Rate();

            r.VehicleGroupId = model.VehicleGroupId;
            r.RateCodeId = model.RateCodeId;
            r.BranchId = model.BranchId;
            r.ValidStartDate = model.ValidStartDate;
            r.ValidEndDate = model.ValidEndDate;
            r.Price = model.Price;
            r.IsOpenEnded = model.IsOpenEnded;
            r.CostPerKm = model.CostPerKm;
            r.FreeKms = model.FreeKms;
            r.HasUnlimitedKms = model.HasUnlimitedKms;
            _db.Rates.Add(r);
            _db.SaveChanges();
            model.Id = r.Id;
            return model;
        }

        public RateModel Update(RateModel model) {
            Rate r = _db.Rates.Where(x => x.Id == model.Id).SingleOrDefault();
            if (r == null)
                throw new Exception(RateNotFound);

            //we don't need to ever change these 
            //r.VehicleGroupId = model.VehicleGroupId;
            //r.RateCodeId = model.RateCodeId;
            //r.BranchId = model.BranchId;
            r.ValidStartDate = model.ValidStartDate;
            r.ValidEndDate = model.ValidEndDate;
            r.Price = model.Price;
            r.IsOpenEnded = model.IsOpenEnded;
            r.CostPerKm = model.CostPerKm;
            r.FreeKms = model.FreeKms;
            r.HasUnlimitedKms = model.HasUnlimitedKms;
            _db.SaveChanges();
            return model;
        }

        public void MarkAsDeleted(int id) {
            Rate r = _db.Rates.Where(x => x.Id == id).SingleOrDefault();
            r.IsDeleted = true;
            _db.SaveChanges();
        }

        public List<RateModel> Get(RateFilterModel filter) {
            var list = getAsIQueryable();
            if (filter != null) {


                list = list.Where(x => x.RateCodeId == filter.RateCodeId);
                if (filter.BranchIds != null) {
                    if (filter.BranchIds.Count > 0) {
                        list = list.Where(x => filter.BranchIds.Contains(x.BranchId));
                    }
                }


                if (filter.IsOpenEnded.HasValue) {
                    list = list.Where(x => x.IsOpenEnded == filter.IsOpenEnded);
                }

                //if (filter.VehicleGroupId.HasValue)
                //    list = list.Where(x => x.VehicleGroupId == filter.VehicleGroupId);

                if (filter.ValidStartDate.HasValue)
                    list = list.Where(x => x.ValidStartDate == filter.ValidStartDate);
                if (filter.ValidEndDate.HasValue)
                    list = list.Where(x => x.ValidEndDate == filter.ValidEndDate);
                if (filter.IsDeleted.HasValue)
                    list = list.Where(x => x.IsDeleted == filter.IsDeleted);
                if (filter.IsUnlimitedKms.HasValue)
                    list = list.Where(x => x.HasUnlimitedKms == filter.IsUnlimitedKms);
            }
            return list.ToList();
        }

        public RateModel GetById(int id) {
            RateModel r = getAsIQueryable().Where(x => x.Id == id).SingleOrDefault();
            if (r == null)
                throw new Exception(RateNotFound);
            return r;
        }



        private IQueryable<RateModel> getAsIQueryable() {
            return _db.Rates.Select(x => new RateModel {
                Id = x.Id,
                VehicleGroupId = x.VehicleGroupId,
                VehicleGroup = new VehicleGroupModel {
                    Id = x.VehicleGroup.Id,
                    Title = x.VehicleGroup.Title
                },
                BranchId = x.BranchId,
                ValidStartDate = x.ValidStartDate,
                ValidEndDate = x.ValidEndDate,
                Price = x.Price,
                CostPerKm = x.CostPerKm,
                IsOpenEnded = x.IsOpenEnded,
                IsDeleted = x.IsDeleted,
                RateCodeId = x.RateCodeId,
                FreeKms = x.FreeKms,
                HasUnlimitedKms = x.HasUnlimitedKms,
                RateCode = new RateCodeModel {
                    Id = x.RateCode.Id,
                    AvailableToCorporate = x.RateCode.AvailableToCorporate,
                    AvailableToLoyalty = x.RateCode.AvailableToLoyalty,
                    AvailableToPublic = x.RateCode.AvailableToPublic,
                    IsNotAdjustable = x.RateCode.IsNotAdjustable,
                    IsSticky = x.RateCode.IsSticky,
                    RateRule = new RateRuleModel {
                        Id = x.RateCode.RateRule.Id,
                        Title = x.RateCode.RateRule.Title,
                        DaysOfWeek = x.RateCode.RateRule.DaysOfWeek,
                        MaxDays = x.RateCode.RateRule.MaxDays,
                        MinDays = x.RateCode.RateRule.MinDays
                    },
                    Campaigns = x.RateCode.Campaigns.Select(y => new CampaignModel {
                        Id = y.Id,
                        Title = y.Title,
                        StartDate = y.StartDate,
                        EndDate = y.EndDate,
                        IsArchived = y.IsArchived,
                        SearchResultIconFileUploadId = y.SearchResultIconFileUploadId
                    }),
                    RateRuleId = x.RateCode.RateRuleId,
                    Title = x.RateCode.Title,
                    Corporates = x.RateCode.CorporateRateCodes.Select(y => new CorporateModel {
                        Id = y.CorporateId,
                        Title = y.Corporate.Title
                    }),
                    ColorCode = x.RateCode.ColorCode
                }
            });
        }

        public List<RateModel> GetRatesForSearchCriteria(SearchCriteriaModel criteria, List<int> availableVehicleGroupIds) {

            var list = getAsIQueryable();
            string pickupDayOfWeek = (Convert.ToInt32(criteria.PickupDateTime.DayOfWeek)).ToString();

            var results = list
                .Where(rate => rate.RateCode.AvailableToPublic == true || rate.RateCode.AvailableToLoyalty == criteria.IsAdvance || rate.RateCode.AvailableToCorporate == criteria.IsCorporate)
                .Where(rate => rate.RateCode.RateRule.MinDays <= criteria.NumberOfDays)
                .Where(rate => rate.RateCode.RateRule.MaxDays >= criteria.NumberOfDays)
                .Where(rate => rate.RateCode.RateRule.DaysOfWeek.Contains(pickupDayOfWeek))
                .Where(rate => availableVehicleGroupIds.Contains(rate.VehicleGroupId))
                .Where(rate => rate.BranchId == criteria.PickUpLocationId)
                .Where(rate => rate.IsDeleted == false)
                .Where(rate => (rate.IsOpenEnded) || (rate.ValidStartDate <= criteria.PickupDateTime && rate.ValidEndDate >= criteria.PickupDateTime))
                .ToList();

            //var results = list
            //  .Where(rate => rate.RateCode.AvailableToPublic == true || rate.RateCode.AvailableToLoyalty == criteria.IsAdvance || rate.RateCode.AvailableToCorporate == criteria.IsCorporate).ToList();

            //results = results.Where(rate => rate.RateCode.RateRule.MinDays <= criteria.NumberOfDays).ToList();

            //results = results.Where(rate => rate.RateCode.RateRule.MaxDays >= criteria.NumberOfDays).ToList();

            //results = results.Where(rate => rate.RateCode.RateRule.DaysOfWeek.Contains(pickupDayOfWeek)).ToList();

            //results = results.Where(rate => availableVehicleGroupIds.Contains(rate.VehicleGroupId)).ToList();

            //results = results.Where(rate => rate.BranchId == criteria.PickUpLocationId).ToList();

            //results = results.Where(rate => rate.IsDeleted == false).ToList();

            //results = results.Where(rate => (rate.IsOpenEnded) || (rate.ValidStartDate <= criteria.PickupDateTime && rate.ValidEndDate >= criteria.PickupDateTime))
            //  .ToList();

            return results.ToList();
        }
    }
}
