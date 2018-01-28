using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class NewsCategoryGetQuery : IQuery<ListOf<NewsCategoryModel>> {
        public NewsCategoryFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class NewsCategoryGetQueryHandler : IQueryHandler<NewsCategoryGetQuery, ListOf<NewsCategoryModel>> {
        private readonly INewsCategoryService _newsCategoryService;
        public NewsCategoryGetQueryHandler(INewsCategoryService newsService) {
            _newsCategoryService = newsService;
        }

        public ListOf<NewsCategoryModel> Process(NewsCategoryGetQuery query) {
            return _newsCategoryService.Get(query.Filter, query.Pagination);
        }
    }
}
