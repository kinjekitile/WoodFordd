using FluentValidation.Attributes;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(CampaignValidator))]
    public class CampaignViewModel {
        public CampaignModel Campaign { get; set; }
        public HttpPostedFileBase CampaignImage { get; set; }
        public HttpPostedFileBase CampaignSearchResultIconImage { get; set; }
    }
}