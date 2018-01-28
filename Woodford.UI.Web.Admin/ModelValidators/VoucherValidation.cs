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
    public static class VoucherValidationRuleSets {
        public const string Default = "default";
    }
    public class VoucherValidator : AbstractValidator<VoucherViewModel> {        
        public VoucherValidator() {            
            RuleSet(VoucherValidationRuleSets.Default, () => {                
            });
        }
        
    }    
}
