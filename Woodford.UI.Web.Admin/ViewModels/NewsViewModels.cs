using FluentValidation.Attributes;
using System.Web;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.UI.Web.Admin.ModelValidators;

namespace Woodford.UI.Web.Admin.ViewModels {
    [Validator(typeof(NewsCategoryValidator))]    
    public class NewsCategoryViewModel {
        public NewsCategoryModel Category { get; set; }        
    }

    [Validator(typeof(NewsValidator))]
    public class NewsViewModel {
        public NewsModel News { get; set; }
        public HttpPostedFileBase NewsImage { get; set; }
        public NewsCategoryModel NewsCategory { get; set; }
    }

    public class NewsListViewModel {
        public ListOf<NewsModel> Articles { get; set; }
        public NewsCategoryModel NewsCategory { get; set; }
    }
}
