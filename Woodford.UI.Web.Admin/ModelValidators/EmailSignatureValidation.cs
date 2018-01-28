using FluentValidation;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    public static class EmailSignatureCampaignValidationRuleSets {
        public const string Default = "default";
    }
    public class EmailSignatureCampaignValidator : AbstractValidator<EmailSignatureCampaignViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;
        public EmailSignatureCampaignValidator() {

            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();

            RuleSet(EmailSignatureCampaignValidationRuleSets.Default, () => {
                RuleFor(x => x.Signature.CampaignName)
                    .NotEmpty()
                    .WithMessage("Campaign Name is required");


                string dimensionsMessage = "{0} must be {1}px wide by {2}px high";

                //Main Content
                RuleFor(x => x.MainContentImage)
                .NotNull()
                .When(x => x.Signature.MainContentFileUploadId == 0)
                .WithMessage("Main Content Image required");



                RuleFor(x => x.MainContentImage)
                .Must(x => CheckImageDimensions(x, SignatureLayout.MainWidth, SignatureLayout.MainHeight))
                .When(x => x.MainContentImage != null)
                .WithMessage(string.Format(dimensionsMessage, "Main Content", SignatureLayout.MainWidth, SignatureLayout.MainHeight));

                RuleFor(x => x.MainContentImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.MainContentImage != null)
                    .WithMessage("Main Content Image is not an accepted image type");





                //Main Content Narrow
                RuleFor(x => x.MainContentNarrowImage)
                .NotNull()
                .When(x => x.Signature.MainContentNarrowFileUploadId == 0)
                .WithMessage("Main Content Narrow Image required");

                RuleFor(x => x.MainContentNarrowImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.MainContentNarrowImage != null)
                    .WithMessage("Main Content Narrow Image is not an accepted image type");

                RuleFor(x => x.MainContentNarrowImage)
                .Must(x => CheckImageDimensions(x, SignatureLayout.MainNarrowWidth, SignatureLayout.MainNarrowHeight))
                .When(x => x.MainContentNarrowImage != null)
                .WithMessage(string.Format(dimensionsMessage, "Main Narrow Content", SignatureLayout.MainNarrowWidth, SignatureLayout.MainNarrowHeight));



                //Side Content
                RuleFor(x => x.SideContentImage)
                .NotNull()
                .When(x => x.Signature.SidePanelContentFileUploadId == 0)
                .WithMessage("Side Content Image required");

                RuleFor(x => x.SideContentImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.SideContentImage != null)
                    .WithMessage("Side Content Image is not an accepted image type");


                RuleFor(x => x.SideContentImage)
                .Must(x => CheckImageDimensions(x, SignatureLayout.SideWidth, SignatureLayout.SideHeight))
                .When(x => x.SideContentImage != null)
                .WithMessage(string.Format(dimensionsMessage, "Side Content", SignatureLayout.SideWidth, SignatureLayout.SideHeight));


                //Footer Content
                RuleFor(x => x.FooterContentImage)
                .NotNull()
                .When(x => x.Signature.FooterContentFileUploadId == 0)
                .WithMessage("Footer Content Image required");

                RuleFor(x => x.FooterContentImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.FooterContentImage != null)
                    .WithMessage("Footer Content Image is not an accepted image type");


                RuleFor(x => x.FooterContentImage)
                .Must(x => CheckImageDimensions(x, SignatureLayout.FooterWidth, SignatureLayout.FooterHeight))
                .When(x => x.FooterContentImage != null)
                .WithMessage(string.Format(dimensionsMessage, "Footer Content", SignatureLayout.FooterWidth, SignatureLayout.FooterHeight));


                //Footer Content Narrow
                RuleFor(x => x.FooterContentNarrowImage)
                .NotNull()
                .When(x => x.Signature.FooterContentNarrowFileUploadId == 0)
                .WithMessage("Footer Content Narrow Image required");

                RuleFor(x => x.FooterContentNarrowImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.FooterContentNarrowImage != null)
                    .WithMessage("Footer Content Narrow Image is not an accepted image type");


                RuleFor(x => x.FooterContentNarrowImage)
                .Must(x => CheckImageDimensions(x, SignatureLayout.FooterNarrowWidth, SignatureLayout.FooterNarrowHeight))
                .When(x => x.FooterContentNarrowImage != null)
                .WithMessage(string.Format(dimensionsMessage, "Footer Narrow Content", SignatureLayout.FooterNarrowWidth, SignatureLayout.FooterNarrowHeight));


                //Underline Content
                RuleFor(x => x.UnderlineContentImage)
                .NotNull()
                .When(x => x.Signature.UnderlineContentFileUploadId == 0)
                .WithMessage("Underline Content Image required");

                RuleFor(x => x.UnderlineContentImage)
                    .Must(CheckAcceptedImageTypes)
                    .When(x => x.UnderlineContentImage != null)
                    .WithMessage("Underline Content Image is not an accepted image type");

                RuleFor(x => x.UnderlineContentImage)
                .Must(x => CheckImageDimensions(x, SignatureLayout.UnderlineWidth, SignatureLayout.UnderlineHeight))
                .When(x => x.UnderlineContentImage != null)
                .WithMessage(string.Format(dimensionsMessage, "Underline Content", SignatureLayout.UnderlineWidth, SignatureLayout.UnderlineHeight));

            });
        }



        private bool CheckImageDimensions(HttpPostedFileBase postedFile, int x, int y) {
            using (var ms = new MemoryStream()) {
                postedFile.InputStream.CopyTo(ms);

                Image postedImage = Image.FromStream(ms);
                if (postedImage.Width != x) {
                    return false;
                }
                if (postedImage.Height != y) {
                    return false;
                }
                postedFile.InputStream.Position = 0;
                ms.Position = 0;
            }


            return true;
        }

        private bool CheckAcceptedImageTypes(HttpPostedFileBase uploadedFile) {
            string fileName = uploadedFile.FileName;
            string acceptedImageTypes = _settings.GetValue(Setting.Accepted_Image_Types);
            string extension = Path.GetExtension(fileName);
            return acceptedImageTypes.ToLower().Contains(extension.ToLower());
        }

    }

}
