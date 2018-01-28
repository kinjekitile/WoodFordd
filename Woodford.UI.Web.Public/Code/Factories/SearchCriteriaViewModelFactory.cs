using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;
using Woodford.UI.Web.Public.ViewModels;

namespace Woodford.UI.Web.Public.Code {
    public static class SearchCriteriaViewModelFactory {
        public static SearchCriteriaViewModel CreateInstance(int? campaignId = null) {
            //GetInstance

            ISettingService settings = MvcApplication.Container.GetInstance<ISettingService>();
            IBranchService branchService = MvcApplication.Container.GetInstance<IBranchService>();

            int minLeadTime = Convert.ToInt32(settings.Get(Setting.Booking_Lead_Time_Hours).Value);
            int defaultLocationId = Convert.ToInt32(settings.Get(Setting.Default_Location_Id).Value);

            SearchCriteriaViewModel res = new SearchCriteriaViewModel();
            res.PickupDate = DateTime.Now.Date.AddHours(Math.Max(minLeadTime, 24));
            res.DropOffDate = res.PickupDate.AddDays(2);
            res.Criteria = new SearchCriteriaModel();
            res.Criteria.PickupDate = res.PickupDate;
            res.Criteria.DropOffDate = res.DropOffDate;
            res.Criteria.PickupTime = DateTime.Now.Hour + 2;
            res.Criteria.DropOffTime = DateTime.Now.Hour + 2;
            res.Criteria.PickupTimeFull = DateTime.Now.AddHours(2).ToString("hh:00 tt");
            res.Criteria.DropOffTimeFull = DateTime.Now.AddHours(2).ToString("hh:00 tt");
            res.Criteria.PickUpLocationId = defaultLocationId;
            res.Criteria.DropOffLocationId = defaultLocationId;
            if (campaignId.HasValue) {
                res.Criteria.CampaignId = campaignId.Value;
            }

            ListOf<BranchModel> airports = branchService.Get(new BranchFilterModel { IsAirport = true }, null);

            res.AirportLocationIds = string.Join(",", airports.Items.Select(x => x.Id).ToArray());
            return res;
        }
    }
}