using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    public class VoucherFilterAndResultsViewModel {
        public ListOf<VoucherModel> Vouchers { get; set; }
        public bool IsFiltered { get; set; }
        public bool IsRedeemed { get; set; }
        public bool IsExpired { get; set; }
        public bool IsMultiUse { get; set; }    

    }

    [Validator(typeof(VoucherValidator))]
    public class VoucherViewModel {
        public VoucherModel Voucher { get; set; }
    }
}
