using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class SearchCriteriaModel {
        public int DropOffLocationId { get; set; }

        public int PickUpLocationId { get; set; }

        public DateTime PickupDateTime {
            get {
                bool isAM = PickupTimeFull.Contains("AM");

                int hours = Convert.ToInt32(PickupTimeFull.Split(new string[] { ":" }, StringSplitOptions.None)[0]);

                if (!isAM) {
                    if (hours != 12) {
                        hours += 12;
                    }
                    
                } else {
                    if (hours == 12) {
                        hours = 0;
                    }
                }
                return PickupDate.Date.AddHours(hours);
                
            }
        }

        public DateTime DropOffDateTime {
            get {
                bool isAM = DropOffTimeFull.Contains("AM");

                int hours = Convert.ToInt32(DropOffTimeFull.Split(new string[] { ":" }, StringSplitOptions.None)[0]);

                if (!isAM) {
                    if (hours != 12) {
                        hours += 12;
                    }

                }
                else {
                    if (hours == 12) {
                        hours = 0;
                    }
                }
                return DropOffDate.Date.AddHours(hours);
                
            }
        }

        public string PickupTimeFull { get; set; }
        public string DropOffTimeFull { get; set; }
        public int PickUpTimeFullInt {
            get {
                bool isAM = PickupTimeFull.Contains("AM");

                int hours = Convert.ToInt32(PickupTimeFull.Split(new string[] { ":" }, StringSplitOptions.None)[0]);

                if (!isAM) {
                    if (hours != 12) {
                        hours += 12;
                    }

                }
                else {
                    if (hours == 12) {
                        hours = 0;
                    }
                }
                return hours;
            }
        }

        public int DropOffTimeFullInt {
            get {
                bool isAM = DropOffTimeFull.Contains("AM");

                int hours = Convert.ToInt32(DropOffTimeFull.Split(new string[] { ":" }, StringSplitOptions.None)[0]);

                if (!isAM) {
                    if (hours != 12) {
                        hours += 12;
                    }

                }
                else {
                    if (hours == 12) {
                        hours = 0;
                    }
                }
                return hours;
            }
        }

        public DateTime PickupDate { get; set; }
        public int PickupTime { get; set; }
        public DateTime DropOffDate { get; set; }
        public int DropOffTime { get; set; }
        public bool IsLoggedIn { get; set; }
        public int? UserId { get; set; }
        public bool IsAdvance { get; set; }

        public bool IsCorporate { get; set; }
        public int? CorporateId { get; set; }
        public int? CampaignId { get; set; }
        public int? MinimumDays { get; set; }

        public int? VehicleId { get; set; }
        public VehicleGroupType? VehicleGroupType { get; set; }

        public int NumberOfDays {
            get {
                int dropOffGrace = 1; //1 hour
                double hoursDiff = (DropOffDateTime - PickupDateTime).TotalHours;
                hoursDiff = hoursDiff - dropOffGrace;
                int result = Convert.ToInt32(Math.Ceiling(hoursDiff / 24));
                return result;
                
            }
        }



    }

    public class SearchResultsModel {
        public SearchCriteriaModel Criteria { get; set; }

        public List<SearchResultItemModel> Items { get; set; }

        public SearchResultsModel() {
            Items = new List<SearchResultItemModel>();
        }
    }

    public class SearchResultItemModel {

        public VehicleModel Vehicle { get; set; }
        public string VehicleGroupName { get; set; }

        public List<SearchResultItemRateModel> Rates { get; set; }

        public SearchResultItemModel() {
            Rates = new List<SearchResultItemRateModel>();
        }
    }

    public class SearchResultItemRateModel {
        public int RateId { get; set; }
        public decimal Price { get; set; }
        public int RateCodeId { get; set; }
        public string RateCodeTitle { get; set; }
        public string RateCodeColor { get; set; }
        public bool RateCodeIsSticky { get; set; }
        public RateAdjustmentType? AdjustmentType { get; set; }
        public int? AdjustmentId { get; set; }
        public decimal? AdjustmentPercentage { get; set; }

        public SearchPriceCalculationModel PriceCalculation { get; set; }
        public bool CanUseCountdownSpecial { get; set; }

        public List<CampaignModel> Campaigns { get; set; }

        public bool HasLiveCampaign {
            get {
                if (Campaigns == null)
                    return false;
                if (Campaigns.Count == 0)
                    return false;

                var latestCampaign = Campaigns.Where(x => x.StartDate <= DateTime.Today && x.EndDate >= DateTime.Today && !x.IsArchived).OrderByDescending(x => x.Id).FirstOrDefault();
                if (latestCampaign == null) {
                    return false;
                } else {
                    return true;
                }

                return false;
            }
        }

        public int? LiveCampaignIconFileUploadId {
            get {
                if (Campaigns != null) {
                    if (Campaigns.Count > 0) {
                        var latestCampaign = Campaigns.Where(x => x.StartDate <= DateTime.Today && x.EndDate >= DateTime.Today && !x.IsArchived).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (latestCampaign == null) {
                            return (int?)null;
                        } else {
                            //TODO new fileupload id
                            return latestCampaign.SearchResultIconFileUploadId;
                            throw new NotImplementedException();
                        }
                    } else {
                        return (int?)null;
                    }
                } else {
                    return (int?)null;
                }
            }
        }



    }

    public class SearchPriceCalculationModel {
        public int NumberOfDays { get; set; }
        public decimal RatePrice { get; set; }
        public decimal RateAdjustmentPercentage { get; set; }
        public decimal AdjustedPrice {
            get {
                return RatePrice + (RatePrice * (RateAdjustmentPercentage / 100));
            }
        }
        public decimal TaxAmount {
            get {
                return (AdjustedPrice * NumberOfDays) * (TaxRate / 100);
            }
        }
        public decimal DropOffFee { get; set; }
        public decimal TaxRate { get; set; }
        public decimal AdminFee { get; set; }

        public decimal BookingPrice {
            get {
                return (AdjustedPrice * NumberOfDays) + TaxAmount + DropOffFee;

            }
        }
    }
    
}
