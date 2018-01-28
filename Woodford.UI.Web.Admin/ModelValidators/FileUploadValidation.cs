using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Admin.ViewModels;

namespace Woodford.UI.Web.Admin.ModelValidators {
    public static class FileUploadValidationRuleSets {
        public const string Default = "default";
    }
    public class FileUploadValidator : AbstractValidator<FileUploadViewModel> {        
        public FileUploadValidator() {
            
            RuleSet(VehicleGroupValidationRuleSets.Default, () => {
                RuleFor(x => x.File.Title)
                    .NotEmpty().WithMessage("Title is required");
                RuleFor(x => x.Upload)
                    .NotNull().WithMessage("Upload is required")
                    .When(x => x.File.Id == 0);
            });
        }        
    }    
}
