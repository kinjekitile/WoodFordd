using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(EmailSignatureCampaignValidator))]
    public class EmailSignatureCampaignViewModel {
        public EmailSignatureCampaignModel Signature { get; set; }

        public HttpPostedFileBase MainContentImage { get; set; }

        public HttpPostedFileBase MainContentNarrowImage { get; set; }
        public HttpPostedFileBase FooterContentImage { get; set; }
        public HttpPostedFileBase FooterContentNarrowImage { get; set; }
        public HttpPostedFileBase SideContentImage { get; set; }
        public HttpPostedFileBase UnderlineContentImage { get; set; }
    }
}