using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class HerospaceItemModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public bool IsArchived { get; set; }
        public int FileUploadId { get; set; }
        public int SortOrder { get; set; }
        public FileUploadModel HerospaceImage { get; set; }
    }

    public class HerospaceItemFilterModel {
        public bool? IsArchived { get; set; }
    }
}
