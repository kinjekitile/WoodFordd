using FluentValidation.Attributes;
using System.Web;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;
namespace Woodford.UI.Web.Admin.ViewModels {

    public class UrlRedirectSearchModel {
        public UrlRedirectFilterModel Filter { get; set; }
        public ListOf<UrlRedirectModel> Result { get; set; }
    }

    [Validator(typeof(UrlRedirectValidator))]
    public class UrlRedirectViewModel {
        public UrlRedirectModel Redirect { get; set; }
    }
}