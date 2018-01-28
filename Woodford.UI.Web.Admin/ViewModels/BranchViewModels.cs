using FluentValidation.Attributes;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(BranchValidator))]    
    public class BranchViewModel {
        public BranchModel Branch { get; set; }
        public HttpPostedFileBase BranchImage { get; set; }
        public int TotalPickups { get; set; }
    }

    [Validator(typeof(InterBranchDropOffFeeValidator))]
    public class InterBranchDropOffFeeViewModel {
        public InterBranchDropOffFeeModel DropOff { get; set; }
    }
}
