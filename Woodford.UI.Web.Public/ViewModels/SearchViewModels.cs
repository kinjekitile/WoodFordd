using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Public.ModelValidators;

namespace Woodford.UI.Web.Public.ViewModels {
    [Validator(typeof(SearchValidator))]
    public class SearchCriteriaViewModel {
        public SearchCriteriaModel Criteria { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                             ApplyFormatInEditMode = true)]
        public DateTime PickupDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                             ApplyFormatInEditMode = true)]
        public DateTime DropOffDate { get; set; }

        public string AirportLocationIds { get; set; }

        public bool IsReturnDifferentLocation { get; set; }

        public DateTime Foobar { get; set; }
    }

    public class SearchResultsViewModel {
        public ReservationModel Reservation { get; set; }
        public SearchCriteriaViewModel Criteria { get; set; }
        public SearchResultsModel Results { get; set; }

        public CountdownSpecialModel CountdownSpecial { get; set; }

        public DateTime CountDownSpecialExpiry { get; set; }

        //public SearchResultsViewModel() {
        //    Criteria.Criteria.PickupTime = DateTime.Now.Hour + 2;
        //    Criteria.Criteria.DropOffTime = DateTime.Now.Hour + 2;
        //    int minLeadTime = Convert.ToInt32(ConfigurationManager.AppSettings["minLeadTime"]);
        //    PickUpDate = DateTime.Now.Date.AddHours(Math.Max(minLeadTime, 24));
        //    DropOffDate = PickUpDate.AddDays(2);

        //    int primaryLocationId = Convert.ToInt32(ConfigurationManager.AppSettings["PrimaryLocationId"]);
        //    PickUpLocationId = primaryLocationId;
        //    DropOffLocationId = primaryLocationId;
        //}
    }
}