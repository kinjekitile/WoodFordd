using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.ModelValidators {

    public static class UrlRedirectValidationRuleSets {
        public const string Default = "default";
    }

    public class UrlRedirectValidator : AbstractValidator<UrlRedirectViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;
        public UrlRedirectValidator() {
            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(UrlRedirectValidationRuleSets.Default, () => {
                RuleFor(x => x.Redirect.OldUrl)
                    .NotEmpty().WithMessage("Old Url is required")
                .Must(CheckPageUrlIsUnique).WithMessage("A Redirect for this url already exists");
                RuleFor(x => x.Redirect.NewUrl)
                    .NotEmpty().WithMessage("New Url is required");
                RuleFor(x => x.Redirect.RedirectType)
                    .NotEmpty().WithMessage("Type is required");
            });

        }

        private bool CheckPageUrlIsUnique(UrlRedirectViewModel instance, string pageUrl) {
            if (string.IsNullOrEmpty(pageUrl))
                return true;

            UrlRedirectModel b = null;
            try {
                UrlRedirectGetByUrlQuery query = new UrlRedirectGetByUrlQuery { Url = pageUrl };
                b = _query.Process(query);
            } catch (Exception) { }

            if (b == null) {
                return true;
            } else {
                if (instance.Redirect.Id == b.Id) {
                    //same page. this is fine
                    return true;
                } else {
                    //add page / edited page url, but this conflicts with another page in the db
                    return false;
                }
            }
        }
    }
}