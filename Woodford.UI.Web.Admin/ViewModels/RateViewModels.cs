using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(RateRuleValidator))]
    public class RateRuleViewModel {
        public RateRuleModel Rule { get; set; }
    }

    [Validator(typeof(RateCodeValidator))]
    public class RateCodeViewModel {
        public RateCodeModel Code { get; set; }
        
    }

    public class RateSearchViewModel {
        public List<BranchModel> Branches { get; set; }
        public List<BranchModel> SelectedBranches { get; set; }
        public int[] SelectedBranchIds { get; set; }

        public RateFilterModel Filter { get; set; }

        public List<RateFilterResultsModel> Results { get; set; }

        public List<RateModel> Rates { get; set; }
        public RateSearchViewModel() {
            SelectedBranches = new List<BranchModel>();
            Results = new List<RateFilterResultsModel>();
            Rates = new List<RateModel>();
        }
    }

    public class RateUpdateViewModel {
        public List<BranchModel> SelectedBranches { get; set; }
        public RateCodeModel RateCode { get; set; }
        public List<VehicleGroupModel> VehicleGroups { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsOpenEnded { get; set; }
        public List<RateModel> Rates { get; set; }
        public RateDatesViewModel RateAppliesTo { get; set; }
        public RateUpdateViewModel() {
            SelectedBranches = new List<BranchModel>();
            VehicleGroups = new List<VehicleGroupModel>();
            Rates = new List<RateModel>();
        }
    }

    public class RateDatesViewModel {
        public bool IsOpenEnded { get; set; }
        public DateTime? ValidStartDate { get; set; }
        public DateTime? ValidEndDate { get; set; }
    }

    [Validator(typeof(RateUpdateFilterValidator))]
    public class RateSearchAndUpdateViewModel {
        public RateSearchModel Search { get; set; }
        public RateSearchResultModel Results { get; set; }

        public List<BranchModel> AllBranches { get; set; }
        public int[] SelectedBranchIds { get; set; }
        public List<BranchModel> SelectedBranches { get; set; }
        public RateDatesViewModel UpdateDateRange { get; set; }
        public string SelectedBranchIdsString { get; set; }

        public bool HasSearchRun { get; set; }

        public RateSearchAndUpdateViewModel() {
            Search = new RateSearchModel();
            AllBranches = new List<BranchModel>();
            SelectedBranches = new List<BranchModel>();
            UpdateDateRange = new RateDatesViewModel();
        }
    }
    
    public class RateFindViewModel {
        public List<BranchModel> AllBranches { get; set; }
        public List<BranchModel> SelectedBranches { get; set; }
        public int[] SelectedBranchIds { get; set; }

        public int RateCodeId { get; set; }
        public RateFindResultsModel Results { get; set; }
        public bool SearchRun { get; set; }

        public RateFindViewModel() {
            SearchRun = false;
            Results = new RateFindResultsModel();
        }
        //private bool findResult(RateFindResultsViewModel

        
    }
}
