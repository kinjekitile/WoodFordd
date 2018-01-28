using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Enums;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface IPageContentRepository {
        PageContentModel Create(PageContentModel model);
        PageContentModel Update(PageContentModel model);
        PageContentModel GetById(int id);
        PageContentModel GetByForeignKey(int id, PageContentForeignKey foreignKey);

        //PageContentModel GetByUrl(string url);
        //int? GetIdByUrl(string url);
        //int? GetForeignKeyIdByUrl(string url, PageContentForeignKey foreignKey);
        //List<PageContentModel> Get(PageContentFilterModel filter);
    }
}
