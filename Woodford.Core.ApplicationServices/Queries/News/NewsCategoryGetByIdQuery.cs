using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class NewsCategoryGetByIdQuery : IQuery<NewsCategoryModel> {
        public int Id { get; set; }
        public ListPaginationModel Pagination { get; set; }
        public bool IncludeArticles { get; set; }
    }

    public class NewsCategoryGetByIdQueryHandler : IQueryHandler<NewsCategoryGetByIdQuery, NewsCategoryModel> {
        private readonly INewsCategoryService _newsCategoryService;
        public NewsCategoryGetByIdQueryHandler(INewsCategoryService newsService) {
            _newsCategoryService = newsService;
        }

        public NewsCategoryModel Process(NewsCategoryGetByIdQuery query) {
            return _newsCategoryService.GetById(query.Id, query.IncludeArticles, query.Pagination);
        }
    }
}
