using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices {
    public class ReservationBuilder : IReservationBuilder {
        
        private readonly IVehicleService _vehicleService;
        private readonly ISearchService _searchService;
        private readonly IUserService _userService;
        private readonly IBranchService _branchService;

        public ReservationBuilder(IVehicleService vehicleService,  ISearchService searchService, IUserService userService, IBranchService branchService) {            
            _vehicleService = vehicleService;
            _searchService = searchService;
            _userService = userService;
            _branchService = branchService;
        }

        public ReservationModel InitializeModifiedFromCriteria(ReservationModel r, SearchCriteriaModel criteria, int vehicleId, int rateId) {
            VehicleModel v = _vehicleService.GetById(vehicleId, false);
            var rateSearchResult = _searchService.GetReservationRateInfo(criteria, vehicleId, rateId);
            var branches = _branchService.Get(new BranchFilterModel { IsArchived = false }, null).Items;

            r.PickupDate = criteria.PickupDateTime;
            r.DropOffDate = criteria.DropOffDateTime;
            r.PickupBranchId = criteria.PickUpLocationId;
            r.PickupBranch = branches.Single(x => x.Id == r.PickupBranchId);
            r.DropOffBranchId = criteria.DropOffLocationId;
            r.DropOffBranch = branches.Single(x => x.Id == r.DropOffBranchId);
            r.VehicleId = vehicleId;
            r.Vehicle = v;
            r.VehicleExcess = v.ExcessAmount;
            r.VehicleDeposit = v.DepositAmount;
            r.RateId = rateId;
            r.RateCodeId = rateSearchResult.RateCodeId;
            r.RateCodeTitle = rateSearchResult.RateCodeTitle;
            r.RatePrice = rateSearchResult.Price;
            r.RateAdjustmentId = rateSearchResult.AdjustmentId;
            r.RateAdjustmentType = rateSearchResult.AdjustmentType;
            r.RateAdjustmentPercentage = rateSearchResult.AdjustmentPercentage;
            r.ContractFee = rateSearchResult.PriceCalculation.AdminFee;
            r.DropOffFee = rateSearchResult.PriceCalculation.DropOffFee;
            r.TaxRate = _branchService.GetTaxRateByBranchId(criteria.PickUpLocationId);
            if (r.VehicleId != vehicleId) {
                //Remove any upgrade
                r.VehicleUpgradeId = null;
                r.VehicleUpgrade = null;
            }

            return r;
        }

        public ReservationModel InitializeFromSearch(SearchCriteriaModel criteria, int vehicleId, int rateId) {
            VehicleModel v = _vehicleService.GetById(vehicleId, false);
            
            var rateSearchResult = _searchService.GetReservationRateInfo(criteria, vehicleId, rateId);            

            ReservationModel r = new ReservationModel();
            r.PickupDate = criteria.PickupDateTime;
            r.DropOffDate = criteria.DropOffDateTime;
            r.PickupBranchId = criteria.PickUpLocationId;
            r.DropOffBranchId = criteria.DropOffLocationId;
            r.VehicleId = vehicleId;
            r.VehicleExcess = v.ExcessAmount;
            r.VehicleDeposit = v.DepositAmount;
            r.RateId = rateId;
            r.RateCodeId = rateSearchResult.RateCodeId;
            r.RateCodeTitle = rateSearchResult.RateCodeTitle;
            r.RatePrice = rateSearchResult.Price;
            r.RateAdjustmentId = rateSearchResult.AdjustmentId;
            r.RateAdjustmentType = rateSearchResult.AdjustmentType;
            r.RateAdjustmentPercentage = rateSearchResult.AdjustmentPercentage;
            r.ContractFee = rateSearchResult.PriceCalculation.AdminFee;
            r.DropOffFee = rateSearchResult.PriceCalculation.DropOffFee;
            r.IsQuote = true;
            UserModel currentUser = _userService.GetCurrentUser();

            if (currentUser != null) {
                r.FirstName = currentUser.FirstName;
                r.LastName = currentUser.LastName;
                r.Email = currentUser.Email;
                r.IdNumber = currentUser.IdNumber;                            
            }

            r.DateCreated = DateTime.Now;
            r.ReservationState = ReservationState.Started;
            r.TaxRate = _branchService.GetTaxRateByBranchId(criteria.PickUpLocationId);

            return r;
        }
    }
}
