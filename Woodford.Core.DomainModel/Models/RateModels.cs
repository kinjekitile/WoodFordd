using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {

    public class RateUpsertModel {
        public List<RateUpdateModel> RatesToUpdate { get; set; }
        public List<RateModel> RatesToAdd { get; set; }
        public RateUpsertModel() {
            RatesToAdd = new List<RateModel>();
            RatesToUpdate = new List<RateUpdateModel>();
        }
    }


    
    public class RateUpdateModel {
        public int RateId { get; set; }
        public decimal RatePrice { get; set; }
    }

    public class RateForGroupModel {
        public List<int> RateIds { get; set; }
        public int VehicleGroupId { get; set; }
        public decimal RatePrice { get; set; }
    }

    public class RateCodeModel {
        public int Id { get; set; }
        public bool AvailableToPublic { get; set; }
        public bool AvailableToLoyalty { get; set; }
        public bool AvailableToCorporate { get; set; }
        public bool IsNotAdjustable { get; set; }
        public string Title { get; set; }
        public int RateRuleId { get; set; }
        public bool CanHaveUpgradeApplied { get; set; }
        public bool IsSticky { get; set; }
        public List<RateModel> Rates { get; set; }
        public RateRuleModel RateRule { get; set; }
        public string ColorCode { get; set; }

        public IEnumerable<CampaignModel> Campaigns { get; set; }
        public IEnumerable<CorporateModel> Corporates { get; set; }
        public IEnumerable<BranchRateCodeExclusionModel> Exclusions { get; set; }
        public RateCodeModel() {
            Rates = new List<RateModel>();
            Campaigns = new List<CampaignModel>();
            RateRule = new RateRuleModel();
            Corporates = new List<CorporateModel>();
            Exclusions = new List<BranchRateCodeExclusionModel>();
        }
    }
    public class RateCodeFilterModel {
        public bool? IsNotAdjustable { get; set; }
        public string Title { get; set; }
        public int? RateRuleId { get; set; }
        public bool? AvailableToPublic { get; set; }
        public bool? AvailableToLoyalty { get; set; }
        public bool? AvailableToCorporate { get; set; }

        public bool? CanHaveUpgradeApplied { get; set; }
        //todo change to 
        //public RateCodeAvailableToType? AvailableToType { get; set; }

    }

    public class RateRuleModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DaysOfWeek { get; set; }
        public int MinDays { get; set; }
        public int MaxDays { get; set; }
        //public int FreeKmsPerDay { get; set; }
        //public RateType RateType { get; set; }

        //todo RateType enum
        public RateRuleModel() {

        }
    }

    public class RateRuleFilterModel {
        public string Title { get; set; }
        public RateType? RateType { get; set; }
    }

    public class RateSearchModel {
        public List<int> BranchIds { get; set; }
        public int RateCodeId { get; set; }
        public bool IsOpenEnded { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

    public class RateSearchResultModel {

        public List<VehicleGroupModel> VehicleGroups { get; set; }
        public List<BranchModel> Branches { get; set; }
        public List<RateModel> Rates { get; set; }
        public RateSearchResultModel() {
            VehicleGroups = new List<VehicleGroupModel>();
            Branches = new List<BranchModel>();
            Rates = new List<RateModel>();
        }
    }

    public class RateFilterResultsModel {
        public string Title
        {
            get
            {
                if (IsOpenEnded) {
                    return "Open Ended";
                } else {
                    string title = "";
                    if (StartDate.HasValue && EndDate.HasValue) {
                        title = StartDate.Value.ToString("d MMM yyyy") + " - " + EndDate.Value.ToString("d MMM yyyy");

                    } else {
                        if (!StartDate.HasValue && !EndDate.HasValue) {
                            title = "Open Ended";
                        } else {
                            if (StartDate.HasValue) {
                                title = StartDate.Value.ToString("d MMM yyyy") + " - NA";
                            } else {
                                title = "NA - " + EndDate.Value.ToString("d MMM yyyy");
                            }
                        }
                    }
                    return title;


                }
            }
        }
        public List<int> BranchIds { get; set; }
        public string UrlBranches
        {
            get
            {
                List<string> branches = new List<string>();
                BranchIds.ForEach(x => branches.Add(x.ToString()));

                return string.Join(",", branches);
            }
        }
        public List<int> RateIds { get; set; }
        public string UrlRateIds
        {
            get
            {
                List<string> rates = new List<string>();
                RateIds.ForEach(x => rates.Add(x.ToString()));
                return string.Join(",", rates);
            }
        }
        public int RateCodeId { get; set; }
        public bool IsOpenEnded { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<decimal> Prices { get; set; }
        public RateFilterResultsModel() {
            BranchIds = new List<int>();
            RateIds = new List<int>();
            Prices = new List<decimal>();
        }
    }


    public class RateModel {
        public int Id { get; set; }
        public int VehicleGroupId { get; set; }
        public VehicleGroupModel VehicleGroup { get; set; }
        public int RateCodeId { get; set; }
        public RateCodeModel RateCode { get; set; }
        public int BranchId { get; set; }
        public BranchModel Branch { get; set; }
        public DateTime? ValidStartDate { get; set; }
        public DateTime? ValidEndDate { get; set; }
        public decimal Price { get; set; }
        public decimal CostPerKm { get; set; }
        public bool IsOpenEnded { get; set; }
        public bool IsDeleted { get; set; }
        public int FreeKms { get; set; }
        public bool HasUnlimitedKms { get; set; }
        public RateModel() {
            VehicleGroup = new VehicleGroupModel();
            RateCode = new RateCodeModel();
            Branch = new BranchModel();
        }

    }

    public class RateFilterModel {
        public int RateCodeId { get; set; }
        public List<int> BranchIds { get; set; }
        public bool? IsOpenEnded { get; set; }
        public DateTime? ValidStartDate { get; set; }
        public DateTime? ValidEndDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsUnlimitedKms { get; set; }
        public RateFilterModel() {
            BranchIds = new List<int>();
        }

    }

    public class RateFindResultsModel {
        public List<RateFindResultModel> Items { get; set; }
        public void AddResult(RateFindResultModel result) {
            int uniqueCount = Items.Where(x => x.EndDate == result.EndDate && x.StartDate == result.StartDate && x.IsOpenEnded == result.IsOpenEnded).Count();

            if (uniqueCount == 0) {
                Items.Add(result);
            }
        }
        public RateFindResultsModel() {
            Items = new List<RateFindResultModel>();
        }
    }

    public class RateFindResultModel {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsOpenEnded { get; set; }

        public string Title {
            get {
                if (IsOpenEnded) {
                    return "Is Open Ended";
                } else {
                    if (StartDate.HasValue && EndDate.HasValue) {
                        return StartDate.Value.ToString("dd/MM/yyyy") + " - " + EndDate.Value.ToString("dd/MM/yyyy");
                    } else {
                        if (StartDate.HasValue) {
                            return StartDate.Value.ToString("dd/MM/yyyy") + " - End Date not set";
                        }
                        if (EndDate.HasValue) {
                            return "Start Date not set - " + EndDate.Value.ToString("dd/MM/yyyy");
                        }
                        return "Start and End dates not set";
                    }
                    
                }
            }
        }

    }

}
