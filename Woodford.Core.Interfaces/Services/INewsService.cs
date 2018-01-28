using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface INewsCategoryService {
        NewsCategoryModel Create(NewsCategoryModel model);
        NewsCategoryModel Update(NewsCategoryModel model);
        NewsCategoryModel GetById(int id, bool includeArticles = false, ListPaginationModel pagination = null);
        NewsCategoryModel GetByUrl(string url, bool includeArticles = false, ListPaginationModel pagination = null);
        ListOf<NewsCategoryModel> Get(NewsCategoryFilterModel filter, ListPaginationModel pagination);
        NewsCategoryModel MarkAs(int id, bool markAs);
    }

    public interface INewsService {
        NewsModel Create(NewsModel model);
        NewsModel Update(NewsModel model);
        NewsModel GetById(int id, bool includePageContent = false);
        NewsModel GetByUrl(string url, bool includePageContent = false);
        ListOf<NewsModel> Get(NewsFilterModel filter, ListPaginationModel pagination);
        NewsModel MarkAs(int id, bool markAs);
    }
}
