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

    public static class ReportValidationRuleSets {
        public const string Default = "default";
    }
    public class ReportValidation : AbstractValidator<ReservationSearchViewModel> {

        public ReportValidation() {
            RuleSet(ReportValidationRuleSets.Default, () => {
                RuleFor(x => x.Report.Title)
                .NotEmpty().WithMessage("Title is required");
                RuleFor(x => x.Report.DateUnitsToAdd)
                .NotEmpty().WithMessage("Date Units is required");
                RuleFor(x => x.Report.DateUnitType)
                .NotNull().WithMessage("Date Unit Type is required");

            });
        }


    }
}