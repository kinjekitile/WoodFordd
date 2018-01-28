using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;

namespace Woodford.UI.Web.Public.ViewModels {
    public class CampaignViewModel {
        public CampaignModel Campaign { get; set; }

        public SearchResultsViewModel Search { get; set; }
    }
}