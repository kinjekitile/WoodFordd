using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.ApplicationServices.Queries;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.ModelValidators {
    public static class DynamicPageValidationRuleSets {
        public const string Default = "default";
    }
    public class DynamicPageValidator : AbstractValidator<DynamicPageViewModel> {
        private readonly IQueryProcessor _query;

        public DynamicPageValidator() {

            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(DynamicPageValidationRuleSets.Default, () => {

                RuleFor(x => x.Page.AdminDescription)
                    .NotEmpty().WithMessage("Admin Name is required");

                RuleFor(x => x.Page.PageUrl)
                    .NotEmpty().WithMessage("Page Url is required")                    
                    .Must(CheckPageUrlIsUnique).WithMessage("Page Url already exists");

                RuleFor(x => x.Page.PageContent.PageTitle)
                    .NotEmpty().WithMessage("Page Title is required");
            });
        }

        private bool CheckPageUrlIsUnique(DynamicPageViewModel instance, string pageUrl) {
            if (string.IsNullOrEmpty(pageUrl))
                return true;
                        
            DynamicPageIdGetByUrlQuery query = new DynamicPageIdGetByUrlQuery { Url = pageUrl };
            int pageId = (_query.Process(query) ?? 0);

            if (pageId > 0) {
                if (instance.Page.Id == pageId) {
                    //same page. this is fine
                    return true;
                } else {
                    //add page / edited page url, but this conflicts with another page in the db
                    return false;
                }
            } else {
                //couldn't find url, so unique
                return true;
            }            
        }
    }

}
