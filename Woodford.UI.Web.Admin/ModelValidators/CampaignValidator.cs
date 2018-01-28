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

    public static class CampaignValidatorRuleSets {
        public const string Default = "default";
    }
    public class CampaignValidator : AbstractValidator<CampaignViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;

        public CampaignValidator() {

            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();
            _settings = MvcApplication.Container.GetInstance<ISettingService>();

            RuleSet(CampaignValidatorRuleSets.Default, () => {
                RuleFor(x => x.Campaign.Title)
                    .NotEmpty().WithMessage("Title is required");

                RuleFor(x => x.CampaignImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.CampaignImage != null)
                    .WithMessage("Campaign Image is not an accepted image type");

                RuleFor(x => x.Campaign.StartDate)
                    .NotEmpty().WithMessage("Start Date is required");

                RuleFor(x => x.Campaign.EndDate)
                    .NotEmpty().WithMessage("End Date is required")
                    .GreaterThan(x => x.Campaign.StartDate).WithMessage("End date must be after start date");

                RuleFor(x => x.Campaign.RateCodeId)
                    .NotNull().WithMessage("Rate Code is required");

                RuleFor(x => x.Campaign.PageUrl)
                    .NotEmpty().WithMessage("Page Url is required")
                    .Must(CheckPageUrlIsUnique).WithMessage("Page Url already exists");

                RuleFor(x => x.Campaign.PageContent.PageTitle)
                .NotEmpty().WithMessage("Page Title is required");
            });
        }

        private bool CheckPageUrlIsUnique(CampaignViewModel instance, string pageUrl) {
            if (string.IsNullOrEmpty(pageUrl))
                return true;

            CampaignModel c = null;
            try {
                CampaignGetByUrlQuery query = new CampaignGetByUrlQuery { Url = pageUrl, IncludePageContent = false };
                c = _query.Process(query);
            } catch (Exception) {}            

            if (c == null) {
                return true;
            } else {
                if (instance.Campaign.Id == c.Id) {
                    //same page. this is fine
                    return true;
                } else {
                    //add page / edited page url, but this conflicts with another page in the db
                    return false;
                }
            }
        }

        private bool CheckAcceptedImageTypes(HttpPostedFileBase uploadedFile) {
            string fileName = uploadedFile.FileName;
            string acceptedImageTypes = _settings.GetValue(Setting.Accepted_Image_Types);
            string extension = Path.GetExtension(fileName);
            return acceptedImageTypes.ToLower().Contains(extension.ToLower());
        }


    }
}