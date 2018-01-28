using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(CorporateValidator))]
    public class CorporateViewModel {
        public CorporateModel Corporate { get; set; }
        public ListOf<UserModel> Users { get; set; }
        public List<RateCodeModel> CorporateRateCodes { get; set; }
        public ListOf<ReservationModel> Reservations { get; set; }

        public CorporateViewModel() {
            CorporateRateCodes = new List<RateCodeModel>();
            Reservations = new ListOf<ReservationModel>();
        }
    }

    public class CorporateRateCodesViewModel {
        public int CorporateId { get; set; }
        public List<RateCodeModel> AllRateCodesAvailableToCorporates { get; set; }


        public List<RateCodeModel> CorporateRateCodes { get; set; }
        public bool Filtered { get; set; }
        public bool Updated { get; set; }
        public CorporateRateCodesViewModel() {
            AllRateCodesAvailableToCorporates = new List<RateCodeModel>();
            CorporateRateCodes = new List<RateCodeModel>();
        }
    }
}