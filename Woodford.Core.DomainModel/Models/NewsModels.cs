using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;

namespace Woodford.Core.DomainModel.Models {
    public class NewsCategoryModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsArchived { get; set; }
        public string PageUrl { get; set; }
        public string PageTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public ListOf<NewsModel> Articles { get; set; }
    }

    public class NewsCategoryFilterModel {
        public bool? IsArchived { get; set; }
    }

    public class NewsModel {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string ShortDescription { get; set; }
        public int? FileUploadId { get; set; }
        public FileUploadModel NewsImage { get; set; }
        public DateTime DateCreated { get; set; }
        public string Author { get; set; }
        public bool IsArchived { get; set; }
        public int NewsCategoryId { get; set; }
        public string PageUrl { get; set; }
        public PageContentModel PageContent { get; set; }
    }

    public class NewsFilterModel {
        public int? NewsCategoryId { get; set; }
        public bool? IsArchived { get; set; }
    }
}
