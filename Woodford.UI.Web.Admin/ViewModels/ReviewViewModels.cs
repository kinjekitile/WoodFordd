using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;

namespace Woodford.UI.Web.Admin.ViewModels {
    public class ReviewSearchViewModels {
        public ReviewFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
        public List<ReviewModel> Items { get; set; }
    }
}