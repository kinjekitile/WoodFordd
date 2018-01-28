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
    public static class BranchSurchargeValidationRuleSets {
        public const string Default = "default";
    }
    public class BranchSurchargeValidation : AbstractValidator<BranchSurchargeViewModel> {
        private readonly IQueryProcessor _query;
        private readonly ISettingService _settings;


        public BranchSurchargeValidation() {
            _settings = MvcApplication.Container.GetInstance<ISettingService>();
            _query = MvcApplication.Container.GetInstance<IQueryProcessor>();


            RuleSet(BranchSurchargeValidationRuleSets.Default, () => {
                RuleFor(x => x.Surcharge.Title)
                .NotEmpty().WithMessage("Name is required");
                RuleFor(x => x.Surcharge.SurchargeAmount)
                .GreaterThan(0).WithMessage("Must be greater than 0");
                RuleFor(x => x.Surcharge.BranchId)
                .NotEmpty().WithMessage("Branch is required");

            });
        }


    }
}