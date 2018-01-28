using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Public.ModelValidators;

namespace Woodford.UI.Web.Public.ViewModels {

    public class ReservationQuoteContactDetailsModel {
        public int ReservationId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }

    [Validator(typeof(ReservationValidator))]
    public class ReservationViewModel {
        public ReservationModel Reservation { get; set; }
        public List<WaiverModel> Waivers { get; set; }
    }


    public class ReservationModifyViewModel {
        public ReservationModel Reservation { get; set; }
        public ReservationModel Modified { get; set; }

        public SearchCriteriaModel Criteria { get; set; }
    }


    public class ReservationModifyVehicleViewModel {
        public int ReservationId { get; set; }
        public string ReservationPin { get; set; }
    }
    public class ReservationModifyDateViewModel {
        public ReservationModel Reservation { get; set; }
        public int ReservationId { get; set; }
        public string ReservationPin { get; set; }
        public DateTime ModifyPickupDate { get; set; }

        public DateTime ModifyDropOffDate { get; set; }

        public int ModifyPickupTime { get; set; }
        public int ModifyDropOffTime { get; set; }
        public List<SearchResultItemRateModel> AlternateRates { get; set; }
        public List<SearchResultItemModel> AlternateResults { get; set; }
        public ReservationModifyDateViewModel() {
            AlternateRates = new List<SearchResultItemRateModel>();
            AlternateResults = new List<SearchResultItemModel>();
        }
    }
}