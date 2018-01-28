using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class BranchModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? FileUploadId { get; set; }
        public FileUploadModel BranchImage { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsAirport { get; set; }
        public bool IsArchived { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string MapEmbed { get; set; }
        public string AfterHoursTelephoneNumber { get; set; }
        public string PageUrl { get; set; }
        public PageContentModel PageContent { get; set; }
        public string BranchSubTitle { get; set; }
        public string BranchAddress { get; set; }
        public string BranchShortDescription { get; set; }
        public string BranchContactPageTitle { get; set; }
        public string BranchContactMetaKeywords { get; set; }
        public string BranchContactMetaDescription { get; set; }
        public string BranchContactPageContent { get; set; }
        public string BranchRequestCallbackPageTitle { get; set; }
        public string BranchRequestCallbackMetaKeywords { get; set; }
        public string BranchRequestCallbackMetaDescription { get; set; }
        public string BranchRequestCallbackPageContent { get; set; }

        public string WeatherApiId { get; set; }

        public int? BookingLeadTimeDay { get; set; }
        public int? BookingLeadTimeNight { get; set; }

        //Field is used for Dashboard only
        public int TotalPickups { get; set; }
    }

    //public class ListOfBranches {
    //    public List<BranchModel> Branches { get; set; }
    //    public ListPaginationModel Pagination { get; set; }
    //}

    public class BranchFilterModel {
        public int? Id { get; set; }
        public List<int> Ids { get; set; }
        public bool? IsAirport { get; set; }
        public bool? IsArchived { get; set; }
    }
}
