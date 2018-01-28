using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IDynamicPageService {
        DynamicPageModel Create(DynamicPageModel model);
        DynamicPageModel Update(DynamicPageModel model);
        DynamicPageModel GetById(int id, bool includePageContent = false);
        int? GetIdByUrl(string url);
        ListOfDynamicPageModel Get(DynamicPageFilterModel filter, ListPaginationModel pagination);
        DynamicPageModel Archive(int id);
        DynamicPageModel UnArchive(int id);
    }
}
