using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woodford.Core.DomainModel.Common;
using Woodford.Core.DomainModel.Models;
using Woodford.Core.Interfaces;

namespace Woodford.Core.ApplicationServices.Queries {
    public class NewsGetQuery : IQuery<ListOf<NewsModel>> {
        public NewsFilterModel Filter { get; set; }
        public ListPaginationModel Pagination { get; set; }
    }

    public class NewsGetQueryHandler : IQueryHandler<NewsGetQuery, ListOf<NewsModel>> {
        private readonly INewsService _newsService;
        public NewsGetQueryHandler(INewsService newsService) {
            _newsService = newsService;
        }

        public ListOf<NewsModel> Process(NewsGetQuery query) {
            return _newsService.Get(query.Filter, query.Pagination);
        }
    }
}
