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
    public static class BranchValidationRuleSets {
        public const string Default = "default";
    }
    public class BranchValidator : AbstractValidator<BranchViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;
        public BranchValidator() {

            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(BranchValidationRuleSets.Default, () => {
                RuleFor(x => x.Branch.Title)
                    .NotEmpty().WithMessage("Branch name is required");
                RuleFor(x => x.Branch.Email)
                    .NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Please enter a valid email address");
                RuleFor(x => x.Branch.TelephoneNumber)
                    .NotEmpty().WithMessage("Branch telephone is required");
                RuleFor(x => x.BranchImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.BranchImage != null)
                    .WithMessage("Branch Image is not an accepted image type");
                RuleFor(x => x.Branch.PageUrl)
                    .NotEmpty().WithMessage("Page Url is required")
                    .Must(CheckPageUrlIsUnique).WithMessage("Page Url already exists");
                RuleFor(x => x.Branch.PageContent.PageTitle)
                    .NotEmpty().WithMessage("Page Title is required");
            });
        }

        private bool CheckAcceptedImageTypes(HttpPostedFileBase uploadedFile) {
            string fileName = uploadedFile.FileName;
            string acceptedImageTypes = _settings.GetValue(Setting.Accepted_Image_Types);
            string extension = Path.GetExtension(fileName);
            return acceptedImageTypes.ToLower().Contains(extension.ToLower());
        }

        private bool CheckPageUrlIsUnique(BranchViewModel instance, string pageUrl) {
            if (string.IsNullOrEmpty(pageUrl))
                return true;

            BranchModel b = null;
            try {
                BranchGetByUrlQuery query = new BranchGetByUrlQuery { Url = pageUrl, IncludePageContent = false };
                b = _query.Process(query);
            } catch (Exception) { }

            if (b == null) {
                return true;
            } else {
                if (instance.Branch.Id == b.Id) {
                    //same page. this is fine
                    return true;
                } else {
                    //add page / edited page url, but this conflicts with another page in the db
                    return false;
                }
            }
        }
        
    }
    public static class InterBranchDropOffFeeValidationRuleSets {
        public const string Default = "default";
    }
    public class InterBranchDropOffFeeValidator : AbstractValidator<InterBranchDropOffFeeViewModel> {
        public InterBranchDropOffFeeValidator() {


            RuleSet(InterBranchDropOffFeeValidationRuleSets.Default, () => {
                RuleFor(x => x.DropOff.Branch1Id)
                    .NotEmpty().WithMessage("Branch 1 is required");
                RuleFor(x => x.DropOff.Branch2Id)
                    .NotEmpty().WithMessage("Branch 2 is required");
                RuleFor(x => x.DropOff.Price)
                    .NotEmpty().WithMessage("Price is required");
            });
        }
    }
}
