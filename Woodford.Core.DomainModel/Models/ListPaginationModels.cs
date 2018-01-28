using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woodford.Core.DomainModel.Models {
    public class ListPaginationModel {
        public bool ShowPagination { get {
                return TotalItems > ItemsPerPage;
            }
        }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int SkipItems {
            get {
                return (CurrentPage - 1) * ItemsPerPage;
            }
        }

        public string UrlGetParameters { get; set; }
        public string UrlAction { get; set; }
        public int TotalPages {
            get {
                return Convert.ToInt32(Math.Ceiling((double)TotalItems / (double)ItemsPerPage));
            }
        }
    }
}
