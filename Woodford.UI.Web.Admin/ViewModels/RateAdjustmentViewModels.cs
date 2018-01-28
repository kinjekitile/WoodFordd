using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {

    [Validator(typeof(RateAdjustmentValidator))]
    public class RateAdjustmentViewModel {
        public RateAdjustmentModel RateAdjustment { get; set; }
    }

    public class RateAdjustmentListViewModel {
        public bool ShowPastAdjustments { get; set; }
        public List<RateAdjustmentModel> Items { get; set; }
        public RateAdjustmentListViewModel() {
            Items = new List<RateAdjustmentModel>();
        }
    }
}
