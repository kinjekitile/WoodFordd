using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class NewsCategoryGetByUrlQuery : IQuery<NewsCategoryModel> {
        public string Url { get; set; }
        public ListPaginationModel Pagination { get; set; }
        public bool IncludeArticles { get; set; }
    }

    public class NewsCategoryGetByUrlQueryHandler : IQueryHandler<NewsCategoryGetByUrlQuery, NewsCategoryModel> {
        private readonly INewsCategoryService _newsCategoryService;
        public NewsCategoryGetByUrlQueryHandler(INewsCategoryService newsService) {
            _newsCategoryService = newsService;
        }

        public NewsCategoryModel Process(NewsCategoryGetByUrlQuery query) {
            return _newsCategoryService.GetByUrl(query.Url, query.IncludeArticles, query.Pagination);
        }
    }
}
