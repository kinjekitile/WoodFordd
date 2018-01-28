using FluentValidation.Attributes;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(DynamicPageValidator))]    
    public class DynamicPageViewModel {
        public DynamicPageModel Page { get; set; }        
    }
}
