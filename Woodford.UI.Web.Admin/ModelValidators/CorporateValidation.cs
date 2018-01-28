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

    public static class CorporateValidationRuleSets {
        public const string Default = "default";
    }
    public class CorporateValidator : AbstractValidator<CorporateViewModel> {
        public CorporateValidator() {
            RuleSet(CorporateValidationRuleSets.Default, () => {
                RuleFor(x => x.Corporate.Title)
                    .NotEmpty().WithMessage("Title is required");
            });
        }
    }
}