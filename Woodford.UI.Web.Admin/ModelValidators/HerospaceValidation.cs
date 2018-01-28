using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.ModelValidators {
    public static class HerospaceValidationRuleSets {
        public const string Default = "default";
    }
    public class HerospaceValidator : AbstractValidator<HerospaceItemViewModel> {
        private readonly ISettingService _settings;
        public HerospaceValidator() {

            _settings = MvcApplication.Container.GetInstance<ISettingService>();

            RuleSet(VehicleGroupValidationRuleSets.Default, () => {
                RuleFor(x => x.Item.Title)
                    .NotEmpty().WithMessage("Title is required");

                RuleFor(x => x.HerospaceImage)
                    .NotNull().WithMessage("Herospace image is required")
                    .When(x => x.Item.Id == 0)
                    .Must(CheckAcceptedImageTypes)                    
                    .WithMessage("Herospace image is not an accepted image type");
            });
        }

        private bool CheckAcceptedImageTypes(HttpPostedFileBase uploadedFile) {
            if (uploadedFile == null) return true;
            string fileName = uploadedFile.FileName;
            string acceptedImageTypes = _settings.GetValue(Setting.Accepted_Image_Types);
            string extension = Path.GetExtension(fileName);
            return acceptedImageTypes.ToLower().Contains(extension.ToLower());
        }
    }
}
