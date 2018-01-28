using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Woodford.Core.DomainModel.Models;

namespace Woodford.UI.Web.Public.ViewModels {
    public class NavigationViewModel {
        public List<NewsCategoryModel> NewsCategories { get; set; }
    }
}