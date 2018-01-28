using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class PageContentModel {
        public int Id { get; set; }
        public string PageTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string PageContent { get; set; }
        public int? DynamicPageId { get; set; }
        public int? VehicleGroupId { get; set; }
        public int? VehicleId { get; set; }
        public int? NewsId { get; set; }
        public int? CampaignId { get; set; }
        public int? BranchId { get; set; }

        public int? VehicleManufacturerId { get; set; }

    }  
}
