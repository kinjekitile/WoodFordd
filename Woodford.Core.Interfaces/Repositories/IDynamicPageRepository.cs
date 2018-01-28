using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IDynamicPageRepository {
        DynamicPageModel Create(DynamicPageModel model);
        DynamicPageModel Update(DynamicPageModel model);
        DynamicPageModel GetById(int id);
        //DynamicPageModel GetByUrl(string url);
        List<DynamicPageModel> Get(DynamicPageFilterModel filter, ListPaginationModel pagination);
        int GetCount(DynamicPageFilterModel filter);
        int? GetIdByUrl(string url);
    }
}
