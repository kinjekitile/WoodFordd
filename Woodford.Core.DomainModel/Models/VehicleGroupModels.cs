using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;

namespace Woodford.Core.DomainModel.Models {
    public class VehicleGroupModel {
        public int Id { get; set; }

        public VehicleGroupType GroupType { get; set; }
        public string Title { get; set; }
        public string TitleDescription { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsArchived { get; set; }
        public int? FileUploadId { get; set; }
        public FileUploadModel VehicleGroupImage { get; set; }
        public string PageUrl { get; set; }
        public PageContentModel PageContent { get; set; }
        public List<VehicleModel> Vehicles { get; set; }
        public int SortOrder { get; set; }
    }

    //public class ListOf<VehicleGroupModel> {
    //    public List<VehicleGroupModel> Groups { get; set; }
    //    public ListPaginationModel Pagination { get; set; }
    //}

    public class VehicleGroupFilterModel {
        public int? Id { get; set; }
        public List<int> Ids { get; set; }
        public VehicleGroupType? GroupType { get; set; }

        public bool? IsArchived { get; set; }
        public VehicleGroupFilterModel() {
            Ids = new List<int>();
 
        }
    }
}
