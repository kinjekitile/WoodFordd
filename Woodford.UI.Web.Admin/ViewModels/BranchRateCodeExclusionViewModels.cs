using FluentValidation.Attributes;
using System;
using System.Web;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(BranchRateCodeExclusionsFilterValidator))]
    public class BranchRateCodeExclusionsViewModel {
        public int BranchId { get; set; }
        public bool Filtered { get; set; }
        public ListOf<BranchRateCodeExclusionModel> BranchRateCodeExclusions { get; set; }
        public bool ShowPastExclusions { get; set; }
    }

    [Validator(typeof(BranchRateCodeExclusionValidator))]
    public class BranchRateCodeExlusionViewModel {
        public BranchRateCodeExclusionModel Exclusion { get; set; }
        public RateCodeModel RateCode { get; set; }
        public BranchModel Branch { get; set; }

        public BranchRateCodeExlusionViewModel() {
            Exclusion = new BranchRateCodeExclusionModel();
            Exclusion.StartDate = DateTime.Now;
            Exclusion.EndDate = DateTime.Now;
        }
    }
}