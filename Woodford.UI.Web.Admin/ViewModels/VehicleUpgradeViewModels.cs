using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(VehicleUpgradeValidator))]
    public class VehicleUpgradeViewModel {
        public VehicleUpgradeModel VehicleUpgrade { get; set; }
        public int? p { get; set; }
    }
}
