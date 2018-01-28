using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class CampaignModel {
        public int Id { get; set; }
        public int RateCodeId { get; set; }
        public RateCodeModel RateCode { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string PageUrl { get; set; }
        public int? FileUploadId { get; set; }
        public int? SearchResultIconFileUploadId { get; set; }
        public bool IsArchived { get; set; }
        public FileUploadModel CampaignImage { get; set; }
        public FileUploadModel SearchResultIconImage { get; set; }
        public PageContentModel PageContent { get; set; }

        public CampaignModel() {
            RateCode = new RateCodeModel();
        }
    }

    public class CampaignFilterModel {
        public int? RateCodeId { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsArchived { get; set; }
        
    }
}
