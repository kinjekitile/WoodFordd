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

    public static class ReservationValidationRuleSets {
        public const string Search = "default";
        public const string Report = "report";
    }
    public class ReservationValidation : AbstractValidator<ReservationSearchViewModel> {

        public ReservationValidation() {
            RuleSet(ReservationValidationRuleSets.Search, () => {

                When(x => x.Filter.DateFilterType != Core.DomainModel.Enums.ReservationDateFilterTypes.None, () => {
                    RuleFor(x => x.Filter.DateSearchEnd).GreaterThan(x => x.Filter.DateSearchStart)
                    .WithMessage("End date must be after start date");
                });

            });

            RuleSet(ReservationValidationRuleSets.Report, () => {
                RuleFor(x => x.Report.Title)
                   .NotEmpty().WithMessage("Title is required");

                When(x => x.Report.UseCurrentDateAsStartDate, () => {
                    RuleFor(x => x.Report.DateUnitsToAdd)
                    .NotEmpty().WithMessage("Date Units is required");
                    RuleFor(x => x.Report.DateUnitType)
                    .NotEmpty().WithMessage("Unit Type is required");

                });

            });
        }


    }
}