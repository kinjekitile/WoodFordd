using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class DynamicPageModel {
        public int Id { get; set; }
        public string AdminDescription { get; set; }
        public bool IsArchived { get; set; }
        public string PageUrl { get; set; }
        public PageContentModel PageContent { get; set; }
    }

    public class ListOfDynamicPageModel {
        public List<DynamicPageModel> Pages { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class DynamicPageFilterModel {
        public int? Id { get; set; }
        public bool? IsArchived { get; set; }
    }
}
