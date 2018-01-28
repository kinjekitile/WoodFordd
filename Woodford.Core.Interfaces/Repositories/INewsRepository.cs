using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Models;

namespace Woodford.Core.Interfaces {
    public interface INewsCategoryRepository {
        NewsCategoryModel Create(NewsCategoryModel model);
        NewsCategoryModel Update(NewsCategoryModel model);
        NewsCategoryModel GetById(int id);
        NewsCategoryModel GetByUrl(string url);
        List<NewsCategoryModel> Get(NewsCategoryFilterModel filter, ListPaginationModel pagination);
        int GetCount(NewsCategoryFilterModel filter);
    }

    public interface INewsRepository {
        NewsModel Create(NewsModel model);
        NewsModel Update(NewsModel model);
        NewsModel GetById(int id);
        NewsModel GetByUrl(string url);
        List<NewsModel> Get(NewsFilterModel filter, ListPaginationModel pagination);
        int GetCount(NewsFilterModel filter);
    }
}
