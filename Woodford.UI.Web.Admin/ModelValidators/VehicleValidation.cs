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

    public static class VehicleManufacturerValidationRuleSets {
        public const string Default = "default";
    }

    public class VehicleManufacturerValidator : AbstractValidator<VehicleManufacturerViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;
        public VehicleManufacturerValidator() {
            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(VehicleManufacturerValidationRuleSets.Default, () => {
                RuleFor(x => x.Manufacturer.Title)
                .NotEmpty().WithMessage("Title is required");
                RuleFor(x => x.Manufacturer.PageUrl)
                .NotEmpty().WithMessage("Page Url is required");
                RuleFor(x => x.Manufacturer.PageUrl)
                    .NotEmpty().WithMessage("Page Url is required")
                    .Must(CheckPageUrlIsUnique).WithMessage("Page Url already exists");
                RuleFor(x => x.Manufacturer.PageContent.PageTitle)
                    .NotEmpty().WithMessage("Page Title is required");
            });
        }

        private bool CheckPageUrlIsUnique(VehicleManufacturerViewModel instance, string pageUrl) {
            if (string.IsNullOrEmpty(pageUrl))
                return true;

            VehicleManufacturerModel vg = null;
            try {
                VehicleManufacturerGetByUrlQuery query = new VehicleManufacturerGetByUrlQuery { Url = pageUrl, IncludePageContent = false };
                vg = _query.Process(query);
            }
            catch (Exception) { }

            if (vg == null) {
                return true;
            }
            else {
                if (instance.Manufacturer.Id == vg.Id) {
                    //same page. this is fine
                    return true;
                }
                else {
                    //add page / edited page url, but this conflicts with another page in the db
                    return false;
                }
            }
        }
    }


    public static class VehicleGroupValidationRuleSets {
        public const string Default = "default";
    }
    public class VehicleGroupValidator : AbstractValidator<VehicleGroupViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;
        public VehicleGroupValidator() {

            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(VehicleGroupValidationRuleSets.Default, () => {
                RuleFor(x => x.VehicleGroup.Title)
                    .NotEmpty().WithMessage("Vehicle Group name is required");
                RuleFor(x => x.VehicleGroup.TitleDescription)
                    .NotEmpty().WithMessage("Vehicle Group Description is required");
                RuleFor(x => x.VehicleGroupImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.VehicleGroupImage != null)
                    .WithMessage("Vehicle Group Image is not an accepted image type");
                RuleFor(x => x.VehicleGroup.PageUrl)
                    .NotEmpty().WithMessage("Page Url is required")
                    .Must(CheckPageUrlIsUnique).WithMessage("Page Url already exists");
                RuleFor(x => x.VehicleGroup.PageContent.PageTitle)
                    .NotEmpty().WithMessage("Page Title is required");
            });
        }

        private bool CheckAcceptedImageTypes(HttpPostedFileBase uploadedFile) {
            string fileName = uploadedFile.FileName;
            string acceptedImageTypes = _settings.GetValue(Setting.Accepted_Image_Types);
            string extension = Path.GetExtension(fileName);
            return acceptedImageTypes.ToLower().Contains(extension.ToLower());
        }

        private bool CheckPageUrlIsUnique(VehicleGroupViewModel instance, string pageUrl) {
            if (string.IsNullOrEmpty(pageUrl))
                return true;

            VehicleGroupModel vg = null;
            try {
                VehicleGroupGetByUrlQuery query = new VehicleGroupGetByUrlQuery { Url = pageUrl, IncludePageContent = false };
                vg = _query.Process(query);
            }
            catch (Exception) { }

            if (vg == null) {
                return true;
            } else {
                if (instance.VehicleGroup.Id == vg.Id) {
                    //same page. this is fine
                    return true;
                } else {
                    //add page / edited page url, but this conflicts with another page in the db
                    return false;
                }
            }
        }
    }


    public static class VehicleValidationRuleSets {
        public const string Default = "default";
    }
    public class VehicleValidator : AbstractValidator<VehicleViewModel> {

        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;

        public VehicleValidator() {

            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(VehicleGroupValidationRuleSets.Default, () => {
                RuleFor(x => x.Vehicle.Title)
                    .NotEmpty().WithMessage("Vehicle name is required");
                RuleFor(x => x.Vehicle.VehicleGroupId)
                    .GreaterThan(0).WithMessage("Please select a Vehicle Group");
                RuleFor(x => x.VehicleImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.VehicleImage != null)
                    .WithMessage("Vehicle Image is not an accepted image type");
                RuleFor(x => x.VehicleImage2)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.VehicleImage2 != null)
                    .WithMessage("Interior Vehicle Image is not an accepted image type");
                RuleFor(x => x.Vehicle.PageUrl)
                    .NotEmpty().WithMessage("Page Url is required")
                    .Must(CheckPageUrlIsUnique).WithMessage("Page Url already exists");
                RuleFor(x => x.Vehicle.PageContent.PageTitle)
                    .NotEmpty().WithMessage("Page Title is required");
            });
        }

        private bool CheckAcceptedImageTypes(HttpPostedFileBase uploadedFile) {
            string fileName = uploadedFile.FileName;
            string acceptedImageTypes = _settings.GetValue(Setting.Accepted_Image_Types);
            string extension = Path.GetExtension(fileName);
            return acceptedImageTypes.ToLower().Contains(extension.ToLower());
        }

        private bool CheckPageUrlIsUnique(VehicleViewModel instance, string pageUrl) {
            if (string.IsNullOrEmpty(pageUrl))
                return true;

            VehicleModel v = null;
            try {
                VehicleGetByUrlQuery query = new VehicleGetByUrlQuery { Url = pageUrl, IncludePageContent = false };
                v = _query.Process(query);
            }
            catch (Exception) { }

            if (v == null) {
                return true;
            } else {
                if (instance.Vehicle.Id == v.Id) {
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
