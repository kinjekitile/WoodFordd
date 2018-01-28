using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.DomainModel.Common {
    public class ListOf<T> : List<T> {
        public List<T> Items { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }
}
