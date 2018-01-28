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
    public static class NewsCategoryValidationRuleSets {
        public const string Default = "default";
    }
    public class NewsCategoryValidator : AbstractValidator<NewsCategoryViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;
        public NewsCategoryValidator() {

            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(BranchValidationRuleSets.Default, () => {
                RuleFor(x => x.Category.Title)
                    .NotEmpty().WithMessage("Category title is required");                
                RuleFor(x => x.Category.PageUrl)
                    .NotEmpty().WithMessage("Page Url is required")
                    .Must(CheckPageUrlIsUnique).WithMessage("Page Url already exists");
                RuleFor(x => x.Category.PageTitle)
                    .NotEmpty().WithMessage("Page Title is required");
            });
        }

        private bool CheckAcceptedImageTypes(HttpPostedFileBase uploadedFile) {
            string fileName = uploadedFile.FileName;
            string acceptedImageTypes = _settings.GetValue(Setting.Accepted_Image_Types);
            string extension = Path.GetExtension(fileName);
            return acceptedImageTypes.ToLower().Contains(extension.ToLower());
        }

        private bool CheckPageUrlIsUnique(NewsCategoryViewModel instance, string pageUrl) {
            if (string.IsNullOrEmpty(pageUrl))
                return true;

            NewsCategoryModel c = null;
            try {
                NewsCategoryGetByUrlQuery query = new NewsCategoryGetByUrlQuery { Url = pageUrl, IncludeArticles = false };
                c = _query.Process(query);
            } catch (Exception) { }

            if (c == null) {
                return true;
            } else {
                if (instance.Category.Id ==c.Id) {
                    //same page. this is fine
                    return true;
                } else {
                    //add page / edited page url, but this conflicts with another page in the db
                    return false;
                }
            }
        }
        
    }

    public static class NewsValidationRuleSets {
        public const string Default = "default";
    }
    public class NewsValidator : AbstractValidator<NewsViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;
        public NewsValidator() {

            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(BranchValidationRuleSets.Default, () => {
                RuleFor(x => x.News.Headline)
                    .NotEmpty().WithMessage("Headline is required");
                RuleFor(x => x.News.Author)
                    .NotEmpty().WithMessage("Author is required");
                RuleFor(x => x.News.ShortDescription)
                   .NotEmpty().WithMessage("Short Description is required");
                RuleFor(x => x.News.PageContent.PageContent)
                   .NotEmpty().WithMessage("Article is required");
                RuleFor(x => x.NewsImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.NewsImage != null)
                    .WithMessage("Article Image is not an accepted image type");
                RuleFor(x => x.News.PageUrl)
                    .NotEmpty().WithMessage("Page Url is required")
                    .Must(CheckPageUrlIsUnique).WithMessage("Page Url already exists");
                RuleFor(x => x.News.PageContent.PageTitle)
                    .NotEmpty().WithMessage("Page Title is required");
            });
        }

        private bool CheckAcceptedImageTypes(HttpPostedFileBase uploadedFile) {
            string fileName = uploadedFile.FileName;
            string acceptedImageTypes = _settings.GetValue(Setting.Accepted_Image_Types);
            string extension = Path.GetExtension(fileName);
            return acceptedImageTypes.ToLower().Contains(extension.ToLower());
        }

        private bool CheckPageUrlIsUnique(NewsViewModel instance, string pageUrl) {
            if (string.IsNullOrEmpty(pageUrl))
                return true;

            NewsModel a = null;
            try {
                NewsGetByUrlQuery query = new NewsGetByUrlQuery { Url = pageUrl, IncludePageContent = false };
                a = _query.Process(query);
            } catch (Exception) { }

            if (a == null) {
                return true;
            } else {
                if (instance.News.Id == a.Id) {
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
