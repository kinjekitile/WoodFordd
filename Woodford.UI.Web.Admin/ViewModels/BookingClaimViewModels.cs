using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    
    public class BookingClaimViewModel {
        public BookingClaimModel Claim { get; set; }
        public string ExternalId { get; set; }
        public UserModel User { get; set; }
    }
}