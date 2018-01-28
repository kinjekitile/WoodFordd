using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.Core.Interfaces.Providers;

namespace Woodford.Core.ApplicationServices {
    public class SearchService : ISearchService {

        private readonly IAuthenticate _authenticate;
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleGroupService _vehicleGroupService;
        private readonly IBranchService _branchService;
        private readonly IBranchVehicleService _branchVehicleService;
        private readonly IBranchVehicleExclusionService _branchVehicleExclusionService;
        private readonly IRateService _rateService;
        private readonly IRateCodeService _rateCodeService;
        private readonly IRateRuleService _rateRuleService;
        private readonly IRateAdjustmentService _rateAdjustmentService;
        private readonly ICountdownSpecialService _countdownSpecialService;
        private readonly ILoyaltyService _loyaltyService;
        private readonly ISettingService _settingService;
        private readonly IInterBranchDropOffFeeService _interBranchDropOffFeeService;
        private readonly IUserService _userService;
        private readonly ICorporateService _corporateService;
        private readonly IBranchRateCodeExclusionRepository _branchRateCodeExclusions;
        private readonly ICampaignService _campaignService;

        public SearchService(IVehicleService vehicleService, IVehicleGroupService vehicleGroupService, IBranchService branchService, IRateService rateService, IRateCodeService rateCodeService, IRateRuleService rateRuleService, IRateAdjustmentService rateAdjustmentService, ICountdownSpecialService countdownSpecialService, ILoyaltyService loyaltyService, IBranchVehicleService branchVehicleService, IBranchVehicleExclusionService branchVehicleExclusionService, IAuthenticate authenticate, ISettingService settingService, IInterBranchDropOffFeeService interBranchDropOffFeeService, IUserService userService, ICorporateService corporateService, IBranchRateCodeExclusionRepository branchRateCodeExclusions, ICampaignService campaignService) {

            _authenticate = authenticate;
            _vehicleService = vehicleService;
            _vehicleGroupService = vehicleGroupService;
            _branchService = branchService;
            _rateService = rateService;
            _rateCodeService = rateCodeService;
            _rateRuleService = rateRuleService;
            _rateAdjustmentService = rateAdjustmentService;
            _countdownSpecialService = countdownSpecialService;
            _loyaltyService = loyaltyService;
            _branchVehicleService = branchVehicleService;
            _branchVehicleExclusionService = branchVehicleExclusionService;
            _settingService = settingService;
            _interBranchDropOffFeeService = interBranchDropOffFeeService;
            _userService = userService;
            _corporateService = corporateService;
            _branchRateCodeExclusions = branchRateCodeExclusions;
            _campaignService = campaignService;

        }

        public SearchResultsModel Search(SearchCriteriaModel criteria) {

            SearchResultsModel searchResults = new SearchResultsModel();

            criteria.IsAdvance = !string.IsNullOrEmpty(_authenticate.CurrentUserName());
  

            searchResults.Criteria = criteria;

            string username = _authenticate.CurrentUserName();


            UserModel user = null;
            bool isLoggedIn = false;   

            if (!string.IsNullOrEmpty(username)) {
                user = _userService.GetByUsername(username);
                isLoggedIn = true;
                if (user.CorporateId.HasValue) {
                    criteria.CorporateId = user.CorporateId;
                    searchResults.Criteria.IsLoggedIn = true;
                    searchResults.Criteria.IsCorporate = true;

                }

            } else {
                searchResults.Criteria.CorporateId = null;
                searchResults.Criteria.IsLoggedIn = false;
                searchResults.Criteria.IsCorporate = false;
            }


        

            //Branch Vehicle Availability
            var vehicles = _branchVehicleService.GetAvailableVehicles(criteria.PickUpLocationId, criteria.PickupDateTime).Where(x => !x.IsArchived);

            if (criteria.VehicleGroupType.HasValue) {
                vehicles = vehicles.Where(x => x.VehicleGroup.GroupType == criteria.VehicleGroupType.Value);
            }

            decimal taxRate = _branchService.GetTaxRateByBranchId(criteria.PickUpLocationId);

            var adminFee = Convert.ToDecimal(_settingService.Get(Setting.Booking_Admin_Fee).Value);

            decimal dropOffFee = 0m;
            if (criteria.DropOffLocationId != criteria.PickUpLocationId) {
                var d = _interBranchDropOffFeeService.Get(new InterBranchDropOffFeeFilterModel { IsActive = true, Branch1Id = criteria.PickUpLocationId, Branch2Id = criteria.DropOffLocationId }, null);
                if (d != null) {
                    InterBranchDropOffFeeModel dropOffFeeModel = d.Items.FirstOrDefault();
                    if (dropOffFeeModel != null) {
                        dropOffFee = dropOffFeeModel.Price;
                    }
                }
            }

            if (criteria.VehicleId.HasValue) {
                vehicles = vehicles.Where(x => x.Id == criteria.VehicleId.Value);
            }

            //Vehicle Groups
            var vehicleGroups = vehicles.Select(x => x.VehicleGroup).ToList();

            //Vehicles Excluded from branch
            var vehicleExclusions = _branchVehicleExclusionService.Get(new BranchVehicleExclusionFilterModel { BranchId = criteria.PickUpLocationId, SearchStart = criteria.PickupDate, SearchEnd = criteria.DropOffDate }, null).Items;

            //Remove Excluded Vehicles
            vehicles = vehicles.Where(x => vehicleExclusions.Where(y => y.BranchVehicle.VehicleId == x.Id).Count() == 0).ToList();

            //Applicable Rates
            var rates = _rateService.GetRatesForSearchCriteria(criteria, vehicleGroups.Select(x => x.Id).Distinct().ToList());

            var rateCodeIds = rates.Select(x => x.RateCodeId).Distinct().ToList();

            //Rate Codes that have been excluded from the pickup branch for specific time periods
            var rateCodeExclusions = _branchRateCodeExclusions.Get(new BranchRateCodeExclusionFilterModel { BranchId = criteria.PickUpLocationId, SearchStart = criteria.PickupDate, SearchEnd = criteria.DropOffDate }, null);

            
            

            //Remove excluded rates
            rates = rates.Where(x => rateCodeExclusions.Where(y => y.RateCodeId == x.RateCodeId).Count() == 0).ToList();


            //Rate Rule Minimum Days
            if (criteria.MinimumDays.HasValue) {
                rates = rates.Where(x => x.RateCode.RateRule.MinDays >= criteria.MinimumDays).ToList();
            }
            

            //Only show corporate rate codes to relevant corporate users
            if (criteria.CorporateId.HasValue) {
                var corporate = _corporateService.GetById(criteria.CorporateId.Value);
                //corporates only see their own rates, they do NOT see public rates
                rates = rates.Where(x => corporate.RateCodes.Select(y => y.Id).Contains(x.RateCodeId)).ToList();
            } else {
                //Do not show corporate rates to non corporate users
                rates = rates.Where(x => x.RateCode.Corporates.Count() == 0 || x.RateCode.AvailableToPublic == true).ToList();
            }

            var today = DateTime.Today;
            //Campaign RateCode to not show in normal results
            if (criteria.CampaignId.HasValue) {
                var campaign = _campaignService.GetById(criteria.CampaignId.Value);

                //Check campaing has valid dates
                if (campaign.StartDate <= today && campaign.EndDate >= today) {
                    //Only show rates related to campaign id
                    rates = rates.Where(x => x.RateCode.Campaigns.Where(y => !y.IsArchived).Select(y => y.Id).Contains(campaign.Id)).ToList();
                }
            } else {
                //non campaign search - do not show any campaign related rates 
                //Essa says campaigns can show in main results
                rates = rates.Where(x => x.RateCode.Campaigns.Any(y => !y.IsArchived && y.StartDate <= today && y.EndDate >= today) || x.RateCode.Campaigns.Count() == 0).ToList();
            }


            //Rate Adjustments
            var allAdjustments = _rateAdjustmentService.GetAdjustmentsForSearchCriteria(criteria);


            foreach (var vehicle in vehicles) {
                var vehicleGroupRates = rates.Where(r => r.VehicleGroupId == vehicle.VehicleGroupId).OrderBy(x => x.Price).ToList();
                if (vehicleGroupRates.Count() > 0) {

                    //need to ensure vehicle.VehicleGroup.Title is populated by _branchVehicleService.GetAvailableVehicles method
                    var resultItem = new SearchResultItemModel { Vehicle = vehicle, VehicleGroupName = vehicle.VehicleGroup.TitleDescription };

                    bool countdownSpecialRowFound = false;
                    foreach (var vgRate in vehicleGroupRates.GroupBy(x => x.RateCodeId).Select(vg => vg.OrderByDescending(y => y.Id).First()).ToList()) {

                        var adjustmentsForRate = allAdjustments.Where(a => (a.BranchId == criteria.PickUpLocationId && a.AdjustmentType == RateAdjustmentType.ByBranch) || (a.RateCodeId == vgRate.RateCodeId && a.AdjustmentType == RateAdjustmentType.ByRateCode) || (a.VehicleGroupId == vgRate.VehicleGroupId && a.AdjustmentType == RateAdjustmentType.ByVehicleGroup)).ToList();
                        var resultRate = new SearchResultItemRateModel { RateId = vgRate.Id, Price = vgRate.Price, RateCodeId = vgRate.RateCodeId, RateCodeTitle = vgRate.RateCode.Title, RateCodeIsSticky = vgRate.RateCode.IsSticky, RateCodeColor = vgRate.RateCode.ColorCode, Campaigns = vgRate.RateCode.Campaigns.ToList()  };

                   

                        resultRate.PriceCalculation = new SearchPriceCalculationModel { NumberOfDays = criteria.NumberOfDays, TaxRate = taxRate, AdminFee = adminFee, DropOffFee = dropOffFee, RatePrice = vgRate.Price };
                        
                        if (!vgRate.RateCode.IsNotAdjustable) {
                            if (adjustmentsForRate.Count() > 0) {
                                var highestRateAdjustment = adjustmentsForRate.OrderByDescending(fr => fr.AdjustmentPercentage).First();
                                resultRate.AdjustmentId = highestRateAdjustment.Id;
                                resultRate.AdjustmentPercentage = highestRateAdjustment.AdjustmentPercentage;
                                resultRate.PriceCalculation.RateAdjustmentPercentage = highestRateAdjustment.AdjustmentPercentage;
                                resultRate.AdjustmentType = highestRateAdjustment.AdjustmentType;
                            }
                        }
                       

                        if (!countdownSpecialRowFound) {
                            
                            resultRate.CanUseCountdownSpecial = true;
                            countdownSpecialRowFound = true;
                        }

                        resultItem.Rates.Add(resultRate);

                    }

                    searchResults.Items.Add(resultItem);

                }
            }

            return searchResults;
        }

        public SearchResultItemRateModel GetReservationRateInfo(SearchCriteriaModel criteria, int vehicleId, int rateId) {

            SearchResultsModel search = Search(criteria);

            var vehicle = search.Items.SingleOrDefault(x => x.Vehicle.Id == vehicleId);

            var rate = vehicle.Rates.SingleOrDefault(x => x.RateId == rateId);

            return rate;

        }
    }
}
