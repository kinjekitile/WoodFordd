using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class VehicleManufacturerModel {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public string PageUrl { get; set; }
        public PageContentModel PageContent { get; set; }
        public int? FileUploadId { get; set; }
        public FileUploadModel ManufacturerImage { get; set; }
    }

    public class VehicleManufacturerFilterModel {
        public int? Id { get; set; }
        public string PageUrl { get; set; }

    }
}
